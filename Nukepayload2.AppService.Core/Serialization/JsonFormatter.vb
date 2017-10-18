Imports Newtonsoft.Json

Namespace Serialization
    ''' <summary>
    ''' 在数据序列化之前预先序列化为 Json 字符串。反序列化时还原。
    ''' </summary>
    Public Class JsonFormatter
        Implements IValueFormatter

        Public Function Serialize(data As Object) As Object Implements IValueFormatter.Serialize
            Return JsonConvert.SerializeObject(data)
        End Function

        Public Function Deserialize(Of T)(data As Object) As T Implements IValueFormatter.Deserialize
            Return JsonConvert.DeserializeObject(Of T)(data)
        End Function

        Public Function Deserialize(data As Object, type As Type) As Object Implements IValueFormatter.Deserialize
            Return JsonConvert.DeserializeObject(data, type)
        End Function
    End Class
End Namespace