Imports System.Security.Cryptography
Imports System.Text
Imports Newtonsoft.Json

Namespace Serialization
    ''' <summary>
    ''' 使用自定义的方式加密要序列化的数据。
    ''' </summary>
    Public Class SecuredFormatter
        Implements IValueFormatter

        Public Sub New(decrypt As ICryptoTransform, encrypt As ICryptoTransform)
            If decrypt Is Nothing Then
                Throw New ArgumentNullException(NameOf(decrypt))
            End If
            If encrypt Is Nothing Then
                Throw New ArgumentNullException(NameOf(encrypt))
            End If
            Me.Decrypt = decrypt
            Me.Encrypt = encrypt
        End Sub

        Public ReadOnly Property Decrypt As ICryptoTransform
        Public ReadOnly Property Encrypt As ICryptoTransform

        Public Function Serialize(data As Object) As Object Implements IValueFormatter.Serialize
            Dim json = JsonConvert.SerializeObject(data)
            Dim bytes = Encoding.Unicode.GetBytes(json)
            Dim output As New MemoryStream(Math.Ceiling(bytes.Length / Encrypt.OutputBlockSize) * Encrypt.OutputBlockSize)
            Using strm As New CryptoStream(output, Encrypt, CryptoStreamMode.Write)
                strm.Write(bytes, 0, bytes.Length)
            End Using
            Return output.ToArray
        End Function

        Public Function Deserialize(Of T)(data As Object) As T Implements IValueFormatter.Deserialize
            Dim json As String = DeserializeToString(data)
            Return JsonConvert.DeserializeObject(Of T)(json)
        End Function

        Private Function DeserializeToString(data As Object) As String
            Dim bytes = DirectCast(data, Byte())
            Dim input As New MemoryStream(bytes)
            Dim output As New MemoryStream(bytes.Length)
            Using strm As New CryptoStream(input, Decrypt, CryptoStreamMode.Read)
                strm.CopyTo(output)
                output.SetLength(output.Position + 1)
            End Using
            Return Encoding.Unicode.GetString(output.ToArray)
        End Function

        Public Function Deserialize(data As Object, type As Type) As Object Implements IValueFormatter.Deserialize
            Dim json As String = DeserializeToString(data)
            Return JsonConvert.DeserializeObject(json, type)
        End Function
    End Class
End Namespace