Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core.InteropServices.Primitives

''' <summary>
''' 为连接测试事件提供数据。
''' </summary>
Public Class ConnectionTestEventArgs
    Inherits CommandEventArgs

    Public Sub New(status As AppServiceResponseStatus, commandType As CommandTypes, rawData As IDictionary(Of String, Object), returnCommand As StrongBox(Of ValueSetCommand))
        MyBase.New(status, commandType, rawData, returnCommand)
    End Sub
End Class
