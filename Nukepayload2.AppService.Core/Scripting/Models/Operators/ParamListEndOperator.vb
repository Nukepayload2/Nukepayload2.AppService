Namespace Scripting.Models.Operators
    Public MustInherit Class ParamListEndOperator
        Inherits OperatorNode

        Shared s_OperatorType As OperatorTypes = OperatorTypes.ParameterListEnd

        Public Overrides ReadOnly Property OperatorType As OperatorTypes
            Get
                Return s_OperatorType
            End Get
        End Property
    End Class
End Namespace