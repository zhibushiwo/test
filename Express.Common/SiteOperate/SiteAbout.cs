using System;
using System.Text;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Text.RegularExpressions;
using Express.Common.DEncrypt;
using Express.Common.FileOperate;

namespace Express.Common.SiteOperate
{
	/// <summary>
	/// SiteAbout
	/// </summary>
	public class SiteAbout
	{
		#region Request.Form Request.QueryString
        /// <summary>
        /// ����QueryString�е�queryParamNameֵ��string�ͣ��������ڻ�Ϊ���򷵻�""
        /// </summary>
        /// <param name="queryParamName"></param>
        /// <returns></returns>
		public static string qStr(string queryParamName)
		{
			return qStr(queryParamName,"") ;
		}
        /// <summary>
        /// ����QueryString�е�queryParamNameֵ��int�ͣ��������ڻ�Ϊ���򷵻�-1
        /// </summary>
        /// <param name="queryParamName"></param>
        /// <returns></returns>
		public static int qInt(string queryParamName)
		{
			return int.Parse(qStr(queryParamName,"-1")) ;
		}
        /// <summary>
        /// ����QueryString�е�queryParamNameֵ��int�ͣ��������ڻ�Ϊ���򷵻�defaultValue
        /// </summary>
        /// <param name="queryParamName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
		public static int qInt(string queryParamName,int defaultValue)
		{
			return int.Parse(qStr(queryParamName,defaultValue.ToString())) ;
		}
        /// <summary>
        /// ����QueryString�е�queryParamNameֵ��string�ͣ��������ڻ�Ϊ���򷵻�defaultValue
        /// </summary>
        /// <param name="queryParamName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
		public static string qStr(string queryParamName,string defaultValue)
		{
			string val = System.Web.HttpContext.Current.Request.QueryString[queryParamName] ;
			if ((val == null) || (val == "") )
				return defaultValue ;
			return System.Web.HttpContext.Current.Server.UrlDecode(val) ;
		}
        /// <summary>
        /// ����Form�е�queryParamNameֵ��int�ͣ��������ڻ�Ϊ���򷵻�defaultValue
        /// </summary>
        /// <param name="queryParamName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
		public static int fInt(string queryParamName,int defaultValue)
		{
			return int.Parse(fStr(queryParamName,defaultValue.ToString())) ;
		}
        /// <summary>
        /// ����Form�е�queryParamNameֵ��int�ͣ��������ڻ�Ϊ���򷵻�-1
        /// </summary>
        /// <param name="queryParamName"></param>
        /// <returns></returns>
        public static int fInt(string queryParamName)
        {
            return fInt(queryParamName, -1);
        }
        /// <summary>
        /// ����Form�е�queryParamNameֵ��string�ͣ��������ڻ�Ϊ���򷵻�defaultValue
        /// </summary>
        /// <param name="queryParamName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
		public static string fStr(string queryParamName,string defaultValue)
		{
            string val = System.Web.HttpContext.Current.Server.UrlDecode(System.Web.HttpContext.Current.Request.Form[queryParamName]);
			if ((val == null) || (val == ""))
				return defaultValue ;
			return val ;
		}
        /// <summary>
        /// ����Form�е�queryParamNameֵ��string�ͣ��������ڻ�Ϊ���򷵻�""
        /// </summary>
        /// <param name="queryParamName"></param>
        /// <returns></returns>
        public static string fStr(string queryParamName)
        {
            return fStr(queryParamName,"");
        }
        /// <summary>
        /// ����Request�е�queryParamNameֵ��int�ͣ��������ڻ�Ϊ���򷵻�defaultValue
        /// </summary>
        /// <param name="queryParamName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int gInt(string queryParamName, int defaultValue)
        {
            int i = defaultValue;
            if(string.IsNullOrEmpty(gStr(queryParamName)))
                return defaultValue;
            if (int.TryParse(gStr(queryParamName), out i))
            {
                return i;
            }
            else
                return defaultValue;
        }
        /// <summary>
        /// ����Request�е�queryParamNameֵ��int�ͣ��������ڻ�Ϊ���򷵻�-1
        /// </summary>
        /// <param name="queryParamName"></param>
        /// <returns></returns>
        public static int gInt(string queryParamName)
        {
            return gInt(queryParamName, -1);
        }
        /// <summary>
        /// ����Request�е�queryParamNameֵ��string�ͣ��������ڻ�Ϊ���򷵻�""
        /// </summary>
        /// <param name="queryParamName"></param>
        /// <returns></returns>
        public static string gStr(string queryParamName)
        {
            string str= gStr(queryParamName, "");
            return str;
        }
        /// <summary>
        /// ����Request�е�queryParamNameֵ��string�ͣ��������ڻ�Ϊ���򷵻�defaultValue
        /// </summary>
        /// <param name="queryParamName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string gStr(string queryParamName, string defaultValue)
        {
            string val = System.Web.HttpContext.Current.Request[queryParamName];
            if ((val == null) || (val == ""))
                return defaultValue;
            return val;
        }
		#endregion


		#region Get/Set Application Session Cookie
        /// <summary>
        /// ��ȡsessionֵ
        /// </summary>
        /// <param name="varName">key</param>
        /// <returns>value</returns>
        public static string GetSValue(string varName)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            object o = context.Session[varName];
            return o == null ? "" : o.ToString();
        }
        /// <summary>
        /// ��session��ȡֵ����������ȡcookie�е�ֵ�����浽session
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
		public static string GetValue(string varName)
		{
            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string val = "";
                if (context.Session[varName] != null)
                    val = context.Session[varName].ToString();
                else
                {
                    if (IsSaveCookie)
                        val = GetCookie(varName);
                    if (val != "")
                        SetSValue(varName, val);
                }
                return val;
            }
            catch (System.Exception ex)
            {
                string val = "";

                return val;
            }

		}
        /// <summary>
        /// ��cookie��session�����ñ���
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
        public static void SetValueForever(string varName, string varValue)
		{
			//�ü�������߰�ȫ
		    SetCookieForever(varName,varValue) ;
			SetSValue(varName,varValue) ;
		}
        /// <summary>
        /// �������ý�ֵ���浽session��cookie
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
        public static void SetValue(string varName, string varValue)
        {
            //�ü�������߰�ȫ
            if (IsSaveCookie)
                SetCookieForever(varName, varValue);
            SetSValue(varName, varValue);
        }
        /// <summary>
        /// ��dt������application��
        /// </summary>
        /// <param name="dt">datatable</param>
        /// <param name="fldVarName">key</param>
        /// <param name="fldVarValue">value</param>
		public static void SetAppValues(DataTable dt,string fldVarName,string fldVarValue)
		{
			foreach(DataRow dr in dt.Rows)
			{
				SetAppValue(dr[fldVarName].ToString(),dr[fldVarValue]) ;
			}
		}
        /// <summary>
        /// ��dt������application��
        /// </summary>
        /// <param name="dt"></param>
		public static void SetAppValues(DataTable dt)
		{
			SetAppValues(dt,"VarName","VarValue") ;
		}
        /// <summary>
        /// ����application
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="val">value</param>
		public static void SetAppValue(string name,object val)
		{
            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                context.Application.Lock();
                if (context.Application[name] != null)
                    context.Application[name] = val;
                else
                    context.Application.Add(name, val);
                context.Application.UnLock();
            }
            catch (Exception ex)
            {
                //if (RuntimeCache.Contains<object>(name))
                //{
                //    RuntimeCache.Remove(name);
                //}
                //RuntimeCache.Add(name, val);

            }
		}
        /// <summary>
        /// ��ȡapplication�е�ֵ
        /// </summary>
        /// <param name="name">key</param>
        /// <returns>Application[key]</returns>
		public static object GetAppValue(string name)
		{
            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                object obj = context.Application[name];
                return context.Application[name];
            }
            catch (System.Exception ex)
            {
                object obj=null;
                //RuntimeCache.TryGetValue(name, out obj);
                return obj;
            }

		}

		/// <summary>
		/// ֻ����SESSION
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public static void SetSValue(string name, object value)
		{
            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                if (context.Session[name] != null)
                    context.Session[name] = value;
                else
                    context.Session.Add(name, value);
            }
            catch (System.Exception ex)
            {
                //if (RuntimeCache.Contains<object>(name))
                //    RuntimeCache.Remove(name);
                //RuntimeCache.Add(name, value);
            }

		}
        /// <summary>
        /// ���ط��������ƶ˿�_�ļ�_·��
        /// </summary>
        /// <returns></returns>
        public static string GetCookiePart()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            return context.Request.ServerVariables["SERVER_NAME"] + "_" +
                context.Request.ServerVariables["SERVER_PORT"] + "_" +
                context.Request.ApplicationPath;
        }
        /// <summary>
        /// ����cookie[varName]��ֵ���������򷵻�""
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
		public static string GetCookie(string varName)
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
            varName = GetCookiePart() + varName;
			System.Web.HttpCookie cookie = context.Request.Cookies[varName];
			if (cookie != null)
				//return System.Web.HttpContext.Current.Server.UrlDecode(cookie.Value);
                return DESEncrypt.Decrypt(cookie.Value);
			else
				return "";
		}
        /// <summary>
        /// ����cookie��Ĭ��5Сʱ
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
		public static  void SetCookie(string varName, string varValue)
		{
			SetCookie(varName, varValue, DateTime.Now.AddHours(5));
		}
        /// <summary>
        /// ���ñ���cookie
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
		public static  void SetCookieForever(string varName, string varValue)
		{
			SetCookie(varName, varValue, DateTime.Parse("2099-12-12 12:12:12"));
		}
        /// <summary>
        /// ����cookie[varName]=varValue,����ʱ��dtExpires
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
        /// <param name="dtExpires"></param>
		public static void SetCookie(string varName, string varValue, DateTime dtExpires)
		{
            varValue = DESEncrypt.Encrypt(varValue);
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            varName = GetCookiePart() + varName;
            string val = varValue.ToString();
            HttpCookie cookie = new System.Web.HttpCookie(varName, val);
            //cookie.Domain = SiteAbout.GetHttpServer() ;
            cookie.Expires = dtExpires;
            System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
		}
        /// <summary>
        /// ɾ��cookie[varName]
        /// </summary>
        /// <param name="varName"></param>
        public static void DeleteCookie(string varName)
        {
            SetCookie(varName, "",new DateTime(1900,1,1));
        }
		#endregion


		#region GetUrl
		/// <summary>
		/// �õ���ǰ��ַ�����
		/// </summary>
		/// <returns></returns>
		public static string GetUrl()
		{
            string url = "";
            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                url=context.Request.ServerVariables["URL"] + "?" + context.Request.ServerVariables["QUERY_STRING"] ;
                //RuntimeCache.Add("URL", url);
            }
            catch (System.Exception ex)
            {
                //url = RuntimeCache.Get<string>("URL");
            }
            return url;
		}

		/// <summary>
		/// ����ǰ����ת����һ����ҳ
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string GetUrl(string url)
		{
			return  url + "?" +  System.Web.HttpContext.Current.Request.ServerVariables["QUERY_STRING"] ;
		}
		/// <summary>
		/// ����ǰ��ַĳ�������ı�,�� ��page=1 ��Ϊpage=2
		/// </summary>
		/// <param name="replaceParamName"></param>
		/// <param name="paramValue"></param>
		/// <returns></returns>
		public static string GetUrl(string replaceParamName,object paramValue)
		{
			return GetUrl(GetUrl(),replaceParamName,paramValue) ;
		}

		/// <summary>
		/// ����ǰ��ַĳ�������ı�,�� ��page=1 ��Ϊpage=2
		/// </summary>
		/// <param name="url"></param>
		/// <param name="replaceParamName"></param>
		/// <param name="paramValue"></param>
		/// <returns></returns>
		public static string GetUrl(string url,string replaceParamName,object paramValue)
		{
			string oldParamValue = System.Web.HttpContext.Current.Request[replaceParamName] ;
			string oldParam = replaceParamName + "=" + oldParamValue ;
			if ((oldParamValue == null) || (url.IndexOf(oldParam) == -1)) //����˲���,ֱ�Ӽ���
				url = GetUrlByAdd(url,replaceParamName + "=" + paramValue.ToString()) ;
			else
				url = url.Replace(oldParam,replaceParamName + "=" + paramValue.ToString()) ;
			return url ;
		}

		public static string GetUrlByMove(string url,string moveParamName)
		{
            url = url.ToLower();
            moveParamName = moveParamName.ToLower();
            if (url.IndexOf(moveParamName) == -1)
                return url;

			string paramValue = System.Web.HttpContext.Current.Request.QueryString[moveParamName];
            if (paramValue == null) //����������
                paramValue = "";
            else
                paramValue = paramValue.ToLower();

            string moveStr = moveParamName + "=" + paramValue;
            url = url.Replace(moveStr, "");
            url = url.Replace("&&", "&");
            url = url.Replace("?&", "?");
            if ((url.Substring(url.Length - 1) == "?") || (url.Substring(url.Length - 1) == "&"))
                url = url.Substring(0, url.Length - 1);
            return url;

		}
		public static string GetUrlByAdd(string url,string pram)
		{
			if (url.IndexOf("?") == -1)
				return url + "?" + pram ;
			else
				return url + "&" + pram ;
		}
		#endregion


		/// <summary>
		/// ������Ϣ
		/// </summary>
		/// <param name="msgType">��Ϣ����  1��ʾ  2����  3����</param>
		/// <param name="msgContent">��Ϣ����</param>
		public static void ReportMsg(int msgType, string msgContent)
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current ;
			context.Response.Clear() ;
			context.Response.Write("<LINK href='../css/style.css' type='text/css' rel='stylesheet'>") ;
			context.Response.Write("<div id='SysMsg'>" + msgContent + "<a href='#' onclick='history.back(-1)'>�����ء�</a></div>") ;
			context.Response.End();
		}

        /// <summary>
        /// �Ƿ񱣴�cookie
        /// </summary>
		public static bool IsSaveCookie
		{
			get
			{
                //string val = System.Configuration.ConfigurationManager.AppSettings["IsCookie"];
                string val = ConfigHelper.GetConfigString("IsCookie");
                if (val == "1")
                    return true;
                else
                    return false;
			}

		}

		/// <summary>
		/// Ȩ�ޱ� FunName UserId Operate
		/// </summary>
		public static DataTable Power
		{
			get
			{
				return (DataTable)SiteAbout.GetAppValue("Power") ;
			}
			set
			{
				SiteAbout.SetAppValue("Power",value) ;
			}
		}
        /// <summary>
        /// ��ȡ�ͻ���IP
        /// </summary>
        /// <returns></returns>
		private  string GetClientIP()
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current ;

			object o = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ;
			if (o==null)
				return context.Request.ServerVariables["Remote_Addr"] ;
			else
				return o.ToString() ;
		}
        /// <summary>
        /// ��ȡ��������Ϣ�������������˿ںţ�
        /// </summary>
        /// <returns></returns>


		#region ��֤��
        /// <summary>
        /// ������֤��
        /// </summary>
        /// <returns></returns>
		public static string GenerateCheckCode()
		{
			int number;
			char code;
			string checkCode = String.Empty;

			System.Random random = new Random();

			for(int i=0; i<5; i++)
			{
				number = random.Next();

				if(number % 2 == 0)
					code = (char)('0' + (char)(number % 10));
				else
					code = (char)('A' + (char)(number % 26));

				checkCode += code.ToString();
			}

			CheckCode = checkCode ;

			return checkCode;
		}

		public static string CheckCode
		{
			get{return SiteAbout.GetCookie("CheckCode");}
			set{SiteAbout.SetCookie("CheckCode",value);}

		}
        /// <summary>
        /// ������֤��ͼƬ
        /// </summary>
        /// <param name="checkCode"></param>
		public static void CreateCheckCodeImage(string checkCode)
		{
			if(checkCode == null || checkCode.Trim() == String.Empty)
				return;

			System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
			Graphics g = Graphics.FromImage(image);

			try
			{
				//�������������
				Random random = new Random();

				//���ͼƬ����ɫ
				g.Clear(Color.White);

				//��ͼƬ�ı���������
				for(int i=0; i<25; i++)
				{
					int x1 = random.Next(image.Width);
					int x2 = random.Next(image.Width);
					int y1 = random.Next(image.Height);
					int y2 = random.Next(image.Height);

					g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
				}

				Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
				System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
				g.DrawString(checkCode, font, brush, 2, 2);

				//��ͼƬ��ǰ��������
				for(int i=0; i<100; i++)
				{
					int x = random.Next(image.Width);
					int y = random.Next(image.Height);

					image.SetPixel(x, y, Color.FromArgb(random.Next()));
				}

				//��ͼƬ�ı߿���
				g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
				System.Web.HttpContext.Current.Response.ClearContent();
				System.Web.HttpContext.Current.Response.ContentType = "image/Gif";
				System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
			}

			finally
			{
				g.Dispose();
				image.Dispose();
			}
		}
		#endregion
	}
}
