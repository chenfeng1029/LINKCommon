Namespace Base
    ''' <summary>
    ''' 创建时间：2019.8.10
    ''' 作者: kevin zhu 
    ''' 说明：安全身份验证基类，继承此基类的子类要重写    
    ''' </summary>
    ''' <typeparam name="T">泛型参数，可以是用户ID</typeparam>
    ''' <typeparam name="V">泛型参数，默认是类授权码</typeparam>
    ''' <typeparam name="P">泛型参数，默认是过程及函数授权码</typeparam>
    ''' <remarks></remarks>
    Public MustInherit Class SecuriVerify(Of T, V, P)
#Region "基类相关参数"


        ''' <summary>
        ''' 说明：授权用户名  
        ''' </summary>
        ''' <remarks>只能程序集里访问</remarks>
        Friend MUserName As T '//用户名
        ''' <summary>
        ''' 说明： 类使用的授权码  
        ''' </summary>
        ''' <remarks>只能程序集里访问</remarks>
        Friend MSecuriKey As V '//类授权码 
        ''' <summary>
        ''' 说明：类里面的函数及过程调用的授权码  
        ''' </summary>
        ''' <remarks>只能程序集里访问</remarks>
        Friend MProcKey As P '//类中函数及过程授权码
        ''' <summary>
        ''' 说明：身份验证通过与否标记  
        ''' </summary>
        ''' <remarks>程序集或派生类可以访问</remarks>
        Protected MSecuriPass As Integer
        ''' <summary>
        ''' 说明：'//过程及函数授权等级  
        ''' </summary>
        ''' <remarks>只能在程序集或者派生类中访问</remarks>
        Protected MProcLevel As EmpowerLevel
        ''' <summary>
        ''' 说明：错误信息记录  
        ''' </summary>
        ''' <remarks></remarks>
        Public MErrMessage As String
#End Region
#Region "基类相关属性"


        ''' <summary>
        ''' 说明：返回过程及函数使用的等级  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected MustOverride ReadOnly Property ProcLevel As EmpowerLevel

        ''' <summary>
        ''' 说明： 类调用前身份验证是否通过。1=通过，0=不通过  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>未通过，则无法使用调用类</remarks>
        Protected MustOverride ReadOnly Property SecuriPass As Integer '//验证码是否通过。1=通过，0=不通过
        Public MustOverride ReadOnly Property ErrMessage As String '//错误信息反馈

#End Region
        ''' <summary>
        ''' 说明：有些类确实需要不带参数的构造函数  
        ''' </summary>
        ''' <remarks>程序集内部可以访问</remarks>
        Friend Sub New()
            MSecuriPass = 0
            MProcLevel = EmpowerLevel.Z
            MErrMessage = ""
        End Sub
        ''' <summary>
        ''' 说明： 声明的构造函数保证了外部程序集不可继承该类   
        ''' </summary>
        ''' <remarks></remarks>
        Friend Sub New(ByVal vUserName As T, ByVal vSecuriKey As V, ByVal vProcKey As P)

            MSecuriPass = 0
            MProcLevel = EmpowerLevel.Z
            MErrMessage = ""
            Try
                MUserName = vUserName
                MSecuriKey = vSecuriKey
                MProcKey = vProcKey

                MSecuriPass = CheckCode(MUserName, MSecuriKey)
                If MSecuriPass = 0 Then
                    ExportMessage("Error-1<未被授权无法调用>")
                    Exit Sub
                End If
                '//进行过程及函数授权等级获取 
                MProcLevel = ProcPermission(vProcKey)
            Catch ex As Exception
                ExportMessage(ex.Message)

                Exit Sub
            End Try

        End Sub

#Region "基类相关函数及过程"


        ''' <summary>
        ''' 说明：身份验证函数，输入用户及验证码确认无误后。返回值1，否则返回值0   
        ''' </summary>
        ''' <param name="vUserName">用户名</param>
        ''' <param name="vCode">验证码</param>
        ''' <returns>验证通过1，不通过=0</returns>
        ''' <remarks>当前函数子类内部可以访问</remarks>
        Friend MustOverride Function CheckCode(vUserName As T, vCode As V) As Integer
        ''' <summary>
        ''' 说明：过程及函数调用前的验证。根据验证码返回使用等级，不同函数及过程划分几个使用等级  
        ''' </summary>
        ''' <param name="vCode">函数及过程授权码</param>
        ''' <returns></returns>
        ''' <remarks>当前函数子类内部可以访问</remarks>
        Friend MustOverride Function ProcPermission(vCode As P) As EmpowerLevel
        ''' <summary>
        ''' 说明：输出错误信息  
        ''' </summary>
        ''' <param name="vMsg"></param>
        ''' <remarks></remarks>
        Protected MustOverride Sub ExportMessage(ByVal vMsg As String)

#End Region
    End Class
End NameSpace