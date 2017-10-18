Imports System.Reflection
Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax

Namespace Scripting.Evaluators
    Public Class CallEvaluator
        Friend Function GetNextContext(currentContext As Object, subNode As CallSyntax) As Object
            Return MetadataHelper.InvokeOrGetValue(currentContext, subNode.Name, subNode.ParsedArguments, RemoteVisibilityCheckModes.Checked)
        End Function

        Friend Function GetMemberInformation(currentContext As Object, lastNode As CallSyntax, ByRef params As Object()) As MemberInfo
            ' 只有带索引参数的 Property Write 会调用这个
            ' 索引参数在节点中定义，需要写入的值在 params 定义。
            params = lastNode.ParsedArguments
            Return MetadataHelper.FindMemberInfo(currentContext, lastNode.Name, lastNode.ParsedArguments, RemoteVisibilityCheckModes.Checked)
        End Function
    End Class
End Namespace