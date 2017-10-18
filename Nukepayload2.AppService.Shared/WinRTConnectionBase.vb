Imports Nukepayload2.AppService.Core
Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core.InteropServices
Imports Windows.ApplicationModel.AppService

Public Class WinRTConnectionBase
    Inherits ConnectionBase

    ''' <summary>
    ''' 缓存的应用服务连接
    ''' </summary>
    Protected Friend WithEvents AppServiceConnection As AppServiceConnection

    Private Async Sub AppServiceConnection_RequestReceivedAsync(sender As AppServiceConnection, args As AppServiceRequestReceivedEventArgs) Handles AppServiceConnection.RequestReceived
        Dim def = args.GetDeferral
        Dim msg = DirectCast(args.Request.Message, IDictionary(Of String, Object))
        Dim resp = Await HandleRemoteMessageAsync(msg)
        If resp IsNot Nothing Then
            Dim stat = Await args.Request.SendResponseAsync(resp)
            If stat <> AppServiceResponseStatus.Success Then
                RaiseCommunicationError(stat.AsN2AppServiceResponseStatus)
            End If
        End If
        def.Complete()
    End Sub

    ''' <summary>
    ''' 使用当前应用连接发送数据。
    ''' </summary>
    ''' <param name="data">要发送的数据</param>
    Public Async Function SendAsync(data As IDictionary(Of String, Object)) As Task(Of AppServiceResponse)
        Return Await AppServiceConnection.SendMessageAsync(data)
    End Function

    Public Overrides Async Function SendMessageAsync(msg As IDictionary(Of String, Object)) As Task(Of Primitives.RemoteResponse)
        Dim resp = Await SendAsync(msg)
        If resp.Status <> AppServiceResponseStatus.Success Then
            RaiseCommunicationError(resp.Status)
            Return Nothing
        Else
            Return resp.AsRemoteResponse
        End If
    End Function

    Public Overrides ReadOnly Property CommandFactory As New CommandFactory(Function() New ValueSet)
End Class
