Imports Nukepayload2.AppService.Core.Attributes

Namespace Commands
    ''' <summary>
    ''' 要求响应调用完成的命令
    ''' </summary> 
    <DefaultCommandType(CommandTypes.NotifyCompletion)>
    Public Class ProcedureCallCompletedCommand
        Inherits RemoteReflectionCommandBase

        ''' <summary>
        ''' 创建要求响应调用完成的命令
        ''' </summary>
        ''' <param name="storage">数据存储</param>
        ''' <exception cref="ArgumentNullException">数据存储不能为空。</exception>
        Public Sub New(storage As IDictionary(Of String, Object))
            MyBase.New(storage)
        End Sub

    End Class
End Namespace