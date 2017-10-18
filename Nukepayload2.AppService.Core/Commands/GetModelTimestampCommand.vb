Imports Nukepayload2.AppService.Core.Attributes

Namespace Commands
    ''' <summary>
    ''' 在轻量级远程数据绑定中获取模型类时间戳的命令
    ''' </summary> 
    <DefaultCommandType(CommandTypes.RdbGetModelTimestamp)>
    Public Class GetModelTimestampCommand
        Inherits ValueSetCommand

        ''' <summary>
        ''' 在轻量级远程数据绑定中获取模型类时间戳的命令
        ''' </summary>
        ''' <param name="storage">数据存储</param>
        ''' <exception cref="ArgumentNullException">数据存储不能为空。</exception>
        Public Sub New(storage As IDictionary(Of String, Object))
            MyBase.New(storage)
        End Sub
        ''' <summary>
        ''' 模型类的名称
        ''' </summary>
        Public Property ModelName As String
            Get
                Return GetValue(NameOf(ModelName))
            End Get
            Set(value As String)
                SetValue(NameOf(ModelName), value)
            End Set
        End Property
    End Class
End Namespace