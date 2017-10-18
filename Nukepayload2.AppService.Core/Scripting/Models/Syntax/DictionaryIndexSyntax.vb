Namespace Scripting.Models.Syntax
    Public Class DictionaryIndexSyntax
        Inherits GetMemberSyntax

        Sub New(name As String, index As String)
            MyBase.New(name, {index}, {index})
        End Sub

    End Class
End Namespace