Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core.InteropServices.Primitives

''' <summary>
''' 为远端异常事件提供数据。
''' </summary>
Public Class RemoteExceptionEventArgs
    Inherits CommandEventArgs

    Public Sub New(status As AppServiceResponseStatus, commandType As CommandTypes, rawData As IDictionary(Of String, Object), returnCommand As StrongBox(Of ValueSetCommand),
                   typeName As String, errorCode As Integer, description As String, detail As String, handled As Boolean)
        MyBase.New(status, commandType, rawData, returnCommand)
        Me.TypeName = typeName
        Me.ErrorCode = errorCode
        Me.Description = description
        Me.Detail = detail
        Me.Handled = handled
    End Sub

    ''' <summary>
    ''' 远端异常的类型的命名空间限定名称。
    ''' </summary>
    Public ReadOnly Property TypeName As String
    ''' <summary>
    ''' 异常码。
    ''' </summary>
    Public ReadOnly Property ErrorCode As Integer
    ''' <summary>
    ''' 异常的描述。
    ''' </summary>
    Public ReadOnly Property Description As String
    ''' <summary>
    ''' 异常的详情。通常是堆栈追踪。
    ''' </summary>
    Public ReadOnly Property Detail As String
    ''' <summary>
    ''' 指示这个异常是否已经在远端处理以防止程序崩溃。
    ''' </summary>
    Public ReadOnly Property Handled As Boolean
End Class
