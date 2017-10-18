Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports Nukepayload2.AppService.Core.Scripting.Utilities

Namespace Scripting.Analyzers

    Public Class PatternHelper
        Shared s_regIsIdentifier As New Regex("\w+", RegexOptions.Compiled)
        ''' <summary>
        ''' 是不是标识符
        ''' </summary>
        Public Shared Function IsIdentifier(s As String) As Boolean
            If String.IsNullOrEmpty(s) Then
                Return False
            End If
            Return s_regIsIdentifier.Matches(s).Count = 1 AndAlso s.CountOf("_"c) < s.Length AndAlso Not Char.IsDigit(s(0))
        End Function

        Shared s_regReplaceCommaSpace As New Regex("(, +| +,)", RegexOptions.Compiled)
        ''' <summary>
        ''' 是不是调用语法
        ''' </summary>
        Public Shared Function IsCall(s As String, <Out> ByRef identifierPart As String, <Out> ByRef hasParentheses As Boolean) As Boolean
            If String.IsNullOrEmpty(s) Then
                Return False
            End If
            ' 必须以标识符开始, 括号不能混用
            If s.StartsWith("(") OrElse s.StartsWith("[") Then
                Return False
            End If
            Dim hasLeftPar As Boolean = s.Contains("(")
            Dim hasLeftBracket As Boolean = s.Contains("[")
            If hasLeftPar <> hasLeftBracket Then
                If hasLeftPar Then
                    identifierPart = s.Substring(0, s.IndexOf("("))
                    If Not IsIdentifier(identifierPart) Then
                        identifierPart = Nothing
                        Return False
                    End If
                Else
                    identifierPart = s.Substring(0, s.IndexOf("["))
                    If Not IsIdentifier(identifierPart) Then
                        identifierPart = Nothing
                        Return False
                    End If
                End If
                hasParentheses = hasLeftPar
            Else
                Return False
            End If
            ' 有字符串常量，则将其移除。
            Dim hasStrConst = s.Contains(""""c)
            If hasStrConst Then
                If Not s.CountOf(""""c) Mod 2 = 0 Then
                    Return False
                End If
                Dim isStrConst = False
                Dim splited = s.Split(""""c)
                Dim sb As New StringBuilder
                For Each str In splited
                    If isStrConst Then
                        sb.Append(0)
                    Else
                        sb.Append(str)
                    End If
                    isStrConst = Not isStrConst
                Next
                s = sb.ToString
            End If
            ' 逗号旁边可以有空格。替换掉它。
            s = s_regReplaceCommaSpace.Replace(s, ",")
            ' 逗号不能相邻, 也不能在最后
            If s.Contains(",,") OrElse s.EndsWith(",") Then
                Return False
            End If
            ' 括号必须是唯一成对的，位置不能相反。
            For Each str In {"()", "[]"}
                If s.CountOf(str(0)) = 1 AndAlso s.CountOf(str(1)) = 1 Then
                    If s.IndexOf(str(0)) < s.IndexOf(str(1)) Then
                        Return True
                    End If
                End If
            Next
            ' 括号不符合规则
            Return False
        End Function

    End Class
End Namespace