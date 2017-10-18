Imports Nukepayload2.AppService.Core.Commands

''' <summary>
''' 用来支持远程连接 API。不建议直接访问这个类。
''' </summary>
Public Class ConnectionHandShakeHandler

    WithEvents Connection As IConnection

    Public Sub New(connection As IConnection)
        Me.Connection = connection
    End Sub

    Private Sub Connection_RemoteConnectionTesting(sender As Object, e As CommandEventArgs) Handles Connection.RemoteConnectionTesting
        e.ReturnCommand.Value = New ConnectionTestCommand(e.RawData)
    End Sub
End Class
