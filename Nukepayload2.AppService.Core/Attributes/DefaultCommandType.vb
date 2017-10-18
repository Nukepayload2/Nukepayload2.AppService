Imports Nukepayload2.AppService.Core.Commands

Namespace Attributes
    ''' <summary>
    ''' 标注命令默认的类型。此类型不宜在框架用户的代码中使用。
    ''' </summary>
    <AttributeUsage(AttributeTargets.Class)>
    Public Class DefaultCommandType
        Inherits Attribute
        ''' <summary>
        ''' 创建标注命令默认的类型。
        ''' </summary>
        ''' <param name="commandType">命令的类型</param>
        Public Sub New(commandType As CommandTypes)
            Me.CommandType = commandType
        End Sub
        ''' <summary>
        ''' 命令的类型
        ''' </summary>
        Public Property CommandType As CommandTypes
    End Class

End Namespace