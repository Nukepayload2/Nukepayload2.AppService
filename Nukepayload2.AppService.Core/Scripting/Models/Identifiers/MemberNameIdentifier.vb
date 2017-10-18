Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax
Imports Nukepayload2.AppService.Core.Scripting.Models.Trivia
Imports Nukepayload2.AppService.Core.Scripting.Utilities

Namespace Scripting.Models.Identifiers
    Public Class MemberNameIdentifier
        Inherits IdentifierNode

        Public Sub New(tree As PathExpressionSyntaxTree, range As Range)
            MyBase.New(range.SubString(tree.OriginalExpression))
            Dim textTrivia As New TextTrivia(range, tree.OriginalExpression)
            Trivia.Add(textTrivia)
        End Sub
    End Class
End Namespace