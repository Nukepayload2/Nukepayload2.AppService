Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core.InteropServices.Primitives

''' <summary>
''' 为通信发生错误事件提供数据。
''' </summary>
Public Class CommunicationErrorEventArgs
    Inherits CommandEventArgs

    Public Sub New(status As AppServiceResponseStatus, commandType As CommandTypes, rawData As IDictionary(Of String, Object), returnCommand As StrongBox(Of ValueSetCommand), description As String)
        MyBase.New(status, commandType, rawData, returnCommand)
        Me.Description = description
    End Sub

    ''' <summary>
    ''' 异常的描述。通常包含应用服务状态。
    ''' </summary>
    Public ReadOnly Property Description As String
End Class
