Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core.InteropServices.Primitives

''' <summary>
''' 为模型同步事件提供参数。
''' </summary>
Public Class ModelSynchronizationEventArgs
    Inherits CommandEventArgs

    Public Sub New(status As AppServiceResponseStatus, commandType As CommandTypes, rawData As IDictionary(Of String, Object), returnCommand As StrongBox(Of ValueSetCommand), modelName As String)
        MyBase.New(status, commandType, rawData, returnCommand)
        Me.ModelName = modelName
    End Sub
    ''' <summary>
    ''' 模型的 Json 数据 (可选)。
    ''' </summary>
    Public Property ModelJson As String
    ''' <summary>
    ''' 模型的名称。
    ''' </summary>
    Public ReadOnly Property ModelName As String
    ''' <summary>
    ''' 时间戳 (可选)。
    ''' </summary>
    Public Property Timestamp As Date?
End Class
