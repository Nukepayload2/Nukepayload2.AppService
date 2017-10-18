Namespace Scripting.Models.Syntax
    Public Class DirectGetSyntax
        Inherits GetMemberSyntax

        Sub New(name As String)
            MyBase.New(name, Nothing, Nothing)
        End Sub

    End Class
End Namespace