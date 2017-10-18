Imports Nukepayload2.AppService.Core.Scripting.Models.Identifiers
Imports Nukepayload2.AppService.Core.Scripting.Models.Operators
Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax
Imports Nukepayload2.AppService.Core.Scripting.Models.Trivia
Imports Nukepayload2.AppService.Core.Scripting.Utilities

Namespace Scripting.Analyzers
    Public Class DictionaryIndexSyntaxParser
        Public Function TryParseThrow(tree As PathExpressionSyntaxTree, rangeInTree As Range, ByRef value As DictionaryIndexSyntax) As Boolean
            Dim oriStr = tree.OriginalExpression
            Dim subStr = rangeInTree.SubString(oriStr)
            If PatternHelper.IsIdentifier(subStr) Then
                Return False
            ElseIf Aggregate ch In {""""c, "("c, "["c, ")"c, "]"c} Where subStr.Contains(ch) Into Any Then
                Return False
            ElseIf subStr.CountOf("!"c) = 1 Then
                Dim idNodes = Aggregate rng In rangeInTree.Split(oriStr, {"!"c}) Select New MemberNameIdentifier(tree, rng) Into ToArray
                If Aggregate tri In idNodes Where Not PatternHelper.IsIdentifier(tri.Text) Into Any Then
                    Return False
                End If
                Dim nameParams = subStr.Split("!"c)
                Dim syntax As New DictionaryIndexSyntax(nameParams(0), nameParams(1))
                Dim opFactory As New OperatorFactory(Of ExclamationOperator)
                Dim excRange As New Range(subStr.IndexOf("!"c) + rangeInTree.StartIndex, 1)
                With syntax.SubNodes
                    .Add(idNodes(0))
                    .Add(opFactory.CreateNode(tree, excRange))
                    .Add(idNodes(1))
                End With
                value = syntax
                Return True
            End If
            Return False
        End Function
    End Class
End Namespace