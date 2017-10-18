Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core.InteropServices.Primitives

''' <summary>
''' 为方法调用或属性访问事件提供数据。
''' </summary>
Public Class MethodCallOrPropertyAccessRequestedEventArgs
    Inherits CommandEventArgs

    Public Sub New(status As AppServiceResponseStatus, commandType As CommandTypes, rawData As IDictionary(Of String, Object), returnCommand As StrongBox(Of ValueSetCommand),
                   contextName As String, path As String, isSetProperty As Boolean, hasReturnValue As Boolean, args() As Object)
        MyBase.New(status, commandType, rawData, returnCommand)
        Me.ContextName = contextName
        Me.Path = path
        Me.IsSetProperty = isSetProperty
        Me.HasReturnValue = hasReturnValue
        Me.Args = args
    End Sub
    ''' <summary>
    ''' 上下文的名称。
    ''' </summary>
    Public ReadOnly Property ContextName As String
    ''' <summary>
    ''' 路径表达式。
    ''' </summary>
    Public ReadOnly Property Path As String
    ''' <summary>
    ''' 这个请求是不是设置属性。
    ''' </summary>
    Public ReadOnly Property IsSetProperty As Boolean
    ''' <summary>
    ''' 有没有返回值。
    ''' </summary>
    Public ReadOnly Property HasReturnValue As Boolean
    ''' <summary>
    ''' 传来的参数。
    ''' </summary>
    Public ReadOnly Property Args As Object()
End Class
