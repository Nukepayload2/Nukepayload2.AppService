Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core
Imports Nukepayload2.AppService.Core.InteropServices.Primitives
Imports Nukepayload2.AppService.Core.Serialization

Namespace Dynamic

    ''' <summary>
    ''' 用于支持远程调用的动态 API。不建议直接用 Visual Basic 或 Visual C# 访问这个类。
    ''' </summary>
    Public Class RemoteLateCall
        ''' <summary>
        ''' 创建 远程后期调用。
        ''' </summary>
        ''' <param name="connection">使用的连接。不能为空。</param>
        ''' <param name="commandFactory">使用的命令工厂。不能为空。</param>
        ''' <exception cref="ArgumentNullException">两个参数都不能为空。</exception>
        Public Sub New(connection As IConnection, commandFactory As CommandFactory)
            If connection Is Nothing Then
                Throw New ArgumentNullException(NameOf(connection))
            End If
            If commandFactory Is Nothing Then
                Throw New ArgumentNullException(NameOf(commandFactory))
            End If

            Me.Connection = connection
            Me.CommandFactory = commandFactory
        End Sub
        ''' <summary>
        ''' 当前使用的连接。
        ''' </summary>
        Public ReadOnly Property Connection As IConnection
        ''' <summary>
        ''' 当前使用的命令工厂。
        ''' </summary>
        Public ReadOnly Property CommandFactory As CommandFactory
        ''' <summary>
        ''' 获取或设置当前已注册的 SignalContext 的名称。留空则表示使用默认的 SignalContext 。
        ''' </summary>
        Public Property CurrentContextName As String
        ''' <summary>
        ''' 参数的额外序列化和反序列化工具。
        ''' </summary>
        Public Property ParameterFormatter As IValueFormatter
        ''' <summary>
        ''' 调用没有返回值的方法。
        ''' </summary>
        ''' <param name="path">路径表达式</param>
        ''' <param name="params">方法的参数</param>
        ''' <exception cref="RemoteException">远程端引发了异常。</exception>
        ''' <exception cref="InvalidOperationException">内部错误。</exception>
        Public Async Function CallSubAsync(path As String, params As Object()) As Task
            Dim pkg = CommandFactory.CreateMethodCallOrPropertyAccess
            With pkg
                .HasReturnValue = False
                .IsSetProperty = False
                InitCallCommand(path, params, pkg)
            End With
            Dim resp = Await Connection.SendMessageAsync(pkg.RawData)
            CheckResponse(resp)
            Dim cmd As New ProcedureCallCompletedCommand(resp.Message)
            CopyParamsBack(params, cmd)
        End Function
        ''' <summary>
        ''' 写属性。
        ''' </summary>
        ''' <param name="path">路径表达式</param>
        ''' <param name="value">要写入的值</param>
        ''' <exception cref="RemoteException">远程端引发了异常。</exception>
        ''' <exception cref="InvalidOperationException">内部错误。</exception>
        Public Async Function WritePropertyAsync(path As String, value As Object) As Task
            Dim pkg = CommandFactory.CreateMethodCallOrPropertyAccess
            With pkg
                .HasReturnValue = False
                .IsSetProperty = True
                InitCallCommand(path, {value}, pkg)
            End With
            Dim resp = Await Connection.SendMessageAsync(pkg.RawData)
            CheckResponse(resp)
        End Function

        ''' <summary>
        ''' 请求读取属性或调用方法。
        ''' </summary>
        ''' <param name="path">路径表达式。</param>
        ''' <param name="params">调用的参数。如果没有，置空。</param>
        ''' <exception cref="RemoteException">远程端引发了异常。</exception>
        ''' <exception cref="InvalidOperationException">内部错误。</exception>
        Public Async Function ReadPropertyOrCallFunctionAsync(path As String, params As Object()) As Task(Of Object)
            Dim pkg = CommandFactory.CreateMethodCallOrPropertyAccess
            With pkg
                .HasReturnValue = True
                .IsSetProperty = False
                InitCallCommand(path, params, pkg)
            End With
            Dim resp = Await Connection.SendMessageAsync(pkg.RawData)
            CheckResponse(resp)
            Dim cmd As New FunctionOrPropertyReturnedCommand(resp.Message)
            CopyParamsBack(params, cmd)
            Return cmd.ReturnValue
        End Function

        Friend Shared Sub CopyParamsBack(params() As Object, cmd As RemoteReflectionCommandBase)
            If params IsNot Nothing Then
                Dim newArgs = cmd.GetArgs
                For i = 0 To params.Length - 1
                    params(i) = newArgs(i)
                Next
            End If
        End Sub

        Private Sub InitCallCommand(path As String, params() As Object, pkg As MethodCallOrPropertyAccessCommand)
            With pkg
                If ParameterFormatter IsNot Nothing Then
                    .ParameterFormatter = ParameterFormatter
                End If
                .Path = path
                .InitializeArgs(params)
                .ContextName = CurrentContextName
            End With
        End Sub

        ''' <exception cref="RemoteException">远程端引发了异常。</exception>
        ''' <exception cref="InvalidOperationException">内部错误。</exception>
        Private Sub CheckResponse(resp As RemoteResponse)
            If resp Is Nothing Then
                ' 传输错误
                Throw New RemoteException(Nothing, 5, "传输错误导致调用结果未知。", Nothing, False)
            End If
            If resp.Status = AppServiceResponseStatus.Success Then
                Dim unknownCmd As New UnknownCommand(resp.Message)
                Select Case unknownCmd.CommandType
                    Case CommandTypes.NotifyCompletion, CommandTypes.NotifyCompletionWithReturnValue
                        ' 一切正常
                    Case CommandTypes.ReportError
                        ' 执行过程中抛出了异常。映射到远程调用异常。
                        Dim cmd As New ReportExceptionCommand(resp.Message)
                        Throw New RemoteException(cmd.TypeName, cmd.ErrorCode, cmd.Description, cmd.Detail, False)
                    Case Else
                        Throw New InvalidOperationException("远程调用不能返回完成或异常之外的消息。")
                End Select
            Else
                Throw New InvalidOperationException("平台特定框架处理失败的传输。")
            End If
        End Sub

    End Class

End Namespace