Namespace InteropServices

    Public Class MetadataHelper
        Public Shared Function GetTypeName(value As Object) As String
            If value Is Nothing Then
                Return "System.Object"
            Else
                Dim tp = value.GetType
                Dim fullName = tp.FullName
                If fullName.StartsWith("System.") Then
                    Return fullName
                Else
                    Return tp.AssemblyQualifiedName
                End If
            End If
        End Function
    End Class

End Namespace