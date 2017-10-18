Namespace Scripting.Models.Syntax
    Public MustInherit Class GetMemberSyntax
        Inherits SyntaxNode

        Public Sub New(name As String, arguments() As String, parsedArguments() As Object)
            Me.Name = name
            Me.Arguments = arguments
            Me.ParsedArguments = parsedArguments
        End Sub

        Public ReadOnly Property Name As String

        Public ReadOnly Property Arguments As String()

        Public ReadOnly Property ParsedArguments As Object()
    End Class
End Namespace