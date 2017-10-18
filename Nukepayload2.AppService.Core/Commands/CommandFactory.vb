Namespace Commands
    ''' <summary>
    ''' 命令工厂。
    ''' </summary>
    Public Class CommandFactory
        ''' <summary>
        ''' 使用创建命令存储的委托创建命令工厂。
        ''' </summary>
        ''' <param name="createEmptyValueBagCallback">创建命令存储的委托。不能为空。</param>
        ''' <exception cref="ArgumentNullException">创建命令存储的委托不能为空。</exception>
        Public Sub New(createEmptyValueBagCallback As Func(Of IDictionary(Of String, Object)))
            If createEmptyValueBagCallback Is Nothing Then
                Throw New ArgumentNullException(NameOf(createEmptyValueBagCallback))
            End If

            CreateEmptyValueBag = createEmptyValueBagCallback
        End Sub
        ''' <summary>
        ''' 创建一个新的命令数据存储。在 AppService 是 ValueSet, 在传统桌面是 <see cref="Dictionary(Of String, Object)"/>。
        ''' </summary>
        Public ReadOnly Property CreateEmptyValueBag As Func(Of IDictionary(Of String, Object))
        ''' <summary>
        ''' 创建连接测试命令。
        ''' </summary>
        Public Function CreateConnectionTest() As ConnectionTestCommand
            Return New ConnectionTestCommand(CreateEmptyValueBag.Invoke) With {.CommandType = CommandTypes.ConnectionTest}
        End Function
        ''' <summary>
        ''' 创建调用返回命令。
        ''' </summary>
        Public Function CreateFunctionOrPropertyReturned() As FunctionOrPropertyReturnedCommand
            Return New FunctionOrPropertyReturnedCommand(CreateEmptyValueBag.Invoke) With {.CommandType = CommandTypes.NotifyCompletionWithReturnValue}
        End Function
        ''' <summary>
        ''' 创建模型时间戳获取命令。
        ''' </summary>
        Public Function CreateGetModelTimestamp() As GetModelTimestampCommand
            Return New GetModelTimestampCommand(CreateEmptyValueBag.Invoke) With {.CommandType = CommandTypes.RdbGetModelTimestamp}
        End Function
        ''' <summary>
        ''' 创建方法调用或属性访问命令。
        ''' </summary>
        Public Function CreateMethodCallOrPropertyAccess() As MethodCallOrPropertyAccessCommand
            Return New MethodCallOrPropertyAccessCommand(CreateEmptyValueBag.Invoke) With {.CommandType = CommandTypes.MethodCallOrPropertyAccess}
        End Function
        ''' <summary>
        ''' 创建调用完成命令。
        ''' </summary>
        Public Function CreateProcessCallCompleted() As ProcedureCallCompletedCommand
            Return New ProcedureCallCompletedCommand(CreateEmptyValueBag.Invoke) With {.CommandType = CommandTypes.NotifyCompletion}
        End Function
        ''' <summary>
        ''' 创建拉取模型的命令。
        ''' </summary>
        Public Function CreatePullModel() As PullModelCommand
            Return New PullModelCommand(CreateEmptyValueBag.Invoke) With {.CommandType = CommandTypes.RdbPullModel}
        End Function
        ''' <summary>
        ''' 创建推送模型的命令。
        ''' </summary>
        Public Function CreatePushModel() As PushModelCommand
            Return New PushModelCommand(CreateEmptyValueBag.Invoke) With {.CommandType = CommandTypes.RdbPushModel}
        End Function
        ''' <summary>
        ''' 创建捕获到异常的命令。
        ''' </summary>
        Public Function CreateReportException() As ReportExceptionCommand
            Return New ReportExceptionCommand(CreateEmptyValueBag.Invoke) With {.CommandType = CommandTypes.ReportError}
        End Function
        ''' <summary>
        ''' 创建未捕获到异常的通知命令。
        ''' </summary>
        Public Function CreateReportUnhandledException() As ReportUnhandledExceptionCommand
            Return New ReportUnhandledExceptionCommand(CreateEmptyValueBag.Invoke) With {.CommandType = CommandTypes.ReportCriticalError}
        End Function
        ''' <summary>
        ''' 创建设置模型时间戳回复命令。
        ''' </summary>
        Public Function CreateSetModelTimestamp() As SetModelTimestampCommand
            Return New SetModelTimestampCommand(CreateEmptyValueBag.Invoke) With {.CommandType = CommandTypes.RdbSetModelTimestamp}
        End Function
    End Class

End Namespace