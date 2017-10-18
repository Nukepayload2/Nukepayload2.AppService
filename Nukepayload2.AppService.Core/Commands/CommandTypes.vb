Namespace Commands
    ''' <summary>
    ''' CentennialSignal 协议中命令的类型。
    ''' </summary>
    Public Enum CommandTypes
        ''' <summary>
        ''' 表示当前数据包不含命令, 或者命令尚未初始化。
        ''' </summary>
        Unknown
        ''' <summary>
        ''' 通知一个任务完成了
        ''' </summary>
        NotifyCompletion
        ''' <summary>
        ''' 通知一个任务完成了, 并且有返回值。
        ''' </summary>
        NotifyCompletionWithReturnValue
        ''' <summary>
        ''' 掉线后等待重连过程中用于确认连通性。
        ''' </summary>
        ConnectionTest
        ''' <summary>
        ''' 轻量级远程数据绑定中使用。能获取序列化为 Json 的某个模型类的数据。
        ''' </summary>
        RdbPullModel
        ''' <summary>
        ''' 轻量级远程数据绑定中使用。能获取序列化为 Json 的某个模型类的数据。
        ''' </summary>
        RdbPushModel
        ''' <summary>
        ''' 轻量级远程数据绑定中使用。获取某个模型的修改时间。
        ''' </summary>
        RdbGetModelTimestamp
        ''' <summary>
        ''' 回复某个模型的修改时间。
        ''' </summary>
        RdbSetModelTimestamp
        ''' <summary>
        ''' 调用一个方法。也可以用来读写属性。
        ''' </summary>
        MethodCallOrPropertyAccess
        ''' <summary>
        '''  任务执行时遇到了某个异常。 
        ''' </summary>
        ReportError
        ''' <summary>
        ''' 任务执行时遇到了未处理的异常, 并且需要将异常的情况告诉另一方。如果不需要告知另一方，应用服务的状态码会让对方知道当前应用出现了问题。
        ''' </summary>
        ReportCriticalError
    End Enum

End Namespace