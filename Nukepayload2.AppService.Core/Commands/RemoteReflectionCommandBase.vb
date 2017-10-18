Imports Nukepayload2.AppService.Core.InteropServices

Namespace Commands
    ''' <summary>
    ''' 基于反射的远程调用命令基类。
    ''' </summary>
    Public MustInherit Class RemoteReflectionCommandBase
        Inherits ValueSetCommand

        ''' <exception cref="ArgumentNullException">数据存储不能为空。</exception>
        Public Sub New(storage As IDictionary(Of String, Object))
            MyBase.New(storage)
        End Sub

        ''' <summary>
        ''' 参数列表。设置参数会设置参数类型名称。
        ''' </summary>
        ''' <param name="index">参数的下标</param>
        ''' <exception cref="ArgumentOutOfRangeException">下标不能小于 0。</exception>
        Public Property Arg(index As Integer) As Object
            Get
                Dim tp = Type.GetType(ArgTypeName(index))
                If tp Is Nothing Then
                    Throw New InvalidOperationException($"参数名不符合协议约定。{ArgTypeName(index)} 类型找不到。")
                End If
                If index < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(index))
                End If
                Dim keyName = NameOf(Arg) & index
                Return Deserialize(tp, keyName)
            End Get
            Set(value As Object)
                If index < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(index))
                End If
                Dim keyName = NameOf(Arg) & index
                Serialize(keyName, value)
                ArgTypeName(index) = MetadataHelper.GetTypeName(value)
            End Set
        End Property

        ''' <summary>
        ''' 参数列表中的类型名称。要获取的类型的程序集限定名称。请参阅 <see cref="Type.AssemblyQualifiedName"/>。如果该类型位于当前正在执行的程序集中或者 Mscorlib.dll 中，则提供由命名空间限定的类型名称就足够了。
        ''' </summary>
        ''' <param name="index">参数类型的下标</param>
        ''' <exception cref="ArgumentOutOfRangeException">下标不能小于 0。</exception>
        Public Property ArgTypeName(index As Integer) As String
            Get
                If index < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(index))
                End If
                Dim keyName = NameOf(ArgTypeName) & index
                Return Deserialize(GetType(String), keyName)
            End Get
            Set(value As String)
                If index < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(index))
                End If
                Dim keyName = NameOf(ArgTypeName) & index
                Serialize(keyName, value)
            End Set
        End Property

        ''' <summary>
        ''' 参数数量
        ''' </summary>
        Public Property ArgCount As Integer
            Get
                Return GetValue(NameOf(ArgCount))
            End Get
            Set(value As Integer)
                SetValue(NameOf(ArgCount), value)
            End Set
        End Property

        ''' <summary>
        ''' 将参数复制到新的数组。
        ''' </summary>
        Public Function GetArgs() As Object()
            Dim argc = ArgCount
            Dim args(argc - 1) As Object
            For i = 0 To argc - 1
                args(i) = Arg(i)
            Next
            Return args
        End Function
        ''' <summary>
        ''' 向参数存储变量填充参数。对已经初始化参数列表的命令调用可能带来意外的结果。
        ''' </summary>
        Public Sub InitializeArgs(args As Object())
            If args Is Nothing Then
                Return
            End If
            ArgCount = args.Count
            For i = 0 To args.Length - 1
                Arg(i) = args(i)
            Next
        End Sub
    End Class

End Namespace