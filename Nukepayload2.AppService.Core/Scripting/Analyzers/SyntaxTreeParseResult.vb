Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax

Namespace Scripting.Analyzers
    Public Class SyntaxTreeParseResult
        Implements ISyntaxBuildResult
        Public Sub New(syntaxTree As PathExpressionSyntaxTree, errors As IReadOnlyList(Of OutputItem),
                       warnings As IReadOnlyList(Of OutputItem), messages As IReadOnlyList(Of OutputItem))
            Me.SyntaxTree = syntaxTree
            Me.Errors = errors
            Me.Warnings = warnings
            Me.Messages = messages
        End Sub

        Public ReadOnly Property SyntaxTree As PathExpressionSyntaxTree
        Public ReadOnly Property Errors As IReadOnlyList(Of OutputItem) Implements ISyntaxBuildResult.Errors
        Public ReadOnly Property Warnings As IReadOnlyList(Of OutputItem) Implements ISyntaxBuildResult.Warnings
        Public ReadOnly Property Messages As IReadOnlyList(Of OutputItem) Implements ISyntaxBuildResult.Messages
    End Class
End Namespace