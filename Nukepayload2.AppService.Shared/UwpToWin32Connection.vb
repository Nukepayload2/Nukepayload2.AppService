Imports Windows.ApplicationModel.AppService
Imports Windows.ApplicationModel.Background
''' <summary>
''' 缓存 Win32 端发起的连接，处理 Win32 端发的数据。
''' </summary>
Public Class UwpToWin32Connection
    Inherits WinRTConnectionBase

    ''' <summary>
    ''' 表示应用服务后台任务的完成情况
    ''' </summary>
    Private _deferral As BackgroundTaskDeferral

    ''' <summary>
    ''' 当前后台任务
    ''' </summary>
    WithEvents CurrentTaskInstance As IBackgroundTaskInstance

    ''' <summary>
    ''' 后台任务的入口点
    ''' </summary>
    ''' <param name="taskInstance">当前后台任务</param>
    Public Sub Run(taskInstance As IBackgroundTaskInstance)
        _deferral = taskInstance.GetDeferral
        CurrentTaskInstance = taskInstance
        Dim details = DirectCast(taskInstance.TriggerDetails, AppServiceTriggerDetails)
        AppServiceConnection = details.AppServiceConnection
        'ModelSynchronizationHelper.CurrentConnection = AppServiceConnection
    End Sub

    ''' <summary>
    ''' 后台任务停止工作。
    ''' </summary>
    Private Sub CurrentTaskInstance_Canceled(sender As IBackgroundTaskInstance, reason As BackgroundTaskCancellationReason) Handles CurrentTaskInstance.Canceled
        _deferral?.Complete()
    End Sub

    ''' <summary>
    ''' 处理应用服务被关闭。
    ''' </summary>
    Private Sub AppServiceConnection_ServiceClosed(sender As AppServiceConnection, args As AppServiceClosedEventArgs) Handles AppServiceConnection.ServiceClosed
        'ModelSynchronizationHelper.CurrentConnection = Nothing
        AppServiceConnection = Nothing
        _deferral.Complete()
    End Sub
End Class
