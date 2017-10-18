Namespace Scripting.Models.Constants
    Public Class StringConstant
        Inherits ConstantNode

        Public Sub New(value As String, content As String)
            MyBase.New(value, content)
        End Sub

        Public Shared Function TryParseValue(value As String, ByRef result As String) As Boolean
            If String.IsNullOrEmpty(value) Then
                Return False
            End If
            If value.StartsWith("""") AndAlso value.EndsWith("""") Then
                result = value.Substring(1, value.Length - 2)
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace