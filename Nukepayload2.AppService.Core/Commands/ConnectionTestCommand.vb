Imports Nukepayload2.AppService.Core.Attributes

Namespace Commands
    ''' <summary>
    ''' 连接测试命令
    ''' </summary>
    <DefaultCommandType(CommandTypes.ConnectionTest)>
    Public Class ConnectionTestCommand
        Inherits ValueSetCommand

        ''' <summary>
        ''' 创建连接测试命令。
        ''' </summary>
        ''' <param name="storage">数据包</param>
        ''' <exception cref="ArgumentNullException">数据存储不能为空。</exception>
        Public Sub New(storage As IDictionary(Of String, Object))
            MyBase.New(storage)
        End Sub

    End Class
End Namespace