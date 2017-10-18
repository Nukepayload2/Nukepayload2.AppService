Imports System.Reflection
Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax

Namespace Scripting.Evaluators
    Public Class ContinuousSyntaxEvaluator
        Public Function GetMemberInformation(context As Object, node As ContinuousGetMemberSyntax, ByRef lastContext As Object, ByRef args As Object(), isPropWrite As Boolean) As MemberInfo
            lastContext = context
            For Each subNode In node.SubNodes.Take(node.SubNodes.Count - 1)
                If TypeOf subNode Is GetMemberSyntax Then
                    Select Case subNode.GetType.Name
                        Case NameOf(DirectGetSyntax)
                            lastContext = New DirectGetEvaluator().GetNextContext(lastContext, DirectCast(subNode, DirectGetSyntax))
                        Case NameOf(DictionaryIndexSyntax)
                            lastContext = New DictionaryIndexEvaluator().GetNextContext(lastContext, DirectCast(subNode, DictionaryIndexSyntax))
                        Case NameOf(CallSyntax)
                            lastContext = New CallEvaluator().GetNextContext(lastContext, DirectCast(subNode, CallSyntax))
                    End Select
                End If
            Next
            Dim lastNode = TryCast(node.SubNodes.Last, GetMemberSyntax)
            If lastNode Is Nothing Then
                Throw New ExpressionExecutionException("最后一个节点不是 GetMemberSyntax")
            End If
            If isPropWrite Then
                args = lastNode.ParsedArguments
            End If
            Dim memberInfo As MemberInfo
            Select Case lastNode.GetType.Name
                Case NameOf(DirectGetSyntax)
                    memberInfo = New DirectGetEvaluator().GetMemberInformation(lastContext, DirectCast(lastNode, DirectGetSyntax), args)
                Case NameOf(DictionaryIndexSyntax)
                    Dim key As String = Nothing
                    memberInfo = New DictionaryIndexEvaluator().GetMemberInformation(lastContext, DirectCast(lastNode, DictionaryIndexSyntax), key)
                    args = {key}
                Case NameOf(CallSyntax)
                    memberInfo = New CallEvaluator().GetMemberInformation(lastContext, DirectCast(lastNode, CallSyntax), args)
                Case Else
                    Throw New ExpressionExecutionException("未知的 GetMemberSyntax 节点")
            End Select
            Return memberInfo
        End Function
    End Class
End Namespace