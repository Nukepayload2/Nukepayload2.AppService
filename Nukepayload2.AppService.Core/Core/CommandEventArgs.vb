Imports Nukepayload2.AppService.Core.Commands
Imports Nukepayload2.AppService.Core.InteropServices.Primitives

''' <summary>
''' 与命令相关的事件的基类。
''' </summary>
Public MustInherit Class CommandEventArgs
    Inherits EventArgs

    Public Sub New(status As AppServiceResponseStatus, commandType As CommandTypes, rawData As IDictionary(Of String, Object), returnCommand As StrongBox(Of ValueSetCommand))
        Me.Status = status
        Me.CommandType = commandType
        Me.RawData = rawData
        Me.ReturnCommand = returnCommand
    End Sub
    ''' <summary>
    ''' 用于存放命令的返回值。
    ''' </summary>
    Public ReadOnly Property ReturnCommand As StrongBox(Of ValueSetCommand)
    ''' <summary>
    ''' 当前应用服务的状态。
    ''' </summary>
    Public ReadOnly Property Status As AppServiceResponseStatus
    ''' <summary>
    ''' 命令的类型。
    ''' </summary>
    Public ReadOnly Property CommandType As CommandTypes
    ''' <summary>
    ''' 命令的原始数据。
    ''' </summary>
    Public ReadOnly Property RawData As IDictionary(Of String, Object)
End Class
