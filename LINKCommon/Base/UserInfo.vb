Namespace Base
    ''' <summary>
    ''' 说明：定义的用户结构类  
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure UserInfo
        ''' <summary>
        ''' 说明：用户名  
        ''' </summary>
        ''' <remarks></remarks>
        Public UserName As String '操作员        ''' <summary>
        ''' 说明：岗位  
        ''' </summary>
        ''' <remarks></remarks>
        Public Post As String '操作员所在岗位        ''' <summary>
        ''' 说明：用户ID  
        ''' </summary>
        ''' <remarks></remarks>
        Public UserId As String '用户ID
        Public Password As String '//用户密码 2017/5/9
        ''' <summary>
        ''' 说明：机器码  
        ''' </summary>
        ''' <remarks></remarks>
        Public MachineId As String '机位码        ''' <summary>
        ''' 说明：是否授权开放机器码  
        ''' </summary>
        ''' <remarks></remarks>
        Public OpenMachine As String '是否开放机位码。1=开放。0为限制        ''' <summary>
        ''' 说明：IPAddress  
        ''' </summary>
        ''' <remarks></remarks>
        Public IpAddress As String 'ip地址
        ''' <summary>
        ''' 说明：所属部门  
        ''' </summary>
        ''' <remarks></remarks>
        Public DeptName As String '部门信息
        ''' <summary>
        ''' 说明：Notes Full Name   
        ''' </summary>
        ''' <remarks></remarks>
        Public NoteFullName As String '//notes用户全称
        ''' <summary>
        ''' 说明：NotesID，对应Notes组织库的用户ID  
        ''' </summary>
        ''' <remarks></remarks>
        Public NotesId As String 'notesID--一串数字        ''' <summary>
        ''' 说明：应用实例ID  
        ''' </summary>
        ''' <remarks></remarks>
        Public SysId As String
        ''' <summary>
        ''' 2017/12/11
        ''' 说明：备注信息     
        ''' </summary>
        ''' <remarks></remarks>
        Public Memo As String
        ''' <summary>
        ''' 2017/12/11
        ''' 说明：Net密码加密     
        ''' </summary>
        ''' <remarks></remarks>
        Public NetPassword As String
        ''' <summary>
        ''' 2017/12/11
        ''' 说明：NET调用机器码      
        ''' </summary>
        ''' <remarks></remarks>
        Public NetMachineId As String

    End Structure
End NameSpace