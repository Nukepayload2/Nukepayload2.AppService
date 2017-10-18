Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Nukepayload2.AppService.Core.InteropServices.Primitives
Imports Windows.ApplicationModel.AppService
Imports AppServiceResponseStatus = Windows.ApplicationModel.AppService.AppServiceResponseStatus
Imports N2AppServiceResponseStatus = Nukepayload2.AppService.Core.InteropServices.Primitives.AppServiceResponseStatus

Namespace InteropServices
    Public Module AppServiceResponseStatusAndN2AppServiceResponseStatusConverter
        <StructLayout(LayoutKind.Explicit)>
        Private Structure AppServiceResponseStatusN2AppServiceResponseStatusUnion
            <FieldOffset(0)>
            Dim AppServiceResponseStatusValue As AppServiceResponseStatus
            <FieldOffset(0)>
            Dim N2AppServiceResponseStatusValue As N2AppServiceResponseStatus
        End Structure
        ''' <summary>
        ''' 将 AppServiceResponseStatus 转换成 N2AppServiceResponseStatus
        ''' </summary>
        ''' <param name="AppServiceResponseStatusValue">AppServiceResponseStatus</param>
        ''' <returns>N2AppServiceResponseStatus</returns>
        <Extension>
        Public Function AsN2AppServiceResponseStatus(AppServiceResponseStatusValue As AppServiceResponseStatus) As N2AppServiceResponseStatus
            Return (New AppServiceResponseStatusN2AppServiceResponseStatusUnion With {.AppServiceResponseStatusValue = AppServiceResponseStatusValue}).N2AppServiceResponseStatusValue
        End Function
        ''' <summary>
        ''' 将 N2AppServiceResponseStatus 转换成 AppServiceResponseStatus
        ''' </summary>
        ''' <param name="N2AppServiceResponseStatusValue">N2AppServiceResponseStatus</param>
        ''' <returns>AppServiceResponseStatus</returns>
        <Extension>
        Public Function AsAppServiceResponseStatus(N2AppServiceResponseStatusValue As N2AppServiceResponseStatus) As AppServiceResponseStatus
            Return (New AppServiceResponseStatusN2AppServiceResponseStatusUnion With {.N2AppServiceResponseStatusValue = N2AppServiceResponseStatusValue}).AppServiceResponseStatusValue
        End Function

    End Module

    Public Module AppServiceResponseConverter
        <Extension>
        Public Function AsRemoteResponse(this As AppServiceResponse) As RemoteResponse
            Return New RemoteResponse(this.Message, this.Status.AsN2AppServiceResponseStatus)
        End Function
    End Module
End Namespace