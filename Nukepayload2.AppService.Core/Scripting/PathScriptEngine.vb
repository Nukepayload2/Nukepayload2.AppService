Imports System.Reflection
Imports Nukepayload2.AppService.Core.Scripting.Analyzers
Imports Nukepayload2.AppService.Core.Scripting.Evaluators
Imports Nukepayload2.AppService.Core.Scripting.Models.Syntax

Namespace Scripting
    ''' <summary>
    ''' 执行路径查询的脚本引擎。
    ''' </summary>
    Public Class PathScriptEngine
        ''' <summary>
        ''' 执行脚本。
        ''' </summary>
        ''' <param name="pathExpr">路径表达式。</param>
        ''' <param name="context">当前上下文。为路径表达式提供元数据。</param>
        ''' <param name="args">参数。用于执行路径表达式的最后一个节点。可以为空。</param>
        ''' <param name="isPropertyWrite">表示这是不是一个属性写入操作。</param>
        ''' <returns>执行结果。对于 <see cref="MethodInfo.ReturnType"/> 为 <see cref="Void"/> 的方法或属性写入永远会返回空。</returns>
        Public Function Execute(pathExpr As String, context As Object, Optional args As Object() = Nothing, Optional isPropertyWrite As Boolean = False) As Object
            Dim syntaxTreeParser As New SyntaxTreeParser
            Dim result = syntaxTreeParser.Parse(pathExpr)
            ' TODO: 加上代码分析功能后统计 Warning 和 Message
            If (result.Errors?.Any).GetValueOrDefault Then
                Dim errs = String.Join(vbCrLf, From errText In result.Errors Select s = errText.ToString)
                Throw New ExpressionExecutionException(errs)
            Else
                Dim tree = result.SyntaxTree
                Dim rootNode = tree.ContinuousGetMemberSyntax
                Dim rootNodeEvaluator As New ContinuousSyntaxEvaluator
                Dim lastContext As Object = Nothing
                If isPropertyWrite Then
                    If args.Length <> 1 Then
                        Throw New ExpressionExecutionException("写入属性必须只有一个参数作为属性的值。")
                    End If
                    Dim value = args.First
                    Dim memberInfo = rootNodeEvaluator.GetMemberInformation(context, rootNode, lastContext, args, True)
                    Dim propInfo = TryCast(memberInfo, PropertyInfo)
                    propInfo.SetValue(lastContext, value, DirectCast(rootNode.SubNodes.Last, GetMemberSyntax).ParsedArguments)
                    Return Nothing
                Else
                    Dim memberInfo = rootNodeEvaluator.GetMemberInformation(context, rootNode, lastContext, args, False)
                    Return MetadataHelper.UncheckedInvokeOrGetValue(lastContext, memberInfo, args)
                End If
            End If
        End Function
    End Class
End Namespace