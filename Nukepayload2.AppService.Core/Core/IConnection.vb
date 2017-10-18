Imports Nukepayload2.AppService.Core.InteropServices.Primitives

''' <summary>
''' 表示到远端的连接。
''' </summary>
Public Interface IConnection
    ''' <summary>
    ''' 请求方法调用或属性访问。
    ''' </summary>
    Event MethodCallOrPropertyAccessRequested As EventHandler(Of MethodCallOrPropertyAccessRequestedEventArgs)
    ''' <summary>
    ''' 收到主动推送的模型。
    ''' </summary>
    Event ModelReceived As EventHandler(Of ModelSynchronizationEventArgs)
    ''' <summary>
    ''' 拉取模型。
    ''' </summary>
    Event ModelRequested As EventHandler(Of ModelSynchronizationEventArgs)
    ''' <summary>
    ''' 请求模型时间戳。
    ''' </summary>
    Event ModelTimestampRequested As EventHandler(Of ModelSynchronizationEventArgs)
    ''' <summary>
    ''' 远端发来连接测试。
    ''' </summary>
    Event RemoteConnectionTesting As EventHandler(Of CommandEventArgs)
    ''' <summary>
    ''' 远端捕获到了异常。
    ''' </summary>
    Event RemoteException As EventHandler(Of RemoteExceptionEventArgs)
    ''' <summary>
    ''' 远端发生未处理的异常。
    ''' </summary>
    Event RemoteUnhandledException As EventHandler(Of RemoteExceptionEventArgs)
    ''' <summary>
    ''' 通信错误。
    ''' </summary>
    Event CommunicationError As EventHandler(Of CommunicationErrorEventArgs)
    ''' <summary>
    ''' 在分类消息之前引发这个事件。可以阻止消息的进一步处理。
    ''' </summary>
    Event MessageRelayPreview As EventHandler(Of MessageRelayEventArgs)
    ''' <summary>
    ''' 发送消息。
    ''' </summary>
    ''' <param name="msg">要发送的消息。</param>
    Function SendMessageAsync(msg As IDictionary(Of String, Object)) As Task(Of RemoteResponse)
End Interface
