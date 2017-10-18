Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core.InteropServices.Primitives

''' <summary>
''' 为调用完成事件提供数据。
''' </summary>
Public Class CallCompletedEventArgs
    Inherits CommandEventArgs

    Public Sub New(status As AppServiceResponseStatus, commandType As CommandTypes, rawData As IDictionary(Of String, Object), returnCommand As StrongBox(Of ValueSetCommand), args() As Object)
        MyBase.New(status, commandType, rawData, returnCommand)
        Me.Args = args
    End Sub
    ''' <summary>
    ''' 复制回来的参数。如果被调用的方法有按引用传递的情况，参数可能与调用前不一致。
    ''' </summary>
    Public ReadOnly Property Args As Object()
End Class
