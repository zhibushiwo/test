using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Express.Common.SiteOperate
{
    public class StringPlus
    {
        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }
        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[',']);
        }
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }


        #region 删除最后一个字符之后的字符

        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        #endregion

        ///   <summary>
        ///   去除HTML标记
        ///   </summary>
        ///   <param   name=”NoHTML”>包括HTML的源码   </param>
        ///   <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
            RegexOptions.IgnoreCase);
            //删除HTML 
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"–>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!–.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = System.Web.HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }

        /// <summary>
        /// 取得HTML中所有图片的 URL。
        /// </summary>
        /// <param name="sHtmlText">HTML代码</param>
        /// <returns>图片的URL列表</returns>
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串
            MatchCollection matches = regImg.Matches(sHtmlText);

            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表
            foreach (Match match in matches)
                sUrlList[i++] = match.Value;

            return sUrlList;
        }

        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        public static List<string> GetSubStringList(string o_str, char sepeater)
        {
            List<string> list = new List<string>();
            string[] ss = o_str.Split(sepeater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != sepeater.ToString())
                {
                    list.Add(s);
                }
            }
            return list;
        }


        #region 将字符串样式转换为纯字符串
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            string RetrunValue = "";
            //如果为空，返回空值
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                string NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }
            return RetrunValue;
        }
        #endregion

        #region 将字符串转换为新样式
        public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
        {
            string ReturnValue = "";
            //如果输入空值，返回空，并给出错误提示
            if (StrList == null)
            {
                ReturnValue = "";
                Error = "请输入需要划分格式的字符串";
            }
            else
            {
                //检查传入的字符串长度和样式是否匹配,如果不匹配，则说明使用错误。给出错误信息并返回空值
                int strListLength = StrList.Length;
                int NewStyleLength = GetCleanStyle(NewStyle, SplitString).Length;
                if (strListLength != NewStyleLength)
                {
                    ReturnValue = "";
                    Error = "样式格式的长度与输入的字符长度不符，请重新输入";
                }
                else
                {
                    //检查新样式中分隔符的位置
                    string Lengstr = "";
                    for (int i = 0; i < NewStyle.Length; i++)
                    {
                        if (NewStyle.Substring(i, 1) == SplitString)
                        {
                            Lengstr = Lengstr + "," + i;
                        }
                    }
                    if (Lengstr != "")
                    {
                        Lengstr = Lengstr.Substring(1);
                    }
                    //将分隔符放在新样式中的位置
                    string[] str = Lengstr.Split(',');
                    foreach (string bb in str)
                    {
                        StrList = StrList.Insert(int.Parse(bb), SplitString);
                    }
                    //给出最后的结果
                    ReturnValue = StrList;
                    //因为是正常的输出，没有错误
                    Error = "";
                }
            }
            return ReturnValue;
        }
        #endregion

        public static string CtrlStringLength2(string str, int length)
        {
            if (str == null)
                return "";

            if (str.Length <= length)
                return str;

            return str.Substring(0, length);
        }
        public static string CtrlStringLength(string str, int length)
        {
            if (str == null)
                return "";

            if (str.Length <= length)
                return str;

            return str.Substring(0, length) + "...";
        }

        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="allChar">用,分隔字符</param>
        /// <param name="CodeCount"></param>
        /// <returns></returns>
        public static string GetRandomCode(string allChar, int CodeCount)
        {
            //string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z"; 
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }
            return RandomCode;
        }






        public class CosineSimilarAlgorithm
        {
            public static double getSimilarity(string doc1, string doc2)
            {
                if (doc1 != null && doc1.Trim().Length > 0 && doc2 != null && doc2.Trim().Length > 0)
                {

                    IDictionary<int?, int[]> AlgorithmMap = new Dictionary<int?, int[]>();

                    //将两个字符串中的中文字符以及出现的总数封装到，AlgorithmMap中
                    for (int i = 0; i < doc1.Length; i++)
                    {
                        char d1 = doc1[i];
                        if (isHanZi(d1))
                        {
                            int charIndex = getGB2312Id(d1);
                            if (charIndex != -1)
                            {
                                int[] fq = AlgorithmMap[charIndex];
                                if (fq != null && fq.Length == 2)
                                {
                                    fq[0]++;
                                }
                                else
                                {
                                    fq = new int[2];
                                    fq[0] = 1;
                                    fq[1] = 0;
                                    AlgorithmMap[charIndex] = fq;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < doc2.Length; i++)
                    {
                        char d2 = doc2[i];
                        if (isHanZi(d2))
                        {
                            int charIndex = getGB2312Id(d2);
                            if (charIndex != -1)
                            {
                                int[] fq = AlgorithmMap[charIndex];
                                if (fq != null && fq.Length == 2)
                                {
                                    fq[1]++;
                                }
                                else
                                {
                                    fq = new int[2];
                                    fq[0] = 0;
                                    fq[1] = 1;
                                    AlgorithmMap[charIndex] = fq;
                                }
                            }
                        }
                    }

                    IEnumerator<int?> iterator = AlgorithmMap.Keys.GetEnumerator();
                    double sqdoc1 = 0;
                    double sqdoc2 = 0;
                    double denominator = 0;
                    while (iterator.MoveNext())
                    {
                        int[] c = AlgorithmMap[iterator.Current];
                        denominator += c[0] * c[1];
                        sqdoc1 += c[0] * c[0];
                        sqdoc2 += c[1] * c[1];
                    }

                    return denominator / Math.Sqrt(sqdoc1 * sqdoc2);
                }
                else
                {
                    throw new System.NullReferenceException(" the Document is null or have not cahrs!!");
                }
            }

            public static bool isHanZi(char ch)
            {
                // 判断是否汉字
                return (ch >= 0x4E00 && ch <= 0x9FA5);

            }

            /// <summary>
            /// 根据输入的Unicode字符，获取它的GB2312编码或者ascii编码，
            /// </summary>
            /// <param name="ch">
            ///            输入的GB2312中文字符或者ASCII字符(128个) </param>
            /// <returns> ch在GB2312中的位置，-1表示该字符不认识 </returns>
            public static short getGB2312Id(char ch)
            {
                try
                {
                    sbyte[] buffer = StringHelperClass.GetBytes(ch.ToString(), "GB2312");
                    if (buffer.Length != 2)
                    {
                        // 正常情况下buffer应该是两个字节，否则说明ch不属于GB2312编码，故返回'?'，此时说明不认识该字符
                        return -1;
                    }
                    int b0 = (int)(buffer[0] & 0x0FF) - 161; // 编码从A1开始，因此减去0xA1=161
                    int b1 = (int)(buffer[1] & 0x0FF) - 161; // 第一个字符和最后一个字符没有汉字，因此每个区只收16*6-2=94个汉字
                    return (short)(b0 * 94 + b1);
                }
                catch (Exception e)
                {
                }
                return -1;
            }
        }

        
    }

    //-------------------------------------------------------------------------------------------
    //	Copyright © 2007 - 2014 Tangible Software Solutions Inc.
    //	This class can be used by anyone provided that the copyright notice remains intact.
    //
    //	This class is used to convert some aspects of the Java String class.
    //-------------------------------------------------------------------------------------------
    internal static class StringHelperClass
    {
        //----------------------------------------------------------------------------------
        //	This method replaces the Java String.substring method when 'start' is a
        //	method call or calculated value to ensure that 'start' is obtained just once.
        //----------------------------------------------------------------------------------
        internal static string SubstringSpecial(this string self, int start, int end)
        {
            return self.Substring(start, end - start);
        }

        //------------------------------------------------------------------------------------
        //	This method is used to replace calls to the 2-arg Java String.startsWith method.
        //------------------------------------------------------------------------------------
        internal static bool StartsWith(this string self, string prefix, int toffset)
        {
            return self.IndexOf(prefix, toffset, System.StringComparison.Ordinal) == toffset;
        }

        //------------------------------------------------------------------------------
        //	This method is used to replace most calls to the Java String.split method.
        //------------------------------------------------------------------------------
        internal static string[] Split(this string self, string regexDelimiter, bool trimTrailingEmptyStrings)
        {
            string[] splitArray = System.Text.RegularExpressions.Regex.Split(self, regexDelimiter);

            if (trimTrailingEmptyStrings)
            {
                if (splitArray.Length > 1)
                {
                    for (int i = splitArray.Length; i > 0; i--)
                    {
                        if (splitArray[i - 1].Length > 0)
                        {
                            if (i < splitArray.Length)
                                System.Array.Resize(ref splitArray, i);

                            break;
                        }
                    }
                }
            }

            return splitArray;
        }

        //-----------------------------------------------------------------------------
        //	These methods are used to replace calls to some Java String constructors.
        //-----------------------------------------------------------------------------
        internal static string NewString(sbyte[] bytes)
        {
            return NewString(bytes, 0, bytes.Length);
        }
        internal static string NewString(sbyte[] bytes, int index, int count)
        {
            return System.Text.Encoding.UTF8.GetString((byte[])(object)bytes, index, count);
        }
        internal static string NewString(sbyte[] bytes, string encoding)
        {
            return NewString(bytes, 0, bytes.Length, encoding);
        }
        internal static string NewString(sbyte[] bytes, int index, int count, string encoding)
        {
            return System.Text.Encoding.GetEncoding(encoding).GetString((byte[])(object)bytes, index, count);
        }

        //--------------------------------------------------------------------------------
        //	These methods are used to replace calls to the Java String.getBytes methods.
        //--------------------------------------------------------------------------------
        internal static sbyte[] GetBytes(this string self)
        {
            return GetSBytesForEncoding(System.Text.Encoding.UTF8, self);
        }
        internal static sbyte[] GetBytes(this string self, string encoding)
        {
            return GetSBytesForEncoding(System.Text.Encoding.GetEncoding(encoding), self);
        }
        private static sbyte[] GetSBytesForEncoding(System.Text.Encoding encoding, string s)
        {
            sbyte[] sbytes = new sbyte[encoding.GetByteCount(s)];
            encoding.GetBytes(s, 0, s.Length, (byte[])(object)sbytes, 0);
            return sbytes;
        }
    }
}
