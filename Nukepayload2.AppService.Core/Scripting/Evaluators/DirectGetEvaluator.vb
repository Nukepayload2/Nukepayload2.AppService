Imports System.Reflection
Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax

Namespace Scripting.Evaluators
    Public Class DirectGetEvaluator
        Public Function GetNextContext(currentContext As Object, subNode As DirectGetSyntax) As Object
            Return MetadataHelper.InvokeOrGetValue(currentContext, subNode.Name, Nothing, RemoteVisibilityCheckModes.Checked)
        End Function

        Friend Function GetMemberInformation(lastContext As Object, lastNode As DirectGetSyntax, ByRef args() As Object) As MemberInfo
            ' 无参数属性写入，无参数属性读取，无参数方法调用
            Return MetadataHelper.FindMemberInfo(lastContext, lastNode.Name, args, RemoteVisibilityCheckModes.Checked)
        End Function

    End Class
End Namespace