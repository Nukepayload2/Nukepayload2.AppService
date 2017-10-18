Imports Nukepayload2.AppService.Core.Attributes

Namespace Commands
    ''' <summary>
    ''' 报告未处理的异常的命令
    ''' </summary>
    <DefaultCommandType(CommandTypes.ReportCriticalError)>
    Public Class ReportUnhandledExceptionCommand
        Inherits ReportExceptionCommand
        ''' <summary>
        ''' 创建报告未处理的异常的命令
        ''' </summary>
        ''' <param name="storage">数据存储</param>
        ''' <exception cref="ArgumentNullException">数据存储不能为空。</exception>
        Public Sub New(storage As IDictionary(Of String, Object))
            MyBase.New(storage)
        End Sub
    End Class
End Namespace