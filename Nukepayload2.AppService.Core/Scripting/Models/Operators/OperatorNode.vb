Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax

Namespace Scripting.Models.Operators
    Public MustInherit Class OperatorNode
        Inherits SyntaxNode

        Public MustOverride ReadOnly Property OperatorType As OperatorTypes

        Public MustOverride ReadOnly Property OperatorText As String
    End Class
End Namespace