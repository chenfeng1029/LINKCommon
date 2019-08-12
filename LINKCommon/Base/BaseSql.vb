Imports System.Data.SqlClient

Namespace Base
    ''' <summary>
    ''' 2019.8.12
    ''' 操作数据库类库
    ''' </summary>
    Public Class BaseSql
        Inherits SecuriVerify(Of String, String, String)

#Region "类申明变量"

        ''' <summary>
        ''' 说明：错误信息记录  
        ''' </summary>
        ''' <remarks></remarks>
        Public MErrMessage As String

        ''' <summary>
        ''' 说明：'//过程及函数授权等级  
        ''' </summary>
        ''' <remarks>只能在程序集或者派生类中访问</remarks>
        Protected MProcLevel As EmpowerLevel
        Property MConnectionTimeOut As Integer = 1200
#End Region

        ''' <summary>
        ''' 说明：设置超时时间  
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ConnectionTimeOut As Integer
            Set(value As Integer)
                MConnectionTimeOut = value
            End Set
        End Property
        ''' <summary>
        ''' 返回错误信息
        ''' </summary>
        ''' <returns></returns>
        Public Overrides ReadOnly Property ErrMessage As String
            Get
                Return MErrMessage
            End Get
        End Property
        ''' <summary>
        ''' 返回使用过程及函数授权等级 
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides ReadOnly Property ProcLevel As EmpowerLevel
            Get
                Return MProcLevel
            End Get
        End Property

        Protected Overrides ReadOnly Property SecuriPass As Integer
            Get
                Return MSecuriPass
            End Get
        End Property

#Region "类重构"


        ''' <summary>
        ''' 说明：重构  
        ''' </summary>
        ''' <param name="vUserName">用户名</param>
        ''' <param name="vSecuriKey">类授权代码</param>
        ''' <param name="vProcKey">过程及函数使用授权码</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal vUserName As String, ByVal vSecuriKey As String, ByVal vProcKey As String)

            MyBase.New(vUserName, vSecuriKey, vProcKey)
        End Sub
        ''' <summary>
        ''' 说明：重构函数  
        ''' </summary>
        ''' <param name="vUserSecurity">用户安全认证信息</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal vUserSecurity As UserSecurity)
            MyBase.New(vUserSecurity.UserName, vUserSecurity.SecuriKey, vUserSecurity.ProcKey)
        End Sub

#End Region

#Region "身份校验"

        ''' <summary>
        ''' 说明：进行类身份验证  
        ''' </summary>
        ''' <param name="vUserName">用户名</param>
        ''' <param name="vCode">类授权验证码</param>
        ''' <returns>返回授权是否通过。1=通过，0=不通过</returns>
        ''' <remarks></remarks>
        Friend Overrides Function CheckCode(vUserName As String, vCode As String) As Integer
            Dim strPass As String
            MSecuriPass = 0
            Try
                strPass = Entry.Decrypt(vUserName, "password", "drowssap") + Entry.Decrypt(vCode, "password", "drowssap")


                Select Case strPass
                    Case "abcd1234"
                        MSecuriPass = 1

                End Select
            Catch ex As Exception
                MErrMessage = ex.Message
            End Try

            Return MSecuriPass
        End Function

        ''' <summary>
        ''' 说明：返回类中函数及过程授权等级  
        ''' </summary>
        ''' <param name="vCode">函数及过程使用授权码</param>
        ''' <returns>返回授权等级</returns>
        ''' <remarks></remarks>
        Friend Overrides Function ProcPermission(ByVal vCode As String) As EmpowerLevel
            Dim vKey As String
            MProcLevel = EmpowerLevel.Z
            Try
                vKey = Entry.Decrypt(vCode, "password", "drowssap")
                Select Case vKey
                    Case "19001900"
                        MProcLevel = EmpowerLevel.A
                    Case "18001800"
                        MProcLevel = EmpowerLevel.B
                    Case Else
                        MProcLevel = EmpowerLevel.Z
                End Select
            Catch ex As Exception
                ExportMessage(ex.Message)
            End Try

            Return MProcLevel
        End Function

#End Region

#Region "类自定义函数及事件"

        ''' <summary>
        ''' 说明：输出消息，一般是错误信息居多  
        ''' </summary>
        ''' <param name="vMsg"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub ExportMessage(ByVal vMsg As String)

            MErrMessage = vMsg
        End Sub

        ''' <summary>
        ''' 说明：释放链接  
        ''' </summary>
        ''' <param name="vConn"></param>
        ''' <remarks></remarks>
        Public Sub F_CancelConn(ByVal vConn As SqlConnection)
            If (ConnectionState.Closed <> vConn.State) Then
                vConn.Close()
                vConn = Nothing '//销毁连接
            End If
        End Sub

        ''' <summary>
        ''' 作者: kevin zhu
        ''' 功能：销毁sql command
        ''' </summary>
        ''' <param name="vCmd">Sqlcommand参数</param>
        ''' <remarks></remarks>
        Public Sub F_CancelCommand(ByVal vCmd As SqlCommand)
            If Not IsNothing(vCmd) Then
                vCmd.Dispose() '//销毁命令
                vCmd = Nothing
            End If
        End Sub

#End Region

        #Region "从数据库获取记录集"

        ''' <summary>
        ''' 说明：执行查询SQL语句，返回记录集内存库表DataTable   
        ''' </summary>
        ''' <param name="commandText">数据库链接字符串</param>
        ''' <param name="commandType">命令类型</param>
        ''' <param name="commandParameters">SQL语句</param>
        ''' <param name="vConnection">数据库链接字符串</param>
        ''' <returns>返回内存库表DataTable</returns>
        ''' <remarks></remarks>
        Public Overloads Function F_GetDataTable(
                                                ByVal commandText As String,
                                                ByVal commandType As CommandType,
                                                ByVal commandParameters As SqlParameter(), ByVal vConnection As String) As DataTable
            Dim dt As New DataTable
            If MProcLevel > EmpowerLevel.D Then
                ExportMessage("你无权限调用此函数")
                Return dt
            Else
                '//数据库链接字符串
                Dim cn As New SqlConnection(vConnection)
                Dim ds As New DataSet
                Try
                    If cn.State <> ConnectionState.Open Then

                        cn.Open()
                    End If
                    ds = ExecuteDataset(cn, commandType, commandText, commandParameters)
                    dt = ds.Tables(0)
                    Return dt
                Finally
                    cn.Dispose()
                End Try
                Return dt
            End If

        End Function

        ''' <summary>
        ''' 说明：执行查询sqlcommand，返回记录集Dataset  
        ''' </summary>
        ''' <param name="vSqlcon">sqlconnection参数</param>
        ''' <param name="vcommandType">命令类型</param>
        ''' <param name="vcommandText">SQL查询语句</param>
        ''' <param name="vSqlpara">参数数组</param>
        ''' <returns>数据记录集Dataset</returns>
        ''' <remarks></remarks>
        Friend Overloads Function ExecuteDataset(ByVal vSqlcon As SqlConnection,
                                                 ByVal vcommandType As CommandType,
                                                 ByVal vcommandText As String,
                                                 ByVal ParamArray vSqlpara() As SqlParameter) As DataSet


            Dim vSqlcmd As New SqlCommand()
            Dim ds As New DataSet()
            Dim da As SqlDataAdapter
            Try
                '//配置sqlcommand
                '//如果连接未打开，则打开连接
                If vSqlcon.State <> ConnectionState.Open Then

                    vSqlcon.Open()
                End If

                With vSqlcmd
                    '//设置sqlcommand对应数据库连接
                    .CommandTimeout = MConnectionTimeOut
                    .Connection = vSqlcon
                    .CommandText = vcommandText '//操作SQL命令
                    .CommandType = vcommandType '//命令操作类型
                    '//如果存在参数数组，则添加到sqlcommand
                    If Not (vSqlpara Is Nothing) Then
                        '  AttachParameters(vSqlcmd, vSqlpara)
                        Dim p As SqlParameter
                        For Each p In vSqlpara
                            '//参数可输出也 可输入

                            If p.Direction = ParameterDirection.InputOutput And p.Value Is Nothing Then
                                p.Value = Nothing
                            End If
                            '对于存储过程，有些参数是输出 
                            If p.Direction = ParameterDirection.Output Then
                                '//sqlcommand添加参数变量
                                vSqlcmd.Parameters.Add(p).Direction = ParameterDirection.Output
                            Else
                                '//sqlcommand添加参数变量
                                vSqlcmd.Parameters.Add(p)
                            End If

                        Next
                    End If
                End With
                'create the DataAdapter & DataSet
                da = New SqlDataAdapter(vSqlcmd)
                da.Fill(ds)
                '//清理sqlpara
                vSqlcmd.Parameters.Clear()
                '关闭连接池 
                F_CancelConn(vSqlcon)
            Catch ex As Exception
                '//销毁链接及操作命令
                F_CancelConn(vSqlcon)
                F_CancelCommand(vSqlcmd)
                ExportMessage(ex.Message) '传输错误信息出去
            End Try

            Return ds

        End Function

#End Region
        '
    End Class
End Namespace