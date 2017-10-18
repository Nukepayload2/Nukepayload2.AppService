Namespace Serialization
    ''' <summary>
    ''' 不执行任何预先的序列化和后继的反序列化操作。
    ''' </summary>
    Public Class NullFormatter
        Implements IValueFormatter

        Public Function Serialize(data As Object) As Object Implements IValueFormatter.Serialize
            Return data
        End Function

        Public Function Deserialize(Of T)(data As Object) As T Implements IValueFormatter.Deserialize
            Return DirectCast(data, T)
        End Function

        Public Function Deserialize(data As Object, type As Type) As Object Implements IValueFormatter.Deserialize
            If type Is Nothing Then
                Throw New ArgumentNullException(NameOf(type))
            End If
            Return data
        End Function
    End Class
End Namespace