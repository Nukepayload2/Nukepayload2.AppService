Imports Newtonsoft.Json
Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core.InteropServices.Primitives
Imports Nukepayload2.AppService.Core.ObjectModel
Imports Nukepayload2.AppService.Core.Serialization

''' <summary>
''' 用于进行远程数据绑定。
''' </summary>
Public Class RemoteDataBinding

    WithEvents Connection As IConnection

    Private _commandFactory As CommandFactory
    Private _timestamps As New Dictionary(Of String, Date)
    Private _dataContext As New Dictionary(Of String, ISynchronizationViewModel)
    Private _handlers As New Dictionary(Of String, PropertyChangedEventHandler)

    ''' <summary>
    ''' 获取是否正在同步模型
    ''' </summary>
    Public ReadOnly Property Synchronizing As Boolean
    ''' <summary>
    ''' 获取或设置参数和路径的传输格式。默认情况使用 <see cref="JsonFormatter"/> 格式。
    ''' </summary>
    Public Property Formatter As IValueFormatter = New JsonFormatter
    ''' <summary>
    ''' 添加用于同步的 ViewModel。
    ''' </summary>
    ''' <param name="name">ViewModel 的名称。建议使用程序集限定名。</param>
    ''' <param name="vm">ViewModel</param>
    ''' <exception cref="InvalidOperationException">已经开始同步，不能再添加 ViewModel。</exception>
    Public Sub AddViewModel(name As String, vm As ISynchronizationViewModel)
        If _Synchronizing Then
            Throw New InvalidOperationException("已经开始同步，不能再添加 ViewModel。")
        End If
        _dataContext.Add(name, vm)
        _timestamps.Add(name, Date.Now)
    End Sub
    ''' <summary>
    ''' 创建新的 远程数据绑定。
    ''' </summary>
    ''' <param name="connection">使用的连接。</param>
    ''' <param name="commandFactory">新建命令存储的工厂。</param>
    ''' <exception cref="ArgumentNullException">两个参数都不能为空。</exception>
    Public Sub New(connection As IConnection, commandFactory As CommandFactory)
        If connection Is Nothing Then
            Throw New ArgumentNullException(NameOf(connection))
        End If
        If commandFactory Is Nothing Then
            Throw New ArgumentNullException(NameOf(commandFactory))
        End If

        Me.Connection = connection
        _commandFactory = commandFactory
    End Sub

    Private Sub Connection_ModelReceived(sender As Object, e As ModelSynchronizationEventArgs) Handles Connection.ModelReceived
        If _dataContext.ContainsKey(e.ModelName) Then
            Dim mdl = _dataContext(e.ModelName)
            mdl.IsBusy = True
            JsonConvert.PopulateObject(e.ModelJson, mdl)
            If Not mdl.IsBusy Then
                Throw New InvalidOperationException("模型必须先添加忙碌状态再同步。")
            End If
            mdl.IsBusy = False
            Dim remoteTime = e.Timestamp.Value
            Dim localTime = _timestamps(e.ModelName)
            If localTime < remoteTime Then
                _timestamps(e.ModelName) = e.Timestamp.Value
            End If
        Else
            Throw New InvalidOperationException($"两端的模型字典内容不同步。未找到 {e.ModelName}。")
        End If
    End Sub

    Private Sub Connection_ModelRequested(sender As Object, e As ModelSynchronizationEventArgs) Handles Connection.ModelRequested
        Dim modelName As String = e.ModelName
        If _dataContext.ContainsKey(modelName) Then
            Dim pkg As PushModelCommand = CreatePushCommand(modelName)
            e.ReturnCommand.Value = pkg
        Else
            Throw New InvalidOperationException($"两端的模型字典内容不同步。未找到 {modelName}。")
        End If
    End Sub

    Private Function CreatePushCommand(modelName As String) As PushModelCommand
        Dim mdl = _dataContext(modelName)
        mdl.IsBusy = True
        Dim json = JsonConvert.SerializeObject(mdl)
        mdl.IsBusy = False
        Dim pkg = _commandFactory.CreatePushModel
        With pkg
            .ModelName = modelName
            .ModelJson = json
            .Timestamp = Date.Now
            .ParameterFormatter = Formatter
        End With
        Return pkg
    End Function

    Private Sub Connection_ModelTimestampRequested(sender As Object, e As ModelSynchronizationEventArgs) Handles Connection.ModelTimestampRequested
        If _dataContext.ContainsKey(e.ModelName) Then
            Dim pkg = _commandFactory.CreateSetModelTimestamp
            With pkg
                .ModelName = e.ModelName
                .Timestamp = _timestamps(e.ModelName)
                .ParameterFormatter = Formatter
            End With
            e.ReturnCommand.Value = pkg
        Else
            Throw New InvalidOperationException($"两端的模型字典内容不同步。未找到 {e.ModelName}。")
        End If
    End Sub
    ''' <summary>
    ''' 开始监听远程数据绑定信息，并尝试从远端拉取数据。
    ''' </summary>
    ''' <exception cref="InvalidOperationException">开始同步后调用此方法。</exception>
    ''' <exception cref="ModelSynchronizationException">拉取远程数据失败。</exception>
    Public Async Function BeginSynchronizationAndPullModelsAsync() As Task
        If _Synchronizing Then
            Throw New InvalidOperationException("重复开始同步。")
        End If
        _Synchronizing = True
        For Each dc In _dataContext
            Dim model = dc.Value
            Dim modelName = dc.Key
            Dim cmdPull = _commandFactory.CreatePullModel
            Dim resp = Await Connection.SendMessageAsync(cmdPull.RawData)
            If resp.Status = AppServiceResponseStatus.Success Then
                Dim cmdPush As New PushModelCommand(resp.Message)
                If cmdPush.CommandType <> CommandTypes.RdbPushModel Then
                    Throw New ModelSynchronizationException($"回复了错误的消息类型: {cmdPush.CommandType} 应为 {CommandTypes.RdbPushModel}")
                End If
                If cmdPush.ModelName <> modelName Then
                    Throw New ModelSynchronizationException($"回复了错误的模型: {cmdPush.ModelName} 应为 {modelName}")
                End If
                Dim json = cmdPush.ModelJson
                model.IsBusy = True
                JsonConvert.PopulateObject(json, model)
                If Not model.IsBusy Then
                    Throw New ModelSynchronizationException("模型必须先添加忙碌状态再同步。")
                End If
                model.IsBusy = False
                _timestamps(modelName) = cmdPush.Timestamp
            End If
            Dim handler As PropertyChangedEventHandler =
                Async Sub(sender, e)
                    Dim push = CreatePushCommand(modelName)
                    Await Connection.SendMessageAsync(push.RawData)
                End Sub
            AddHandler model.PropertyChanged, handler
            _handlers.Add(dc.Key, handler)
        Next
    End Function
    ''' <summary>
    ''' 停止监听远程数据绑定信息。
    ''' </summary>
    ''' <exception cref="InvalidOperationException">尚未开始同步时此方法。</exception>
    Public Sub EndSynchronization()
        If Not _Synchronizing Then
            Throw New InvalidOperationException("尚未开始同步。")
        End If
        _Synchronizing = False
        For Each dc In _dataContext
            Dim mdl = dc.Value
            Dim handler = _handlers(dc.Key)
            RemoveHandler mdl.PropertyChanged, handler
            _handlers.Remove(dc.Key)
        Next
    End Sub
End Class
