Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax

Namespace Scripting.Analyzers
    Public Class ExpressionParseException
        Inherits Exception
        Public Sub New(description As String)
            MyBase.New(description)
        End Sub
        Public Sub New(description As String, currentSyntax As Syntax, result As ISyntaxBuildResult)
            MyBase.New(description)
            Me.Result = result
            Me.CurrentSyntax = currentSyntax
        End Sub

        Public ReadOnly Property CurrentSyntax As Syntax
        Public ReadOnly Property Result As ISyntaxBuildResult

    End Class
End Namespace