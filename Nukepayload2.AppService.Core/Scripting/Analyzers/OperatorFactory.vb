Imports Nukepayload2.AppService.Core.Scripting.Models.Operators
Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax
Imports Nukepayload2.AppService.Core.Scripting.Models.Trivia
Imports Nukepayload2.AppService.Core.Scripting.Utilities

Namespace Scripting.Analyzers
    Public Class OperatorFactory(Of T As {OperatorNode, New})
        Public Overridable Function CreateNode(tree As PathExpressionSyntaxTree, range As Range) As T
            Dim trivia As New SymbolTrivia(range, range.SubString(tree.OriginalExpression))
            Dim op As New T
            op.Trivia.Add(trivia)
            Return op
        End Function
    End Class
End Namespace