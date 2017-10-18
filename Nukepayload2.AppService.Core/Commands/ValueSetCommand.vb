Imports Nukepayload2.AppService.Core.Serialization

Namespace Commands
    ''' <summary>
    ''' 命令包的基类。代表使用 <see cref="IDictionary(Of String, Object)"/> 作为载体的命令。
    ''' </summary>
    Public MustInherit Class ValueSetCommand
        ' 内部存储
        Private _storage As IDictionary(Of String, Object)
        ' 序列化和反序列化
        Private _formatter As IValueFormatter = New NullFormatter
        ''' <summary>
        ''' 对于参数和返回值，在数据装入命令数据存储中之前和从命令数据存储取出之后进行数据格式的变换。
        ''' </summary>
        Public Property ParameterFormatter As IValueFormatter
            Get
                Return _formatter
            End Get
            Set(value As IValueFormatter)
                _formatter = value
            End Set
        End Property
        ''' <summary>
        ''' 用于创建命令包。
        ''' </summary>
        ''' <param name="storage">数据存储。不能为空。</param>
        ''' <exception cref="ArgumentNullException">数据存储不能为空。</exception>
        Public Sub New(storage As IDictionary(Of String, Object))
            If storage Is Nothing Then
                Throw New ArgumentNullException(NameOf(storage))
            End If
            _storage = storage
        End Sub
        ''' <summary>
        ''' 命令的类型
        ''' </summary>
        Public Property CommandType As CommandTypes
            Get
                If Not _storage.ContainsKey(NameOf(CommandType)) Then
                    Return CommandTypes.Unknown
                End If
                Return _storage(NameOf(CommandType))
            End Get
            Set(value As CommandTypes)
                SetValue(NameOf(CommandType), value.value__)
            End Set
        End Property
        ''' <summary>
        ''' 承载命令的数据存储
        ''' </summary>
        Public ReadOnly Property RawData As IDictionary(Of String, Object)
            Get
                Return _storage
            End Get
        End Property
        ''' <summary>
        ''' 反序列化参数和返回值
        ''' </summary>
        Protected Function Deserialize(type As Type, keyName As String) As Object
            If Not _storage.ContainsKey(keyName) Then
                Return Nothing
            End If
            Return _formatter.Deserialize(_storage(keyName), type)
        End Function
        ''' <summary>
        ''' 序列化参数和返回值并装入存储
        ''' </summary>
        Protected Sub Serialize(keyName As String, data As Object)
            Dim value = _formatter.Serialize(data)
            SetValue(keyName, value)
        End Sub
        ''' <summary>
        ''' 从内部存储获取值。
        ''' </summary>
        ''' <param name="keyName">键名。使用 NameOf (或 nameof) 关键字产生。</param>
        Protected Function GetValue(keyName As String) As Object
            If Not _storage.ContainsKey(keyName) Then
                Return Nothing
            End If
            Return _storage(keyName)
        End Function
        ''' <summary>
        ''' 直接向内部存储写入值。
        ''' </summary>
        ''' <param name="keyName">键名。使用 NameOf (或 nameof) 关键字产生。</param>
        ''' <param name="value">要写入的值。</param>
        Protected Sub SetValue(keyName As String, value As Object)
            If _storage.ContainsKey(keyName) Then
                _storage(keyName) = value
            Else
                _storage.Add(keyName, value)
            End If
        End Sub
    End Class
End Namespace