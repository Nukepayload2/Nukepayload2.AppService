Imports Nukepayload2.AppService.Core.Scripting.Models.Trivia

Namespace Scripting.Utilities
    Module RangeExtension
        <Extension>
        Function SubString(this As Range, str As String) As String
            If String.IsNullOrEmpty(str) Then
                Throw New ArgumentNullException(NameOf(str))
            End If
            Return str.Substring(this.StartIndex, this.Length)
        End Function
        ''' <summary>
        ''' 对范围内的字符串进行分割，返回分割出的范围。
        ''' </summary>
        <Extension>
        Iterator Function Split(this As Range, str As String, separators As Char(), Optional splitOption As StringSplitOptions = StringSplitOptions.None) As IEnumerable(Of Range)
            If String.IsNullOrEmpty(str) Then
                Throw New ArgumentNullException(NameOf(str))
            End If
            Dim subStr = this.SubString(str)
            Dim splitResult = subStr.Split(separators, splitOption)
            Dim startIndex = this.StartIndex
            For Each s In splitResult
                Yield New Range(startIndex, s.Length)
                startIndex += s.Length + 1
            Next
        End Function
    End Module

End Namespace