Imports Nukepayload2.AppService.Core.Serialization

Namespace Dynamic
    ''' <summary>
    ''' 远程异步动态调用对象。是一种动态对象。
    ''' </summary>
    Public Interface IRemoteDynamic
        ''' <summary>
        ''' 当前上下文
        ''' </summary>
        Property CurrentContextName As String
        ''' <summary>
        ''' 用于等待上一次的调用操作
        ''' </summary>
        ReadOnly Property LastActionAsync As Task
        ''' <summary>
        ''' 指定参数和路径的传输格式
        ''' </summary>
        Property ParameterFormatter As IValueFormatter
    End Interface
End Namespace
