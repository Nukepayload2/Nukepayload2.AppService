Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax
Imports Nukepayload2.AppService.Core.Scripting.Models.Trivia

Namespace Scripting.Analyzers
    Public Class SyntaxTreeParser
        Public Function Parse(expression As String) As SyntaxTreeParseResult
            Dim tree As PathExpressionSyntaxTree = Nothing
            Try
                If String.IsNullOrEmpty(expression) Then
                    Throw New ArgumentNullException(NameOf(expression))
                End If
                tree = New PathExpressionSyntaxTree(expression)
                Dim rootBuilder As New ContinuousGetMemberSyntaxBuilder
                Dim nodeResult = rootBuilder.PopulateTree(tree)
                Return New SyntaxTreeParseResult(tree, nodeResult.Errors, nodeResult.Warnings, nodeResult.Messages)
            Catch ex As ExpressionParseException
                Return New SyntaxTreeParseResult(tree, ex.Result.Errors, ex.Result.Warnings, ex.Result.Messages)
            Catch ex As Exception
                Return New SyntaxTreeParseResult(tree, {New OutputItem(New Range(0, 0), ex.Message, "CLR" & ex.HResult)}, {}, {})
            End Try
        End Function
    End Class
End Namespace