Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax

Namespace Scripting.Analyzers
    Public Class SyntaxTreeNodeBuildResult
        Implements ISyntaxBuildResult

        Public Sub New(node As SyntaxNode, errors As IReadOnlyList(Of OutputItem), messages As IReadOnlyList(Of OutputItem), warnings As IReadOnlyList(Of OutputItem))
            If node Is Nothing Then
                Throw New ArgumentNullException(NameOf(node))
            End If
            If errors Is Nothing Then
                Throw New ArgumentNullException(NameOf(errors))
            End If
            If warnings Is Nothing Then
                Throw New ArgumentNullException(NameOf(warnings))
            End If
            If messages Is Nothing Then
                Throw New ArgumentNullException(NameOf(messages))
            End If
            Me.Node = node
            Me.Errors = errors
            Me.Messages = messages
            Me.Warnings = warnings
        End Sub

        Public ReadOnly Property Node As SyntaxNode

        Public ReadOnly Property Errors As IReadOnlyList(Of OutputItem) Implements ISyntaxBuildResult.Errors

        Public ReadOnly Property Messages As IReadOnlyList(Of OutputItem) Implements ISyntaxBuildResult.Messages

        Public ReadOnly Property Warnings As IReadOnlyList(Of OutputItem) Implements ISyntaxBuildResult.Warnings

    End Class
End Namespace