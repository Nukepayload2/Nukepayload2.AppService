''' <summary>
''' 在模型同步时引发了异常。
''' </summary>
Public Class ModelSynchronizationException
    Inherits Exception
    ''' <summary>
    ''' 使用异常描述创建在模型同步时引发的异常。
    ''' </summary>
    ''' <param name="description"></param>
    Public Sub New(description As String)
        Me.Description = description
    End Sub
    ''' <summary>
    ''' 异常的描述。通常包含通信状态。
    ''' </summary>
    Public ReadOnly Property Description As String
End Class
