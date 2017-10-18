Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax
Imports Nukepayload2.AppService.Core.Scripting.Models.Trivia
Imports Nukepayload2.AppService.Core.Scripting.Utilities

Namespace Scripting.Analyzers
    Public Class GetMemberSyntaxBuilder
        Public Function CreateNode(tree As PathExpressionSyntaxTree, range As Range) As SyntaxTreeNodeBuildResult
            Dim expr = range.SubString(tree.OriginalExpression)
            Dim curNode As GetMemberSyntax = Nothing
            Dim errors As New List(Of OutputItem)
            Try
                Dim simpleNodeParser As New DirectGetSyntaxParser
                Dim indexNodeParser As New DictionaryIndexSyntaxParser
                Dim callNodeParser As New CallSyntaxParser
                Dim simpleNode As DirectGetSyntax = Nothing, indexNode As DictionaryIndexSyntax = Nothing, callNode As CallSyntax = Nothing
                If simpleNodeParser.TryParseThrow(tree, range, simpleNode) Then
                    curNode = simpleNode
                ElseIf indexNodeParser.TryParseThrow(tree, range, indexNode) Then
                    curNode = indexNode
                ElseIf callNodeParser.TryParseThrow(tree, range, callNode) Then
                    curNode = callNode
                Else
                    Throw New ExpressionParseException($"无法确定表达式的类型: {expr}")
                End If
            Catch ex As ExpressionParseException
                errors.Add(New OutputItem(range, ex.Message, "E_PARSE_GET_MEMBER"))
            End Try
            Return New SyntaxTreeNodeBuildResult(curNode, errors, {}, {})
        End Function
    End Class
End Namespace