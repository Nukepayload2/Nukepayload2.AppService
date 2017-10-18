''' <summary>
''' 由远端的异常映射来的异常。
''' </summary>
Public Class RemoteException
    Inherits Exception

    Public Sub New(typeName As String, errorCode As Integer, description As String, detail As String, handled As Boolean)
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
