Namespace Scripting.Models.Operators
    Public Class LeftBracketOperator
        Inherits ParamListBeginOperator

        Shared s_operatorString As String = "["

        Public Overrides ReadOnly Property OperatorText As String
            Get
                Return s_operatorString
            End Get
        End Property
    End Class
End Namespace