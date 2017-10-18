Imports Nukepayload2.AppService.Core.Scripting.Models.Operators
Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax
Imports Nukepayload2.AppService.Core.Scripting.Models.Trivia

Namespace Scripting.Analyzers
    Public Class ContinuousGetMemberSyntaxBuilder
        Public Function PopulateTree(tree As PathExpressionSyntaxTree) As SyntaxTreeNodeBuildResult
            Dim expr = tree.OriginalExpression
            Dim rootSyntax = tree.ContinuousGetMemberSyntax
            Dim errors As New List(Of OutputItem), messages As New List(Of OutputItem), warnings As New List(Of OutputItem)
            Dim ranges As New List(Of Range)

            AddNodeRanges(expr, errors, ranges)

            If Not errors.Any Then
                NodeRangesToNodes(tree, ranges, errors, warnings, messages)
            End If

            Return New SyntaxTreeNodeBuildResult(rootSyntax, errors, {}, {})
        End Function

        Private Shared Sub NodeRangesToNodes(tree As PathExpressionSyntaxTree, ranges As List(Of Range),
            errors As List(Of OutputItem), warnings As List(Of OutputItem), messages As List(Of OutputItem))
            Dim isDot As Boolean
            Dim rootSyntax = tree.ContinuousGetMemberSyntax
            Dim getMemberBuilder As New GetMemberSyntaxBuilder
            Dim dotFactory As New OperatorFactory(Of DotOperator)
            For Each range In ranges
                If isDot Then
                    rootSyntax.SubNodes.Add(dotFactory.CreateNode(tree, range))
                Else
                    Dim createResult = getMemberBuilder.CreateNode(tree, range)
                    errors.AddRange(createResult.Errors)
                    messages.AddRange(createResult.Messages)
                    warnings.AddRange(createResult.Warnings)
                    rootSyntax.SubNodes.Add(createResult.Node)
                End If
                isDot = Not isDot
            Next
        End Sub

        Private Shared Sub AddNodeRanges(expr As String, errors As List(Of OutputItem), ranges As List(Of Range))
            Dim isInStrConst As Boolean
            Dim isInParamList As Boolean
            Dim start = 0, length = 0
            For Each ch In expr
                Select Case ch
                    Case "."c
                        If Not isInStrConst AndAlso Not isInParamList Then
                            If length <= 0 Then
                                errors.Add(New OutputItem(New Range(start, 1), "错误地使用 '.' 运算符。", "E_Dot_Misuse"))
                            Else
                                ranges.Add(New Range(start, length))
                                start += length
                                ranges.Add(New Range(start, 1))
                            End If
                            start += 1
                            length = 0
                        Else
                            length += 1
                        End If
                    Case """"c
                        length += 1
                        isInStrConst = Not isInStrConst
                    Case "("c, ")"c, "["c, "]"c
                        length += 1
                        If Not isInStrConst Then
                            isInParamList = Not isInParamList
                        End If
                    Case Else
                        length += 1
                End Select
            Next
            If length <= 0 Then
                errors.Add(New OutputItem(New Range(start - 1, 1), "错误地使用 '.' 运算符。", "E_Dot_Misuse"))
            End If
            ranges.Add(New Range(start, length))
        End Sub
    End Class
End Namespace