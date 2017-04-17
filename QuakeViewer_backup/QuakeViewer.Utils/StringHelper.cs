using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace QuakeViewer.Utils
{
    public class StringHelper
    {
        /// <summary>
        /// 获得一个唯一字符，16位
        /// </summary>
        /// <returns></returns>
        public static string GuidString()
        {
            Guid guid = Guid.NewGuid();
            byte[] b = guid.ToByteArray();

            long l = BitConverter.ToInt64(b, 0);

            return l.ToString();

        }

        public static string Rnd8String()
        {
            Guid guid = Guid.NewGuid();
            byte[] b = guid.ToByteArray();

            long l = BitConverter.ToInt64(b, 0);

            return l.ToString().Substring(1, 8); ;

        }

        public static string GetStrLenAll(string s, int len)
        {
            string temp = s;
            if (Regex.Replace(temp, "[^\x00-\xff]", "zz", RegexOptions.IgnoreCase).Length <= len)
            {
                return temp;
            }
            for (int i = temp.Length; i >= 0; i--)
            {
                temp = temp.Substring(0, i);
                if (Regex.Replace(temp, "[^\x00-\xff]", "zz", RegexOptions.IgnoreCase).Length <= len)
                {
                    return temp;
                }
            }
            return "";
        }

        /// <summary>
        /// 截断字符串，保留指定字节数
        /// </summary>
        /// <param name="failReason"></param>
        /// <returns></returns>
        public static string TruncateString(string input, uint keepByteCount)
        {
            while (Encoding.GetEncoding("gb2312").GetByteCount(input) > keepByteCount)
            {
                input = input.Remove(input.Length - 1);
            }
            return input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <param name="extStr"></param>
        /// <returns></returns>
        public static string SubString(string source, int length, string extStr)
        {
            if (string.IsNullOrEmpty(source))
                return source;
            if (source.Length <= length)
                return source;

            return source.Substring(0, length) + extStr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeLen"></param>
        /// <returns></returns>
        public static string CreateVerifyCode(int codeLen)
        {
            const string codeSerial = "1,2,3,4,5,6,7,8,9,0";
            //string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";


            string[] arr = codeSerial.Split(',');

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }

        /// <summary>
        /// Gets the PY string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string GetPYString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {//字母和符号原样保留
                    tempStr += c.ToString();
                }
                else
                {//累加拼音声母
                    tempStr += GetPYChar(c.ToString());
                }
            }
            return tempStr;
        }

        /// <summary>
        /// Gets the PY char.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        private static string GetPYChar(string c)
        {
            byte[] array = new byte[2];

            array = System.Text.Encoding.Default.GetBytes(c);
            if (array.Length < 2)
                return "";

            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));

            if (i < 0xB0A1) return "*";
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "j";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";

            return "*";
        }


        /// <summary>
        /// Creates the number.
        /// </summary>
        /// <param name="codeLen">The code len.</param>
        /// <returns></returns>
        public static string CreateNumber(int codeLen)
        {
            const string codeSerial = "1,2,3,4,5,6,7,8,9,0";
            string[] arr = codeSerial.Split(',');

            return CreateRandomStr(codeLen, arr);
        }

        /// <summary>
        /// Creates the password.
        /// </summary>
        /// <param name="codeLen">The code len.</param>
        /// <returns></returns>
        public static string CreatePassword(int codeLen)
        {
            const string codeSerial = "1,2,3,4,5,6,7,8,9,0,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] arr = codeSerial.Split(',');

            return CreateRandomStr(codeLen, arr);
        }



        /// <summary>
        /// Creates the random STR.
        /// </summary>
        /// <param name="codeLen">The code len.</param>
        /// <param name="baseSerial">The base serial.</param>
        /// <returns></returns>
        public static string CreateRandomStr(int codeLen, string[] baseSerial)
        {
            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, baseSerial.Length - 1);

                code += baseSerial[randValue];
            }

            return code;
        }


    }
}
