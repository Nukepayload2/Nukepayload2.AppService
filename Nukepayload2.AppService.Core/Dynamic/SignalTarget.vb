Imports System.Dynamic
Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core
Imports Nukepayload2.AppService.Core.Serialization

Namespace Dynamic
    ''' <summary>
    ''' 用于动态调用 SignalContext。这是一个动态对象。
    ''' </summary>
    Public Class SignalTarget
        Inherits DynamicObject
        Implements IRemoteDynamic

        Public ReadOnly Property RemoteLateCall As RemoteLateCall

        Private _remoteLateCallHandler As RemoteLateCallHandler

        Public Sub New(connection As IConnection, commandFactory As CommandFactory)
            RemoteLateCall = New RemoteLateCall(connection, commandFactory)
            _remoteLateCallHandler = New RemoteLateCallHandler(connection, commandFactory)
        End Sub

        Friend Property CurrentContextName As String Implements IRemoteDynamic.CurrentContextName
            Get
                Return RemoteLateCall.CurrentContextName
            End Get
            Set(value As String)
                RemoteLateCall.CurrentContextName = value
            End Set
        End Property

        Friend Property ParameterFormatter As IValueFormatter Implements IRemoteDynamic.ParameterFormatter
            Get
                Return RemoteLateCall.ParameterFormatter
            End Get
            Set(value As IValueFormatter)
                RemoteLateCall.ParameterFormatter = value
            End Set
        End Property

        Public Overrides Function TryGetMember(binder As GetMemberBinder, ByRef result As Object) As Boolean
            Dim propName = binder.Name
            result = GetMember(propName)
            Return True
        End Function

        Private Function GetMember(path As String) As Object
            Dim result As Object
            Dim getTask = RemoteLateCall.ReadPropertyOrCallFunctionAsync(path, Nothing)
            If path = "LastActionAsync" Then
                ' 这是保留的属性名。
                result = LastActionAsync
            Else
                result = getTask
            End If
            Return result
        End Function

        Public Overrides Function TrySetMember(binder As SetMemberBinder, value As Object) As Boolean
            Dim propName = binder.Name
            SetMember(value, propName)
            Return True
        End Function

        Private Sub SetMember(value As Object, path As String)
            If path = "LastActionAsync" Then
                Throw New InvalidOperationException("尝试动态写入保留的属性。")
            End If
            _LastActionAsync = RemoteLateCall.WritePropertyAsync(path, value)
        End Sub

        Public Overrides Function TryInvokeMember(binder As InvokeMemberBinder, args() As Object, ByRef result As Object) As Boolean
            result = If(binder.ReturnType Is GetType(Void),
                        RemoteLateCall.CallSubAsync(binder.Name, args),
                        RemoteLateCall.ReadPropertyOrCallFunctionAsync(binder.Name, args))
            Return True
        End Function

        Public Overrides Function TryGetIndex(binder As GetIndexBinder, indexes() As Object, ByRef result As Object) As Boolean
            CheckIndexCount(indexes)
            Dim propName$ = indexes.First
            result = GetMember(propName)
            Return True
        End Function

        Public Overrides Function TrySetIndex(binder As SetIndexBinder, indexes() As Object, value As Object) As Boolean
            CheckIndexCount(indexes)
            Dim propName$ = indexes.First
            SetMember(value, propName)
            Return True
        End Function

        Private Shared Sub CheckIndexCount(indexes() As Object)
            If indexes.Length <> 1 Then
                Throw New ArgumentException("设置索引属性只能有一个参数，用于指定 Path 表达式。", NameOf(indexes))
            End If
        End Sub

        Friend ReadOnly Property LastActionAsync As Task Implements IRemoteDynamic.LastActionAsync
    End Class

End Namespace