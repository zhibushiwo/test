using System;
using System.Text;
using System.Security.Cryptography;
namespace Express.Common.DEncrypt
{
    /// <summary>
    /// 得到随机安全码（哈希加密）。
    /// </summary>
    public class HashEncode
    {
        public HashEncode()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 得到随机哈希加密字符串
        /// </summary>
        /// <returns></returns>
        public static string GetSecurity()
        {
            string Security = HashEncoding(GetRandomValue());
            return Security;
        }
        /// <summary>
        /// 得到一个随机数值
        /// </summary>
        /// <returns></returns>
        public static string GetRandomValue()
        {
            Random Seed = new Random();
            string RandomVaule = Seed.Next(1, int.MaxValue).ToString();
            return RandomVaule;
        }
        /// <summary>
        /// 哈希加密一个字符串
        /// </summary>
        /// <param name="Security"></param>
        /// <returns></returns>
        public static string HashEncoding(string Security)
        {
            byte[] Value;
            UnicodeEncoding Code = new UnicodeEncoding();
            byte[] Message = Code.GetBytes(Security);
            SHA512Managed Arithmetic = new SHA512Managed();
            Value = Arithmetic.ComputeHash(Message);
            Security = "";
            foreach (byte o in Value)
            {
                Security += (int)o + "O";
            }
            return Security;
        }

        public static string EncodeBase64(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }

        public static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }

        public static string MD5(string val)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(val);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
    }
}
