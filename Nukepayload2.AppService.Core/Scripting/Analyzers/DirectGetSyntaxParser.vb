Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax
Imports Nukepayload2.AppService.Core.Scripting.Models.Trivia
Imports Nukepayload2.AppService.Core.Scripting.Utilities

Namespace Scripting.Analyzers
    Public Class DirectGetSyntaxParser
        ''' <summary>
        ''' 如果是当前类型的节点则进行转换并返回 True。如果不是这种节点则返回 False。对于确定了类型但是有明显错误的情况会抛出异常。
        ''' </summary>
        '''<exception cref="ExpressionParseException">确定表达式是当前类型，但是其中存在错误。</exception>
        Public Function TryParseThrow(tree As PathExpressionSyntaxTree, rangeInTree As Range, ByRef value As DirectGetSyntax) As Boolean
            Dim s = rangeInTree.SubString(tree.OriginalExpression)
            If PatternHelper.IsIdentifier(s) Then
                Dim trivia As New TextTrivia(rangeInTree, s)
                Dim syntax As New DirectGetSyntax(s)
                syntax.Trivia.Add(trivia)
                value = syntax
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace