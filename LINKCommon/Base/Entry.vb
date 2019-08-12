Namespace Base
    ''' <summary>
    ''' 创建时间：2019.8.12
    ''' 说明：加密解密类  
    ''' 作者：kevin zhu  
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Entry
        ''' <summary>
        ''' 说明：对字符串进行des对称加密，返回加密后的字符串  
        ''' </summary>
        ''' <param name="sourceStr">需要加密的字符串</param>
        ''' <param name="myPassKey">加密使用的Des.key(8位字符串)</param>
        ''' <param name="myPassCheckCode">加密使用的参考Des.LV(8位字符串)</param>
        ''' <returns>返回加密后的字符串</returns>
        ''' <remarks></remarks>
        Public Shared Function Encrypt(ByVal sourceStr As String, ByVal myPassKey As String, ByVal myPassCheckCode As String) As String
            Dim des As New System.Security.Cryptography.DESCryptoServiceProvider '//定义DES算法
            Dim inputByteArray As Byte()
            inputByteArray = System.Text.Encoding.Default.GetBytes(sourceStr)
            des.Key = System.Text.Encoding.UTF8.GetBytes(myPassKey) '//mypasskey des用8个字符.
            des.IV = System.Text.Encoding.UTF8.GetBytes(myPassCheckCode) '//myPassCheckCode, desc用8个字符
            Dim ms As New System.IO.MemoryStream
            Dim cs As New System.Security.Cryptography.CryptoStream(ms, des.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write)
            Dim sw As New System.IO.StreamWriter(cs)
            sw.Write(sourceStr)
            sw.Flush()
            cs.FlushFinalBlock()
            ms.Flush()
            Encrypt = Convert.ToBase64String(ms.GetBuffer(), 0, ms.Length)
        End Function



        ''' <summary>
        ''' 说明：对于DES对称加密的字符串，进行解密操作，返回真实字符串  
        ''' </summary>
        ''' <param name="sourceStr">加密字符串</param>
        ''' <param name="myPassKey">与加密对称使用的Des.key(字符串8长度)</param>
        ''' <param name="myPassCheckCode">与加密使用的参考Des.LV(字符串8长度)</param>
        ''' <returns>返回被解密后的字符串</returns>
        ''' <remarks></remarks>
        Public Shared Function Decrypt(ByVal sourceStr As String, ByVal myPassKey As String, ByVal myPassCheckCode As String) As String    '使用标准DES对称解密  

            Try
                Dim des As New System.Security.Cryptography.DESCryptoServiceProvider 'DES算法  
                des.Key = System.Text.Encoding.UTF8.GetBytes(myPassKey) 'myKey DES用8个字符，TripleDES要24个字符  
                des.IV = System.Text.Encoding.UTF8.GetBytes(myPassCheckCode) 'myIV DES用8个字符，TripleDES要24个字符  
                Dim buffer As Byte() = Convert.FromBase64String(sourceStr)
                Dim ms As New System.IO.MemoryStream(buffer)
                Dim cs As New System.Security.Cryptography.CryptoStream(ms, des.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Read)
                Dim sr As New System.IO.StreamReader(cs)
                Decrypt = sr.ReadToEnd()
            Catch ex As Exception
                Decrypt = "FALSE," + ex.Message
            End Try
            Return Decrypt

        End Function
    End Class
End NameSpace