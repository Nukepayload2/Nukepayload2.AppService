Imports System.Reflection
Imports Nukepayload2.AppService.Core.Attributes

Namespace Scripting.Evaluators
    Public Class RemoteVisibilityCheckModes
        ' 注意：不要移除 [ 和 ] 。因为未来版本的 VB 很可能要加入这两个关键字。
        Public Shared ReadOnly Property [Unchecked] As RemoteVisibilityCheckMode = Function() True
        Public Shared ReadOnly Property [Checked] As RemoteVisibilityCheckMode = Function(member) member.GetCustomAttribute(Of RemoteCallVisibleAttribute) IsNot Nothing

    End Class

    Public Delegate Function RemoteVisibilityCheckMode(member As MemberInfo) As Boolean
End Namespace