Namespace Scripting.Models.Operators
    Public Class DotOperator
        Inherits OperatorNode

        Shared s_OperatorType As OperatorTypes = OperatorTypes.PathSplitter

        Public Overrides ReadOnly Property OperatorType As OperatorTypes
            Get
                Return s_OperatorType
            End Get
        End Property

        Shared s_operatorString As String = "."
        Public Overrides ReadOnly Property OperatorText As String
            Get
                Return s_operatorString
            End Get
        End Property
    End Class
End Namespace