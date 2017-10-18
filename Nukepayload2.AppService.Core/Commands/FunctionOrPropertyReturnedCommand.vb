Imports Nukepayload2.AppService.Core.Attributes
Imports Nukepayload2.AppService.Core.InteropServices

Namespace Commands
    ''' <summary>
    ''' 要求处理方法调用或属性返回的命令
    ''' </summary> 
    <DefaultCommandType(CommandTypes.NotifyCompletionWithReturnValue)>
    Public Class FunctionOrPropertyReturnedCommand
        Inherits ProcedureCallCompletedCommand
        ''' <summary>
        ''' 创建要求处理方法调用或属性返回的命令
        ''' </summary>
        ''' <param name="storage">数据</param>
        ''' <exception cref="ArgumentNullException">数据存储不能为空。</exception>
        Public Sub New(storage As IDictionary(Of String, Object))
            MyBase.New(storage)
        End Sub

        ''' <summary>
        ''' 获取或设置完全序列化的，或者未完全反序列化的返回值。
        ''' </summary>
        Public Property ReturnValue As Object
            Get
                Dim tp = Type.GetType(ReturnValueTypeName)
                Return Deserialize(tp, NameOf(ReturnValue))
            End Get
            Set(value As Object)
                Serialize(NameOf(ReturnValue), value)
                ReturnValueTypeName = MetadataHelper.GetTypeName(value)
            End Set
        End Property

        ''' <summary>
        ''' 设置完全序列化的，或者未完全反序列化的返回值。
        ''' </summary>
        Public Property ReturnValueTypeName As String
            Get
                Return Deserialize(GetType(String), NameOf(ReturnValueTypeName))
            End Get
            Set(value As String)
                Serialize(NameOf(ReturnValueTypeName), value)
            End Set
        End Property

    End Class
End Namespace