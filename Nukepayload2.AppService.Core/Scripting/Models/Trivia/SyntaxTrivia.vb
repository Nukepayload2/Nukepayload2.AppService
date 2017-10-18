Namespace Scripting.Models.Trivia
    Public MustInherit Class SyntaxTrivia
        Public Sub New(range As Range, text As String)
            Me.Range = range
            Me.Text = text
        End Sub

        Public ReadOnly Property Range As Range
        Public ReadOnly Property Text As String
    End Class
End Namespace