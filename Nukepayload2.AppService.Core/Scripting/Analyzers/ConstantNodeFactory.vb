Imports Nukepayload2.AppService.Core.Scripting.Models.Constants
Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax
Imports Nukepayload2.AppService.Core.Scripting.Models.Trivia
Imports Nukepayload2.AppService.Core.Scripting.Utilities

Namespace Scripting.Analyzers
    Public Class ConstantNodeFactory
        Public Function CreateString(tree As PathExpressionSyntaxTree, range As Range) As StringConstant
            If tree Is Nothing Then
                Throw New ArgumentNullException(NameOf(tree))
            End If
            If range Is Nothing Then
                Throw New ArgumentNullException(NameOf(range))
            End If

            Dim val$ = String.Empty
            Dim subStr = range.SubString(tree.OriginalExpression)
            If StringConstant.TryParseValue(subStr, val) Then
                Dim strNode As New StringConstant(subStr, val)
                InitializeTrivia(strNode, tree, range)
                Return strNode
            Else
                Throw New ArgumentException("范围内不是字符串常量。", NameOf(range))
            End If
        End Function

        Public Function CreateNonString(tree As PathExpressionSyntaxTree, range As Range) As ConstantNode
            If tree Is Nothing Then
                Throw New ArgumentNullException(NameOf(tree))
            End If
            If range Is Nothing Then
                Throw New ArgumentNullException(NameOf(range))
            End If

            Dim content = range.SubString(tree.OriginalExpression)
            Dim boolValue = False, int32Value = 0, doubleValue = 0.0
            Dim node As ConstantNode
            If Boolean.TryParse(content, boolValue) Then
                node = New BooleanConstant(content, boolValue)
            ElseIf Integer.TryParse(content, int32Value) Then
                node = New Int32Constant(content, int32Value)
            ElseIf Double.TryParse(content, doubleValue) Then
                node = New DoubleConstant(content, doubleValue)
            Else
                Throw New ArgumentException("范围内不是 布尔值, 32 位整数 或 64 位浮点数 常量。", NameOf(range))
            End If
            InitializeTrivia(node, tree, range)
            Return node
        End Function

        Private Sub InitializeTrivia(node As StringConstant, tree As PathExpressionSyntaxTree, nodeRange As Range)
            Dim symRange As New Range(nodeRange.StartIndex, 1)
            Dim leftDoubleQuotation As New SymbolTrivia(symRange, symRange.SubString(tree.OriginalExpression))
            Dim contentRange As New Range(symRange.NextIndex, nodeRange.Length - 2)
            Dim strContent As New TextTrivia(contentRange, contentRange.SubString(tree.OriginalExpression))
            Dim symRightRange As New Range(contentRange.NextIndex, 1)
            Dim rightDoubleQuotation As New SymbolTrivia(symRightRange, symRightRange.SubString(tree.OriginalExpression))
            node.Trivia.AddRange({leftDoubleQuotation, strContent, rightDoubleQuotation})
        End Sub

        Private Sub InitializeTrivia(node As ConstantNode, tree As PathExpressionSyntaxTree, nodeRange As Range)
            ' TBD: 小数点是否应该分开计算细节？
            ' 当前行为: 小数是一个整体。
            Dim txtTrivia As New TextTrivia(nodeRange, nodeRange.SubString(tree.OriginalExpression))
            node.Trivia.Add(txtTrivia)
        End Sub
    End Class
End Namespace