Namespace ObjectModel

    ''' <summary>
    ''' 表示用于同步数据的视图模型
    ''' </summary>
    Public Interface ISynchronizationViewModel
        Inherits INotifyPropertyChanged
        ''' <summary>
        ''' 是否处于繁忙状态。同步数据时会处于这个状态。
        ''' </summary>
        Property IsBusy As Boolean
        ''' <summary>
        ''' 最后一次发生的异常的描述。
        ''' </summary>
        Property LastErrorText As String
    End Interface

End Namespace