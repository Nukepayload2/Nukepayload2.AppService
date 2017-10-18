Namespace Linq

    Public Module Extensions

        <Extension>
        Sub [Set](Of T)(ByRef this As T, newValue As T)
            this = newValue
        End Sub

    End Module

End Namespace