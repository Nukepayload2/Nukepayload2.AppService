Namespace Scripting.Models.Syntax
    Public Class PathExpressionSyntaxTree
        Inherits Syntax
        Public Sub New(originalExpression As String)
            Me.OriginalExpression = originalExpression
        End Sub

        Public ReadOnly Property ContinuousGetMemberSyntax As New ContinuousGetMemberSyntax

        Public ReadOnly Property OriginalExpression As String
    End Class
End Namespace