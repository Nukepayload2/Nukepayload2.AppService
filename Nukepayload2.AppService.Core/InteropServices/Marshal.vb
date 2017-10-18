Imports Nukepayload2.AppService.Core.Commands

Namespace InteropServices
    ''' <summary>
    ''' 定义框架内特殊数据的封送行为。
    ''' </summary>
    Public Class Marshal
        ''' <summary>
        ''' 将消息的格式转换为目标格式。
        ''' </summary>
        ''' <param name="msg">要转换的消息。</param>
        ''' <param name="targetFactory">代表目标格式的消息工厂。</param>
        Public Shared Function ConvertMessage(msg As IDictionary(Of String, Object), targetFactory As CommandFactory) As IDictionary(Of String, Object)
            Dim newMsg = targetFactory.CreateEmptyValueBag.Invoke
            For Each itm In msg
                newMsg.Add(itm.Key, itm.Value)
            Next
            Return newMsg
        End Function
    End Class

End Namespace