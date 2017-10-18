Imports Nukepayload2.AppService.Core.Attributes

Namespace Commands
    ''' <summary>
    ''' 在轻量级远程数据绑定中推送模型数据的命令
    ''' </summary>
    <DefaultCommandType(CommandTypes.RdbPushModel)>
    Public Class PushModelCommand
        Inherits SynchronizeModelCommand
        ''' <summary>
        ''' 在轻量级远程数据绑定中推送模型数据的命令
        ''' </summary>
        ''' <param name="storage">数据存储</param>
        ''' <exception cref="ArgumentNullException">数据存储不能为空。</exception>
        Public Sub New(storage As IDictionary(Of String, Object))
            MyBase.New(storage)
        End Sub
        ''' <summary>
        ''' 模型 Json。会被预先序列化和后置反序列化。
        ''' </summary>
        Public Property ModelJson As String
            Get
                Return Deserialize(GetType(String), NameOf(ModelJson))
            End Get
            Set(value As String)
                Serialize(NameOf(ModelJson), value)
            End Set
        End Property
    End Class
End Namespace