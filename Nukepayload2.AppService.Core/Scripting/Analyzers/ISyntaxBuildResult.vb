Namespace Scripting.Analyzers
    Public Interface ISyntaxBuildResult
        ReadOnly Property Errors As IReadOnlyList(Of OutputItem)
        ReadOnly Property Messages As IReadOnlyList(Of OutputItem)
        ReadOnly Property Warnings As IReadOnlyList(Of OutputItem)
    End Interface
End Namespace