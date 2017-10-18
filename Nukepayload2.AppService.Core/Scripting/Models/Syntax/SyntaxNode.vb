Imports Nukepayload2.AppService.Core.Scripting.Models.Trivia

Namespace Scripting.Models.Syntax
    Public MustInherit Class SyntaxNode
        Inherits Syntax
        Public ReadOnly Property Trivia As New List(Of SyntaxTrivia)
        Public ReadOnly Property SubNodes As New List(Of SyntaxNode)
    End Class
End Namespace