Imports Nukepayload2.AppService.Core.Scripting.Models.Constants
Imports Nukepayload2.AppService.Core.Scripting.Models.Identifiers
Imports Nukepayload2.AppService.Core.Scripting.Models.Operators
Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax
Imports Nukepayload2.AppService.Core.Scripting.Models.Trivia
Imports Nukepayload2.AppService.Core.Scripting.Utilities

Namespace Scripting.Analyzers
    Public Class CallSyntaxParser

        Public Function TryParseThrow(tree As PathExpressionSyntaxTree, rangeInTree As Range, ByRef value As CallSyntax) As Boolean
            Dim oriStr = tree.OriginalExpression
            Dim subStr = rangeInTree.SubString(oriStr)
            Dim identifierPart As String = Nothing
            Dim hasParentheses As Boolean
            If PatternHelper.IsCall(subStr, identifierPart, hasParentheses) Then
                ' 左侧的标识符
                Dim idNodeRange As New Range(rangeInTree.StartIndex, identifierPart.Length)
                Dim idNode As New MemberNameIdentifier(tree, idNodeRange)
                ' 左边的括号
                Dim leftParNodeRange As New Range(idNodeRange.NextIndex, 1)
                Dim leftParNode As ParamListBeginOperator
                If hasParentheses Then
                    Dim opFactory As New OperatorFactory(Of LeftParenthesesOperator)
                    leftParNode = opFactory.CreateNode(tree, leftParNodeRange)
                Else
                    Dim opFactory As New OperatorFactory(Of LeftBracketOperator)
                    leftParNode = opFactory.CreateNode(tree, leftParNodeRange)
                End If
                ' 参数列表
                Dim paramListRange As New Range(leftParNodeRange.NextIndex, subStr.Length - identifierPart.Length - 2)
                Dim paramPart = paramListRange.SubString(oriStr)
                Dim paramsParseResult = ParseParamList(tree, paramPart, paramListRange.StartIndex)
                ' 右边的括号
                Dim rightParNodeRange As New Range(paramListRange.NextIndex, 1)
                Dim rightParNode As ParamListEndOperator
                If hasParentheses Then
                    Dim opFactory As New OperatorFactory(Of RightParenthesesOperator)
                    rightParNode = opFactory.CreateNode(tree, rightParNodeRange)
                Else
                    Dim opFactory As New OperatorFactory(Of RightBracketOperator)
                    rightParNode = opFactory.CreateNode(tree, rightParNodeRange)
                End If
                ' 构建 CallSyntax 并传递
                Dim name = idNode.Text
                Dim rawArgs = Aggregate arg In paramsParseResult.InnerConstants Select v = arg.Value Into ToArray
                Dim parsedArgs = Aggregate arg In paramsParseResult.InnerConstants Select arg.ParsedValue Into ToArray
                Dim callNode As New CallSyntax(name, rawArgs, parsedArgs)
                With callNode
                    With .SubNodes
                        .Add(idNode)
                        .Add(leftParNode)
                        .AddRange(paramsParseResult.InnerConstants)
                        .AddRange(paramsParseResult.InnerOperators)
                        .Add(rightParNode)
                    End With
                    .Trivia.AddRange(paramsParseResult.InnerTrivia)
                End With
                value = callNode
                Return True
            Else
                Return False
            End If
        End Function

        ''' <exception cref="ExpressionParseException"></exception>
        Public Shared Function ParseParamList(tree As PathExpressionSyntaxTree,
                                          paramPart As String,
                                          startIndex As Integer) As (
                                          InnerConstants As IReadOnlyList(Of ConstantNode),
                                          InnerOperators As IReadOnlyList(Of OperatorNode),
                                          InnerTrivia As IReadOnlyList(Of SyntaxTrivia))
            ' 123, "4,5,6", 890.12
            Dim isInStr = False
            Dim length = 0
            Dim innerConstants As New List(Of ConstantNode)
            Dim innerOperators As New List(Of OperatorNode)
            Dim innerTrivia As New List(Of SyntaxTrivia)
            Dim commaFactory As New OperatorFactory(Of CommaOperator)
            Dim constFactory As New ConstantNodeFactory
            Dim afterCommaSpace = 0
            For Each ch In paramPart
                afterCommaSpace += 1
                If isInStr Then
                    length += 1
                    ' 字符串结束
                    If ch = """"c Then
                        Dim strConstRange As New Range(startIndex, length)
                        innerConstants.Add(constFactory.CreateString(tree, strConstRange))
                        startIndex = strConstRange.NextIndex
                        length = 0
                        isInStr = Not isInStr
                    End If
                Else
                    Select Case ch
                        Case ","c
                            If afterCommaSpace = 1 Then
                                Throw New ExpressionParseException($"检测到当前表达式是 CallSyntax, 但是有多余的逗号位于参数列表 ""{paramPart}"" 中。")
                            End If
                            If length > 0 Then
                                Dim someConstRange As New Range(startIndex, length)
                                innerConstants.Add(constFactory.CreateNonString(tree, someConstRange))
                                startIndex = someConstRange.NextIndex
                            End If
                            length = 1
                            Dim commaRange As New Range(startIndex, length)
                            Dim comma = commaFactory.CreateNode(tree, commaRange)
                            innerOperators.Add(comma)
                            length = 0
                            afterCommaSpace = 0
                        Case " "c
                            If afterCommaSpace = 1 Then
                                afterCommaSpace = 0
                                length += 1
                            Else
                                Throw New ExpressionParseException($"检测到当前表达式是 CallSyntax, 但是有多余的空格位于参数列表 ""{paramPart}"" 中。")
                            End If
                        Case """"c
                            ' 字符串开始
                            isInStr = Not isInStr
                            ' 处理逗号后面的 SpaceTrivia
                            If afterCommaSpace = 1 AndAlso length > 0 Then
                                InsertSpaceTrivia(tree.OriginalExpression, startIndex, length, innerTrivia)
                            End If
                            length = 1
                        Case Else
                            If Not Char.IsLetterOrDigit(ch) AndAlso ch <> "."c Then
                                Throw New ExpressionParseException($"检测到当前表达式是 CallSyntax, 但是其中存在非法字符 U+{AscW(ch).ToString("X")} 位于参数列表 ""{paramPart}"" 中。")
                            End If
                            ' 处理逗号后面的 SpaceTrivia
                            If afterCommaSpace = 1 AndAlso length > 0 Then
                                InsertSpaceTrivia(tree.OriginalExpression, startIndex, length, innerTrivia)
                            Else
                                length += 1
                            End If
                    End Select
                End If
            Next
            ' 最后一个不是字符串常量
            If length > 0 Then
                innerConstants.Add(constFactory.CreateNonString(tree, New Range(startIndex, length)))
            End If
            Return (innerConstants, innerOperators, innerTrivia)
        End Function

        Private Shared Sub InsertSpaceTrivia(oriString As String, ByRef startIndex As Integer, ByRef length As Integer, innerTrivia As List(Of SyntaxTrivia))
            startIndex += 1
            Dim spaceRange As New Range(startIndex, length)
            Dim space As New SpaceTrivia(spaceRange, spaceRange.SubString(oriString))
            innerTrivia.Add(space)
            startIndex = spaceRange.NextIndex
            length = 1
        End Sub
    End Class
End Namespace