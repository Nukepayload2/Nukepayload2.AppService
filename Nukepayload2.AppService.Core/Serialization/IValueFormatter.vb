Namespace Serialization
    ''' <summary>
    ''' 数据装入应用服务消息 <see cref="IDictionary(Of String, Object)"/> 之前, 对数据进行预先的序列化和后续反序列化。这样可以将不可序列化的类型装入应用服务消息，或者在传输之前加密。
    ''' </summary>
    Public Interface IValueFormatter
        ''' <summary>
        ''' 以同步的方式预先序列化
        ''' </summary>
        ''' <param name="data">需要预先序列化的数据</param>
        ''' <returns>将被装入应用服务消息的值</returns>
        Function Serialize(data As Object) As Object
        ''' <summary>
        ''' 以同步的方式后续反序列化
        ''' </summary>
        ''' <typeparam name="T">需要后续反序列化的数据的类型</typeparam>
        ''' <param name="data">需要后续反序列化的数据</param>
        ''' <returns>将从应用服务消息取出的值</returns>
        ''' <exception cref="InvalidCastException"><paramref name="data"/> 不能直接转换成 <typeparamref name="T"/>。</exception>
        Function Deserialize(Of T)(data As Object) As T
        ''' <summary>
        ''' 以同步的方式后续反序列化
        ''' </summary>
        ''' <param name="data">需要后续反序列化的数据</param>
        ''' <param name="type">需要后续反序列化的数据的类型</param>
        ''' <returns>将从应用服务消息取出的值</returns>
        ''' <exception cref="InvalidCastException"><paramref name="data"/> 不能直接转换成 <paramref name="type"/>。</exception>
        Function Deserialize(data As Object, type As Type) As Object
    End Interface

End Namespace