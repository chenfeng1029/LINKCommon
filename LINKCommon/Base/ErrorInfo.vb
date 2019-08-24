Namespace Base
    ''' <summary>
    ''' 创建时间：2019.8.23
    ''' 作者:kevin zhu
    ''' 说明：错误信息处理的类  
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ErrorInfo
        ''' <summary>
        ''' 说明：根据错误代码，返回错误信息  
        ''' </summary>
        ''' <param name="vIndex">错误代码</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ErrorMessage(ByVal vIndex As String) As String
            Dim strError As String
            strError = ""
            Select Case vIndex
                Case Is = 1 '//未授权
                    strError = "Error-1<未被授权无法调用>"
                Case Is = 2 '//窗体加载失败
                    strError = " Error -2<窗体加载失败>"
            End Select
            Return strError
        End Function
    End Class
End NameSpace