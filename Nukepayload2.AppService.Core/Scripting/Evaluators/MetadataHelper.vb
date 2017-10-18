Imports System.Reflection

Namespace Scripting.Evaluators
    Public Class MetadataHelper
        Public Shared Function TryFindMethod(targetType As Type, methodName As String, params() As Object, memberFilter As RemoteVisibilityCheckMode) As MethodInfo
            Return TryFindMember(targetType, methodName, params, Function(tp) tp.GetRuntimeMethods, Function(method) method.GetParameters, memberFilter)
        End Function

        Public Shared Function TryFindProperty(targetType As Type, propName As String, indexes() As Object, memberFilter As RemoteVisibilityCheckMode) As PropertyInfo
            Return TryFindMember(targetType, propName, indexes, Function(tp) tp.GetRuntimeProperties, Function(prop) prop.GetIndexParameters, memberFilter)
        End Function

        Private Shared Function TryFindMember(Of T As MemberInfo)(targetType As Type, methodName As String, params() As Object,
                                                                  getMembers As Func(Of Type, IEnumerable(Of T)),
                                                                  getParams As Func(Of T, ParameterInfo()),
                                                                  memberFilter As RemoteVisibilityCheckMode) As T
            Return Aggregate member In getMembers(targetType)
                   Where member.Name = methodName AndAlso
                   memberFilter(member)
                   Let paramInfoes = getParams(member)
                   Where params Is Nothing AndAlso paramInfoes.Length = 0 OrElse
                       params.Length = paramInfoes.Length AndAlso
                       HasMatchedParams(params, paramInfoes)
                   Select member Into FirstOrDefault
        End Function

        Private Shared Function HasMatchedParams(params() As Object, paramInfoes() As ParameterInfo) As Boolean
            For i = 0 To paramInfoes.Length - 1
                Dim curParam = params(i)
                Dim curParamInfo = paramInfoes(i)
                Dim parameterType As Type = curParamInfo.ParameterType
                Dim paramTypeInfo As TypeInfo = parameterType.GetTypeInfo
                ' 为空的参数, 包括 <Out> ByRef 参数
                If curParam IsNot Nothing Then
                    ' 不为空的参数，包括 传入传出的 ByRef 参数
                    Dim curParamTypeInfo = curParam.GetType.GetTypeInfo
                    If parameterType.IsByRef Then
                        curParamTypeInfo = curParamTypeInfo.MakeByRefType.GetTypeInfo
                    End If
                    If Not paramTypeInfo.IsAssignableFrom(curParamTypeInfo) Then
                        Return False
                    End If
                End If
            Next
            Return True
        End Function

        Public Shared Function InvokeOrGetValue(target As Object, memberName As String, args As Object(), checkMode As RemoteVisibilityCheckMode) As Object
            Dim memInfo = FindMemberInfo(target, memberName, args, checkMode)
            Return UncheckedInvokeOrGetValue(target, memInfo, args)
        End Function

        Public Shared Function UncheckedInvokeOrGetValue(target As Object, memInfo As MemberInfo, args As Object()) As Object
            If TypeOf memInfo Is PropertyInfo Then
                Dim prop = DirectCast(memInfo, PropertyInfo)
                Return prop.GetValue(target, args)
            ElseIf TypeOf memInfo Is MethodInfo Then
                Dim method = DirectCast(memInfo, MethodInfo)
                Dim returnValue = method.Invoke(target, args)
                Return returnValue
            Else
                Throw New ExpressionExecutionException($"内部异常： {NameOf(FindMemberInfo)} 返回类型不是预期的。")
            End If
        End Function

        Public Shared Function FindMemberInfo(target As Object, memberName As String, args As Object(), checkMode As RemoteVisibilityCheckMode) As MemberInfo
            ' 如果是写属性, args 不应该在找属性的时候使用。
            ' 如果是方法调用, args 应当用来筛选参数。
            Dim prop = TryFindProperty(target.GetType, memberName, args, checkMode)
            If prop IsNot Nothing Then
                Return prop
            End If
            Dim method = TryFindMethod(target.GetType, memberName, args, checkMode)
            If method IsNot Nothing Then
                Return method
            End If
            Throw New ExpressionExecutionException($"找不到属性或方法 {memberName}, 无法调用或获取值。")
        End Function
    End Class
End Namespace