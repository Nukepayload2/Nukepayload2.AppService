Namespace Scripting.Models.Constants
    Public Class Int32Constant
        Inherits ConstantNode

        Public Sub New(value As String, parsedValue As Integer)
            MyBase.New(value, parsedValue)
        End Sub
    End Class
End Namespace