Namespace Scripting.Models.Trivia
    Public Class SpaceTrivia
        Inherits SyntaxTrivia

        Public Sub New(range As Range, text As String)
            MyBase.New(range, text)
        End Sub
    End Class
End Namespace