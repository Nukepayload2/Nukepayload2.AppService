Namespace Scripting.Models.Trivia
    Public Class Range
        Sub New(startIndex As Integer, length As Integer)
            If startIndex < 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(startIndex))
            End If
            If length < 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(length))
            End If
            Me.StartIndex = startIndex
            Me.Length = length
        End Sub

        Public ReadOnly Property StartIndex As Integer
        Public ReadOnly Property Length As Integer
        Public ReadOnly Property EndIndex As Integer
            Get
                Return StartIndex + Length - 1
            End Get
        End Property
        Public ReadOnly Property NextIndex As Integer
            Get
                Return StartIndex + Length
            End Get
        End Property

        Public Shared Operator +(v1 As Range, v2 As Integer) As Range
            Return New Range(v1.StartIndex + v2, v1.Length)
        End Operator

        Public Shared Operator -(v1 As Range, v2 As Integer) As Range
            Return New Range(v1.StartIndex - v2, v1.Length)
        End Operator
        ''' <summary>
        ''' 用字符串表示范围
        ''' </summary>
        Public Overrides Function ToString() As String
            Return $"起始位置: {StartIndex}, 长度: {Length}"
        End Function
    End Class
End Namespace