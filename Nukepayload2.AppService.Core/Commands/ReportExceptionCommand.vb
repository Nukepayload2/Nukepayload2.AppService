Imports Nukepayload2.AppService.Core.Attributes

Namespace Commands
    ''' <summary>
    ''' 报告异常的命令
    ''' </summary>
    <DefaultCommandType(CommandTypes.ReportError)>
    Public Class ReportExceptionCommand
        Inherits ValueSetCommand
        ''' <summary>
        ''' 创建报告异常的命令
        ''' </summary>
        ''' <param name="storage">数据存储</param>
        ''' <exception cref="ArgumentNullException">数据存储不能为空。</exception>
        Public Sub New(storage As IDictionary(Of String, Object))
            MyBase.New(storage)
        End Sub
        ''' <summary>
        ''' 类型名称
        ''' </summary>
        Public Property TypeName As String
            Get
                Return GetValue(NameOf(TypeName))
            End Get
            Set(value As String)
                SetValue(NameOf(TypeName), value)
            End Set
        End Property
        ''' <summary>
        ''' 错误码（如果存在）
        ''' </summary>
        Public Property ErrorCode As Integer
            Get
                Return GetValue(NameOf(ErrorCode))
            End Get
            Set(value As Integer)
                SetValue(NameOf(ErrorCode), value)
            End Set
        End Property
        ''' <summary>
        ''' 错误的描述
        ''' </summary>
        Public Property Description As String
            Get
                Return GetValue(NameOf(Description))
            End Get
            Set(value As String)
                SetValue(NameOf(Description), value)
            End Set
        End Property
        ''' <summary>
        ''' 错误的详情。包含描述和堆栈追踪。
        ''' </summary>
        Public Property Detail As String
            Get
                Return GetValue(NameOf(Detail))
            End Get
            Set(value As String)
                SetValue(NameOf(Detail), value)
            End Set
        End Property

    End Class
End Namespace