''' <summary>
''' 用于管理 SignalTarget 访问的 SignalContext 对象。
''' </summary>
Public Class SignalContextManager
    ''' <summary>
    ''' 默认的未命名的 SignalContext 。
    ''' </summary>
    Public Shared ReadOnly Property [Default] As Object

    Private Shared s_SignalContexts As New Dictionary(Of String, Object)
    ''' <summary>
    ''' 已命名的 SignalContext。
    ''' </summary>
    Public Shared ReadOnly Property SignalContexts As IReadOnlyDictionary(Of String, Object)
        Get
            Return s_SignalContexts
        End Get
    End Property

    ''' <summary>
    ''' 注册默认的 SignalContext。一旦注册就不能更改。
    ''' </summary>
    ''' <param name="obj">要注册的 SignalTarget。不能为空。</param>
    ''' <exception cref="ArgumentNullException"><paramref name="obj"/> 是空的。</exception>
    ''' <exception cref="InvalidOperationException">检测到 SignalTarget 重复注册。</exception>
    Public Shared Sub Register(obj As Object)
        If obj Is Nothing Then
            Throw New ArgumentNullException(NameOf(obj))
        End If
        If [Default] IsNot Nothing Then
            Throw New InvalidOperationException("检测到 SignalTarget 重复注册。")
        End If
        _Default = obj
    End Sub

    ''' <summary>
    ''' 注册一个 SignalContext。一旦注册就不能更改。
    ''' </summary>
    ''' <param name="name">要注册的 SignalTarget 的名称。不能为空或长度为 0。</param>
    ''' <param name="obj">要注册的 SignalTarget。不能为空。</param>
    ''' <exception cref="ArgumentNullException"><paramref name="obj"/> 是空的, 或者 <paramref name="name"/> 是 Null 或 Empty。</exception>
    ''' <exception cref="InvalidOperationException">检测到 SignalTarget 重复注册。</exception>
    Public Shared Sub Register(name As String, obj As Object)
        If obj Is Nothing Then
            Throw New ArgumentNullException(NameOf(obj))
        End If
        If String.IsNullOrEmpty(name) Then
            Throw New ArgumentNullException(NameOf(name))
        End If
        If SignalContexts.ContainsKey(name) Then
            Throw New InvalidOperationException("检测到 SignalTarget 重复注册。")
        End If

        s_SignalContexts.Add(name, obj)
    End Sub

    ''' <summary>
    ''' 检索一个指定的 SignalContext。
    ''' </summary>
    ''' <param name="name">如果是空或长度是 0, 返回默认的 SignalContext。否则查询 <see cref="SignalContexts"/>。</param>
    ''' <exception cref="KeyNotFoundException"><see cref="SignalContexts"/> 中找不到指定的键。</exception>
    Public Shared Function GetSignalContext(Optional name As String = Nothing) As Object
        If String.IsNullOrEmpty(name) Then
            Return [Default]
        Else
            Return SignalContexts(name)
        End If
    End Function
End Class
