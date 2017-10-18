Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core.InteropServices

''' <summary>
''' 消息中介将合并两个连接的端点。
''' </summary>
Public Class MessageMediator
    WithEvents SourceConnection As IConnection
    WithEvents DestConnection As IConnection

    Private _sourceCommandPackageFactory, _destCommandPackageFactory As CommandFactory

    Public Sub New(sourceCommandPackageFactory As CommandFactory, destCommandPackageFactory As CommandFactory)
        _sourceCommandPackageFactory = sourceCommandPackageFactory
        _destCommandPackageFactory = destCommandPackageFactory
    End Sub

    Public Sub Begin(sourceConnection As IConnection, destConnection As IConnection)
        Me.SourceConnection = sourceConnection
        Me.DestConnection = destConnection
    End Sub

    Private Sub DestConnection_MessageRelayPreview(sender As Object, e As MessageRelayEventArgs) Handles DestConnection.MessageRelayPreview
        e.RelayTask = RelayMessageAsync(SourceConnection, e.Message, _sourceCommandPackageFactory, _destCommandPackageFactory)
    End Sub

    Private Shared Async Function RelayMessageAsync(src As IConnection, message As IDictionary(Of String, Object), commandPackageFactory As CommandFactory, commandPackageFactoryRev As CommandFactory) As Task(Of IDictionary(Of String, Object))
        ' 命令跨越代理边界时必须转换格式。
        message = Marshal.ConvertMessage(message, commandPackageFactory)
        Dim resp = Await src.SendMessageAsync(message)
        If resp.Status = Primitives.AppServiceResponseStatus.Success Then
            Dim respMsg = resp.Message
            If resp Is Nothing Then
                Throw New InvalidOperationException("回复不得为空。")
            End If
            ' 命令跨越代理边界时必须转换格式。
            Return Marshal.ConvertMessage(respMsg, commandPackageFactoryRev)
        Else
            Throw New InvalidOperationException("转发消息时引发了传输错误。")
        End If
    End Function

    Private Sub SourceConnection_MessageRelayPreview(sender As Object, e As MessageRelayEventArgs) Handles SourceConnection.MessageRelayPreview
        e.RelayTask = RelayMessageAsync(DestConnection, e.Message, _destCommandPackageFactory, _sourceCommandPackageFactory)
    End Sub
End Class
