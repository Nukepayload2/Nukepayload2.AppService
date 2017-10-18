Namespace Commands
    ''' <summary>
    ''' 同步模型的命令的基类
    ''' </summary>
    Public MustInherit Class SynchronizeModelCommand
        Inherits ValueSetCommand
        ''' <summary>
        ''' 用于构造同步模型的命令
        ''' </summary>
        ''' <param name="storage">数据存储</param>
        Public Sub New(storage As IDictionary(Of String, Object))
            MyBase.New(storage)
        End Sub
        ''' <summary>
        ''' 模型的名称
        ''' </summary>
        Public Property ModelName As String
            Get
                Return GetValue(NameOf(ModelName))
            End Get
            Set(value As String)
                SetValue(NameOf(ModelName), value)
            End Set
        End Property
        ''' <summary>
        ''' 时间戳
        ''' </summary>
        Public Property Timestamp As Date
            Get
                Return GetValue(NameOf(Timestamp))
            End Get
            Set(value As Date)
                SetValue(NameOf(Timestamp), value)
            End Set
        End Property
    End Class
End Namespace