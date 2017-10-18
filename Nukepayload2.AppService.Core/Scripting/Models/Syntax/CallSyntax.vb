Namespace Scripting.Models.Syntax
    Public Class CallSyntax
        Inherits GetMemberSyntax

        Public Sub New(name As String, arguments() As String, parsedArguments As Object())
            MyBase.New(name, arguments, parsedArguments)
        End Sub

    End Class
End Namespace