Namespace Scripting.Models.Trivia
    Public Class SymbolTrivia
        Inherits SyntaxTrivia

        Public Sub New(range As Range, text As String)
            MyBase.New(range, text)
        End Sub
    End Class
End Namespace