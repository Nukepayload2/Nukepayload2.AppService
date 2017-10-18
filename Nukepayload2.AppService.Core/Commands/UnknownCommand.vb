Namespace Commands
    ''' <summary>
    ''' 未知类型的命令。用于分类命令。
    ''' </summary>
    Public Class UnknownCommand
        Inherits ValueSetCommand

        ''' <exception cref="ArgumentNullException">数据存储不能为空。</exception>
        Public Sub New(storage As IDictionary(Of String, Object))
            MyBase.New(storage)
        End Sub
    End Class

End Namespace