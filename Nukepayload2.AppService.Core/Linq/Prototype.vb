Imports System.Linq.Expressions

Namespace Linq

    Friend Class Prototype
        Private Class RemoteObject
            Property Value As Integer
        End Class

        Async Function RemoteCallAsync(Of TTarget, TResult)(target As TTarget, expr As Expression(Of Func(Of TTarget, TResult))) As Task(Of TResult)
            Await Task.CompletedTask
            Return Nothing
        End Function

        Async Function RemoteCallAsync(Of TTarget)(target As TTarget, expr As Expression(Of Action(Of TTarget))) As Task
            Await Task.CompletedTask
        End Function

        Async Function TestAsync() As Task
            Dim o As New RemoteObject
            Await RemoteCallAsync(o, Sub(test) test.Value.Set(2))
            Dim value = Await RemoteCallAsync(o, Function(test) test.Value)
        End Function

    End Class

End Namespace