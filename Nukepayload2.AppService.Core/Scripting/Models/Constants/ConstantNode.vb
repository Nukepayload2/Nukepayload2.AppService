Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax

Namespace Scripting.Models.Constants
    Public MustInherit Class ConstantNode
        Inherits SyntaxNode

        Sub New(value As String, parsedValue As Object)
            Me.Value = value
            Me.ParsedValue = parsedValue
        End Sub

        Public ReadOnly Property Value As String

        Public ReadOnly Property ParsedValue As Object

    End Class
End Namespace