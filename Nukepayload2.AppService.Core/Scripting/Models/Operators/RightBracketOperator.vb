Namespace Scripting.Models.Operators
    Public Class RightBracketOperator
        Inherits ParamListEndOperator

        Shared s_operatorString As String = "]"

        Public Overrides ReadOnly Property OperatorText As String
            Get
                Return s_operatorString
            End Get
        End Property
    End Class
End Namespace