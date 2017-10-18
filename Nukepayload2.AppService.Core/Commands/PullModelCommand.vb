Imports Nukepayload2.AppService.Core.Attributes

Namespace Commands
    ''' <summary>
    ''' 在轻量级远程数据绑定中拉取模型数据的命令
    ''' </summary>
    <DefaultCommandType(CommandTypes.RdbPullModel)>
    Public Class PullModelCommand
        Inherits SynchronizeModelCommand
        ''' <summary>
        ''' 创建在轻量级远程数据绑定中拉取模型数据的命令
        ''' </summary>
        ''' <param name="storage">数据存储</param>
        ''' <exception cref="ArgumentNullException">数据存储不能为空。</exception>
        Public Sub New(storage As IDictionary(Of String, Object))
            MyBase.New(storage)
        End Sub

    End Class
End Namespace