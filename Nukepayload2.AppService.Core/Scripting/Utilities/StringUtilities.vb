Namespace Scripting.Utilities
    Friend Module StringUtilities
        <Extension>
        Function CountOf(str$, ch As Char) As Integer
            Return Aggregate c In str Where c = ch Into Count
        End Function
        <Extension>
        Function IsAscII(ch As Char) As Boolean
            Return AscW(ch) <= 255
        End Function
    End Module
End Namespace