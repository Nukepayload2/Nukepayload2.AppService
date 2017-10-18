Imports Nukepayload2.AppService.Core.Scripting.Models.Trivia

Namespace Scripting.Analyzers
    Public Class OutputItem
        Public Sub New(range As Range, description As String, errorCode As String)
            Me.Range = range
            Me.Description = description
            Me.ErrorCode = errorCode
        End Sub

        Public ReadOnly Property Range As Range
        Public ReadOnly Property Description As String
        Public ReadOnly Property ErrorCode As String
        ''' <summary>
        ''' 用字符串表示输出的项目
        ''' </summary>
        Public Overrides Function ToString() As String
            Dim rangeText = If(Range Is Nothing, "未知", Range.ToString)
            Dim descriptionText = If(Description, "未知")
            Dim errorCodeText = If(ErrorCode, "E_UNEXPTED")
            Return $"范围 {rangeText}, 描述 {descriptionText}, 错误码 {errorCodeText}。"
        End Function
    End Class
End Namespace