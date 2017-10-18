Imports Nukepayload2.AppService.Core.Attributes

Namespace Commands
    ''' <summary>
    ''' 方法调用或属性访问的命令
    ''' </summary>
    <DefaultCommandType(CommandTypes.MethodCallOrPropertyAccess)>
    Public Class MethodCallOrPropertyAccessCommand
        Inherits RemoteReflectionCommandBase

        ''' <summary>
        ''' 创建方法调用或属性访问的命令
        ''' </summary>
        ''' <param name="storage">数据存储</param>
        ''' <exception cref="ArgumentNullException">数据存储不能为空。</exception>
        Public Sub New(storage As IDictionary(Of String, Object))
            MyBase.New(storage)
        End Sub

        ''' <summary>
        ''' 要访问的上下文对象的名称。
        ''' </summary>
        Public Property ContextName As String
            Get
                Return GetValue(NameOf(ContextName))
            End Get
            Set(value As String)
                SetValue(NameOf(ContextName), value)
            End Set
        End Property

        ''' <summary>
        ''' 要访问的路径。注意: 路径中可能包含参数，所以也会使用处理参数的方式序列化和反序列化。
        ''' </summary>
        Public Property Path As String
            Get
                Return Deserialize(GetType(String), NameOf(Path))
            End Get
            Set(value As String)
                Serialize(NameOf(Path), value)
            End Set
        End Property

        ''' <summary>
        ''' 这个操作是不是为属性赋值的
        ''' </summary>
        Public Property IsSetProperty As Boolean
            Get
                Return GetValue(NameOf(IsSetProperty))
            End Get
            Set(value As Boolean)
                SetValue(NameOf(IsSetProperty), value)
            End Set
        End Property

        ''' <summary>
        ''' 这个操作有没有返回值
        ''' </summary>
        Public Property HasReturnValue As Boolean
            Get
                Return GetValue(NameOf(HasReturnValue))
            End Get
            Set(value As Boolean)
                SetValue(NameOf(HasReturnValue), value)
            End Set
        End Property

    End Class
End Namespace