Namespace InteropServices.Primitives
    ''' <summary>
    ''' 代表 AppServiceResponse。
    ''' </summary>
    Public Class RemoteResponse
        Public Sub New(message As IDictionary(Of String, Object), status As AppServiceResponseStatus)
            Me.Message = message
            Me.Status = status
        End Sub
        ''' <summary>
        ''' 回应的消息。
        ''' </summary>
        Public ReadOnly Property Message As IDictionary(Of String, Object)
        ''' <summary>
        ''' 回应的状态。
        ''' </summary>
        Public ReadOnly Property Status As AppServiceResponseStatus
    End Class
End Namespace
