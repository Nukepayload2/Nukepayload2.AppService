Namespace Scripting.Models.Operators
    Public MustInherit Class ParamListBeginOperator
        Inherits OperatorNode

        Shared s_OperatorType As OperatorTypes = OperatorTypes.ParameterListBegin

        Public Overrides ReadOnly Property OperatorType As OperatorTypes
            Get
                Return s_OperatorType
            End Get
        End Property
    End Class
End Namespace