Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core
Imports Nukepayload2.AppService.Core.Scripting

Namespace Dynamic

    ''' <summary>
    ''' 用来支持远程动态调用 API。不建议直接访问这个类。
    ''' </summary>
    Friend Class RemoteLateCallHandler

        WithEvents Connection As IConnection

        Private _scriptEngine As New PathScriptEngine

        Public ReadOnly Property CommandFactory As CommandFactory

        Public Sub New(connection As IConnection, commandFactory As CommandFactory)
            If connection Is Nothing Then
                Throw New ArgumentNullException(NameOf(connection))
            End If
            If commandFactory Is Nothing Then
                Throw New ArgumentNullException(NameOf(commandFactory))
            End If

            Me.Connection = connection
            Me.CommandFactory = commandFactory
        End Sub

        Private Sub Connection_MethodCallOrPropertyAccessRequested(sender As Object, e As MethodCallOrPropertyAccessRequestedEventArgs) Handles Connection.MethodCallOrPropertyAccessRequested
            Dim context = SignalContextManager.GetSignalContext(e.ContextName)
            Dim localException As RemoteException = Nothing
            Dim returnValue As Object = Nothing
            Try
                returnValue = _scriptEngine.Execute(e.Path, context, e.Args, e.IsSetProperty)
            Catch ex As Exception
                localException = New RemoteException(ex.GetType.AssemblyQualifiedName, ex.HResult, ex.Message, ex.StackTrace, True)
            End Try
            Dim cmdBack As ValueSetCommand
            If localException Is Nothing Then
                If e.HasReturnValue Then
                    Dim cmd = CommandFactory.CreateFunctionOrPropertyReturned
                    With cmd
                        .InitializeArgs(e.Args)
                        .ReturnValue = returnValue
                    End With
                    cmdBack = cmd
                Else
                    Dim cmd = CommandFactory.CreateProcessCallCompleted
                    cmd.InitializeArgs(e.Args)
                    cmdBack = cmd
                End If
            Else
                Dim cmd = CommandFactory.CreateReportException
                With cmd
                    .Description = localException.Description
                    .Detail = localException.Detail
                    .ErrorCode = localException.ErrorCode
                    .TypeName = localException.TypeName
                End With
                cmdBack = cmd
            End If
            e.ReturnCommand.Value = cmdBack
        End Sub

    End Class

End Namespace