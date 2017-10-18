Namespace Attributes
    ''' <summary>
    ''' 指示成员在远程调用可见。
    ''' </summary>
    <AttributeUsage(AttributeTargets.Property Or AttributeTargets.Method)>
    Public Class RemoteCallVisibleAttribute
        Inherits Attribute
    End Class
End Namespace