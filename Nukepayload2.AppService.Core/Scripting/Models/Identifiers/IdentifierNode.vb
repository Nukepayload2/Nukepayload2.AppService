Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax

Namespace Scripting.Models.Identifiers
    Public MustInherit Class IdentifierNode
        Inherits SyntaxNode

        Public Sub New(text As String)
            Me.Text = text
        End Sub

        Public ReadOnly Property Text As String
    End Class
End Namespace