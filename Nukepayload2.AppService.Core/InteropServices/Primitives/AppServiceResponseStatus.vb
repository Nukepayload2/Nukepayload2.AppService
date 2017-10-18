Namespace InteropServices.Primitives
    ''' <summary>
    ''' 描述当应用尝试通过调用 AppServiceConnection.SendMessageAsync 方法将消息发送到应用服务时的状态。等价于 Windows.ApplicationModel.AppService.AppServiceResponseStatus。
    ''' </summary>
    Public Enum AppServiceResponseStatus
        ''' <summary>
        ''' 应用服务已成功接收和处理消息。
        ''' </summary>
        Success = 0
        ''' <summary>
        ''' 应用服务未能接收和处理消息。
        ''' </summary>
        Failure = 1
        ''' <summary>
        ''' 应用服务已退出，因为可用的资源不够。
        ''' </summary>
        ResourceLimitsExceeded = 2
        ''' <summary>
        ''' 发生未知错误。
        ''' </summary>
        Unknown = 3
        ''' <summary>
        ''' 要发送到的设备不可用。
        ''' </summary>
        RemoteSystemUnavailable = 4
        ''' <summary>
        ''' 应用服务处理消息失败，因为它太大。
        ''' </summary>
        MessageSizeTooLarge = 5
    End Enum
End Namespace