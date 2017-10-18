Imports System.Security.Cryptography

Namespace Serialization
    ''' <summary>
    ''' 数据完成序列化前进行 AES 加密，完成序列化后进行 AES 解密。
    ''' </summary>
    Public Class AesFormatter
        Implements IValueFormatter

        Private _underlyingFormatter As SecuredFormatter
        ''' <summary>
        ''' 创建 AES 加密和解密格式化。
        ''' </summary>
        ''' <param name="key">密钥。</param>
        ''' <param name="iv">初始向量。</param>
        Sub New(key As Byte(), iv As Byte())
            With Aes.Create
                .IV = iv
                .Key = key
                .Mode = CipherMode.CBC
                _underlyingFormatter = New SecuredFormatter(.CreateDecryptor, .CreateEncryptor)
            End With
        End Sub

        Public Function Serialize(data As Object) As Object Implements IValueFormatter.Serialize
            Return _underlyingFormatter.Serialize(data)
        End Function

        Public Function Deserialize(Of T)(data As Object) As T Implements IValueFormatter.Deserialize
            Return _underlyingFormatter.Deserialize(Of T)(data)
        End Function

        Public Function Deserialize(data As Object, type As Type) As Object Implements IValueFormatter.Deserialize
            Return _underlyingFormatter.Deserialize(data, type)
        End Function
    End Class
End Namespace