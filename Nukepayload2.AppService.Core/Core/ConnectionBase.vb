Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core.InteropServices.Primitives

''' <summary>
''' 连接的基类。
''' </summary>
Public MustInherit Class ConnectionBase
    Implements IConnection
    Implements ICommandCreator

    Private _handShakeHandler As New ConnectionHandShakeHandler(Me)
    ''' <summary>
    ''' 命令工厂。
    ''' </summary>
    Public MustOverride ReadOnly Property CommandFactory As CommandFactory Implements ICommandCreator.CommandFactory
    ''' <summary>
    ''' 发送消息。
    ''' </summary>
    ''' <param name="msg">要发送的消息。</param>
    Public MustOverride Function SendMessageAsync(msg As IDictionary(Of String, Object)) As Task(Of RemoteResponse) Implements IConnection.SendMessageAsync

    ''' <summary>
    ''' 处理远程的消息并返回回复。
    ''' </summary>
    ''' <exception cref="InvalidOperationException">检测到违反协议规定的调用。</exception>
    Protected Overridable Async Function HandleRemoteMessageAsync(msg As IDictionary(Of String, Object)) As Task(Of IDictionary(Of String, Object))
        If msg Is Nothing Then
            Throw New InvalidOperationException("未指定消息类型。")
        End If
        Dim relayForward As New MessageRelayEventArgs(msg)
        RaiseEvent MessageRelayPreview(Me, relayForward)
        If relayForward.RelayTask IsNot Nothing Then
            ' 对于消息中介。
            ' 向另一端发生命令。
            Dim reply = Await relayForward.RelayTask
            ' 另一端已经已执行。返回。
            If reply Is Nothing Then
                Throw New InvalidOperationException("消息转发结果不能为空。")
            End If
            Dim typeNameExpected As String = msg.GetType.Name
            Dim typeNameActual As String = reply.GetType.Name
            If typeNameActual <> typeNameExpected Then
                Throw New InvalidCastException($"返回消息应当与源消息使用同一种数据存储工厂。预期的类型是 {typeNameExpected}, 实际是 {typeNameActual}。")
            End If
            Return reply
        End If
        Dim cmd As New UnknownCommand(msg)
        Dim returnCommand As New StrongBox(Of ValueSetCommand)
        ExecuteMessage(msg, cmd, returnCommand)
        Return returnCommand.Value?.RawData
    End Function

    Private Sub ExecuteMessage(msg As IDictionary(Of String, Object), cmd As UnknownCommand, returnCommand As StrongBox(Of ValueSetCommand))
        Select Case cmd.CommandType
            Case CommandTypes.Unknown
                Throw New InvalidOperationException("指定了未初始化的消息类型。")
            Case CommandTypes.NotifyCompletion
                Throw New InvalidOperationException("不能主动推送调用完成信息。应当回复。")
            Case CommandTypes.NotifyCompletionWithReturnValue
                Throw New InvalidOperationException("不能主动推送调用返回信息。应当回复。")
            Case CommandTypes.ConnectionTest
                Dim newCmd As New ConnectionTestCommand(msg)
                RaiseEvent RemoteConnectionTesting(Me, New ConnectionTestEventArgs(AppServiceResponseStatus.Success,
                                                           newCmd.CommandType, msg, returnCommand))
            Case CommandTypes.RdbPullModel
                Dim newCmd As New PullModelCommand(msg)
                RaiseEvent ModelRequested(Me, New ModelSynchronizationEventArgs(AppServiceResponseStatus.Success,
                                                  newCmd.CommandType, msg, returnCommand, newCmd.ModelName) With {
                                                  .Timestamp = newCmd.Timestamp})
            Case CommandTypes.RdbPushModel
                Dim newCmd As New PushModelCommand(msg)
                RaiseEvent ModelReceived(Me, New ModelSynchronizationEventArgs(AppServiceResponseStatus.Success,
                                                 newCmd.CommandType, msg, returnCommand, newCmd.ModelName) With {
                                                 .ModelJson = newCmd.ModelJson, .Timestamp = newCmd.Timestamp})
            Case CommandTypes.RdbSetModelTimestamp
                Throw New InvalidOperationException("不能主动推送模型时间戳。应当回复。")
            Case CommandTypes.RdbGetModelTimestamp
                Dim newCmd As New GetModelTimestampCommand(msg)
                RaiseEvent ModelTimestampRequested(Me, New ModelSynchronizationEventArgs(AppServiceResponseStatus.Success,
                                                       newCmd.CommandType, msg, returnCommand, newCmd.ModelName))
            Case CommandTypes.MethodCallOrPropertyAccess
                Dim newCmd As New MethodCallOrPropertyAccessCommand(msg)
                Dim args = newCmd.GetArgs
                RaiseEvent MethodCallOrPropertyAccessRequested(Me, New MethodCallOrPropertyAccessRequestedEventArgs(
                                                                   AppServiceResponseStatus.Success, newCmd.CommandType, msg, returnCommand,
                                                                   newCmd.ContextName, newCmd.Path, newCmd.IsSetProperty,
                                                                   newCmd.HasReturnValue, args))
            Case CommandTypes.ReportError
                Dim newCmd As New ReportExceptionCommand(msg)
                RaiseEvent RemoteException(Me, New RemoteExceptionEventArgs(
                                               AppServiceResponseStatus.Success, newCmd.CommandType,
                                               msg, returnCommand, newCmd.TypeName, newCmd.ErrorCode,
                                               newCmd.Description, newCmd.Detail, True))
            Case CommandTypes.ReportCriticalError
                Dim newCmd As New ReportUnhandledExceptionCommand(msg)
                RaiseEvent RemoteUnhandledException(Me, New RemoteExceptionEventArgs(
                                                    AppServiceResponseStatus.Success, newCmd.CommandType,
                                                    msg, returnCommand, newCmd.TypeName, newCmd.ErrorCode,
                                                    newCmd.Description, newCmd.Detail, False))
            Case Else
                Throw New InvalidOperationException("指定了损坏的消息类型。")
        End Select
    End Sub

    Protected Sub RaiseCommunicationError(status As AppServiceResponseStatus)
        RaiseEvent CommunicationError(Me, New CommunicationErrorEventArgs(status, CommandTypes.Unknown, Nothing, New StrongBox(Of ValueSetCommand), "传输过程中出现错误"))
    End Sub

    ''' <summary>
    ''' 远端要调用属性或方法。
    ''' </summary>
    Public Event MethodCallOrPropertyAccessRequested As EventHandler(Of MethodCallOrPropertyAccessRequestedEventArgs) Implements IConnection.MethodCallOrPropertyAccessRequested
    ''' <summary>
    ''' 远端发生了异常，已处理和报告。
    ''' </summary>
    Public Event RemoteException As EventHandler(Of RemoteExceptionEventArgs) Implements IConnection.RemoteException
    ''' <summary>
    ''' 远端发生未处理的异常，并且发来了报告。这通常指示远端已经由于应用程序域发生未处理的异常而崩溃。
    ''' </summary>
    Public Event RemoteUnhandledException As EventHandler(Of RemoteExceptionEventArgs) Implements IConnection.RemoteUnhandledException
    ''' <summary>
    ''' 在传输时出现了问题。
    ''' </summary>
    Public Event CommunicationError As EventHandler(Of CommunicationErrorEventArgs) Implements IConnection.CommunicationError
    ''' <summary>
    ''' 收到远端的连接测试。
    ''' </summary>
    Public Event RemoteConnectionTesting As EventHandler(Of CommandEventArgs) Implements IConnection.RemoteConnectionTesting
    ''' <summary>
    ''' 向远端发送模型。
    ''' </summary>
    Public Event ModelRequested As EventHandler(Of ModelSynchronizationEventArgs) Implements IConnection.ModelRequested
    ''' <summary>
    ''' 收到远端发来的模型。
    ''' </summary>
    Public Event ModelReceived As EventHandler(Of ModelSynchronizationEventArgs) Implements IConnection.ModelReceived
    ''' <summary>
    ''' 请求远端的时间戳变更。
    ''' </summary>
    Public Event ModelTimestampRequested As EventHandler(Of ModelSynchronizationEventArgs) Implements IConnection.ModelTimestampRequested
    ''' <summary>
    ''' 消息分类处理之前引发此事件。用于设置转发消息。
    ''' </summary>
    Public Event MessageRelayPreview As EventHandler(Of MessageRelayEventArgs) Implements IConnection.MessageRelayPreview
End Class
