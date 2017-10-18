''' <summary>
''' 为消息转发事件提供数据。
''' </summary>
Public Class MessageRelayEventArgs
    Inherits EventArgs

    Public Sub New(message As IDictionary(Of String, Object))
        Me.Message = message
    End Sub
    ''' <summary>
    ''' 被转发的消息。
    ''' </summary>
    Public ReadOnly Property Message As IDictionary(Of String, Object)
    ''' <summary>
    ''' 转发消息的任务。会返回回复。
    ''' </summary>
    ''' <returns>远程的回复。</returns>
    Public Property RelayTask As Task(Of IDictionary(Of String, Object))
End Class
