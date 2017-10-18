Imports System.Reflection
Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax

Namespace Scripting.Evaluators
    Public Class DictionaryIndexEvaluator
        Friend Function GetNextContext(currentContext As Object, subNode As DictionaryIndexSyntax) As Object
            ' dict!key 相当于 dict.Item("key")。currentContext 是 存放 dict 实例的对象, subNode.Name = "dict", subNode.ParsedArguments = {"key"}。
            ' 所以需要先找到 dict
            Dim dict = GetDict(currentContext, subNode)
            ' 再找并调用 Item 属性的 Get 。
            Return MetadataHelper.InvokeOrGetValue(dict, NameOf(Dictionary(Of String, Object).Item), subNode.ParsedArguments, RemoteVisibilityCheckModes.Unchecked)
        End Function

        Private Shared Function GetDict(currentContext As Object, subNode As DictionaryIndexSyntax) As Object
            Return MetadataHelper.InvokeOrGetValue(currentContext, subNode.Name, Nothing, RemoteVisibilityCheckModes.Checked)
        End Function

        Friend Function GetMemberInformation(ByRef currentContext As Object, lastNode As DictionaryIndexSyntax, ByRef key As String) As MemberInfo
            ' 字典值的读写
            currentContext = GetDict(currentContext, lastNode)
            key = CType(lastNode.ParsedArguments.First, String)
            Return MetadataHelper.FindMemberInfo(currentContext, NameOf(Dictionary(Of String, Object).Item),
                                                 lastNode.ParsedArguments,
                                                 RemoteVisibilityCheckModes.Unchecked)
        End Function
    End Class
End Namespace