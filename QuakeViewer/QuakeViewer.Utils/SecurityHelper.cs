using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace NextStep.Utils
{
    /// <summary>
    /// 封装安全相关，加密，解密方法
    /// </summary>
    public static class SecurityHelper
    {
        #region###Encode/Decode
        public static string Encode(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str).Reverse().ToArray();

            string rtv = Convert.ToBase64String(b);

            return rtv;
        }

        public static string Decode(string str)
        {
            byte[] b = Convert.FromBase64String(str).Reverse().ToArray();

            string rtv = Encoding.UTF8.GetString(b);

            return rtv;
        }
        #endregion

        /// <summary>
        /// 获取字符串的md5值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.Default.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptToSHA1(string str)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] str1 = Encoding.Default.GetBytes(str);
            byte[] str2 = sha1.ComputeHash(str1);
            sha1.Clear();
            (sha1 as IDisposable).Dispose();
            return ToString(str2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptToSHA256(string str)
        {
            SHA256 shaM = new SHA256Managed();


            byte[] str1 = Encoding.Default.GetBytes(str);
            byte[] str2 = shaM.ComputeHash(str1);
            shaM.Clear();
            (shaM as IDisposable).Dispose();
            return ToString(str2);
        }

        /// <summary>
        /// 转换成十六进制字符串
        /// </summary>
        /// <param name="b">二进制数组</param>
        /// <returns></returns>
        public static string ToString(byte[] b)
        {
            return ToString(b, 0, b.Length);
        }
        /// <summary>
        /// 转换成十六进制字符串
        /// </summary>
        /// <param name="b">二进制数组</param>
        /// <param name="startIndex">从何位置开始转换</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        private static string ToString(byte[] b, int startIndex, int length)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = startIndex; i < startIndex + length; i++)
            {
                stringBuilder.Append(b[i].ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        #region 3DES加密
        ///  <summary > 
        /// 3DES加密 
        ///  </summary > 
        ///  <param name="value" >待加密字符串 </param > 
        ///  <param name="sKey" >密钥 </param > 
        ///  <param name="sIV" >矢量 </param > 
        ///  <returns >加密后字符串 </returns > 
        public static string EncryptTo3DES(string inputStr,string sKey,string sIV)
        {
            //构造对称算法 
            SymmetricAlgorithm mCSP = new DESCryptoServiceProvider();

            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = Encoding.UTF8.GetBytes(sKey);
            mCSP.IV = Encoding.UTF8.GetBytes(sIV);
            //指定加密的运算模式 
            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            //获取或设置加密算法的填充模式 
            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);
            byt = Encoding.UTF8.GetBytes(inputStr);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();

            string rtv = Convert.ToBase64String(ms.ToArray());
            rtv = rtv.Replace("/", "-");
            rtv = rtv.Replace("+", "_");
            rtv = rtv.Replace("=", "|");
            return rtv;
        }

        ///  <summary > 
        /// 3DES解密 
        ///  </summary > 
        ///  <param name="Value" >待解密字符串 </param > 
        ///  <param name="sKey" >密钥 </param > 
        ///  <param name="sIV" >矢量 </param > 
        ///  <returns >解密后字符串 </returns > 
        public static string DecryptTo3DES(string inputStr,string sKey,string sIV)
        {
            inputStr = inputStr.Replace("-", "/");
            inputStr = inputStr.Replace("_", "+");
            inputStr = inputStr.Replace("|", "=");
            //构造对称算法 
            SymmetricAlgorithm mCSP = new DESCryptoServiceProvider();

            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = Encoding.UTF8.GetBytes(sKey);
            mCSP.IV = Encoding.UTF8.GetBytes(sIV);
            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);
            byt = Convert.FromBase64String(inputStr);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        #endregion

        /// <summary>
        /// 生成20位Token
        /// </summary>
        /// <param name="baseString"></param>
        /// <returns></returns>
        public static string CreateToken(string baseString)
        {
            var md5 = new MD5CryptoServiceProvider();
            var bytes = Encoding.Default.GetBytes(baseString);
            string bitResult = BitConverter.ToString(md5.ComputeHash(bytes));
            return bitResult.Replace("-", string.Empty);
        }
    }
}
