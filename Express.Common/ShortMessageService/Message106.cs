using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Express.Common.ShortMessageService
{
    public partial class Message106
    {
        //private string _userid { get; set; }
        private string _account { get; set; }
        private string _password { get; set; }
        public Message106(string account,string password)
        {
            _account = account;
            _password = password;
            
        }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="receiveMobile">接收手机号用,分割</param>
        /// <param name="SMSContent">短信内容</param>
        /// <returns></returns>
        public string SendMessage(string receiveMobile,string SMSContent)
        {
            string sendurl = "http://api.sms.cn/mt/";
            string mobile = receiveMobile;  //发送号码
            string strContent = SMSContent;
            StringBuilder sbTemp = new StringBuilder();
            string uid = _account;
            string pwd = _password;
            string Pass = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd + uid, "MD5"); //密码进行MD5加密
                                                                                                    //POST 传值
            sbTemp.Append("uid=" + uid + "&pwd=" + Pass + "&mobile=" + mobile + "&content=" + strContent);
            byte[] bTemp = System.Text.Encoding.GetEncoding("GBK").GetBytes(sbTemp.ToString());
            String postReturn = doPostRequest(sendurl, bTemp);
            return postReturn;
            //Response.Write("Post response is: " + postReturn);  //测试返回结果
        }
        /// <summary>
        /// 查询余额
        /// </summary>
        /// <returns></returns>
        public string GetOverage()
        {
            string Pass = FormsAuthentication.HashPasswordForStoringInConfigFile(_password + _account, "MD5"); //密码进行MD5加密
            string param = "uid="+_account+"&pwd="+ Pass;
            byte[] bs = Encoding.UTF8.GetBytes(param);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://api.sms.cn/mm/");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            using (WebResponse wr = req.GetResponse())
            {
                StreamReader sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.Default);
                return sr.ReadToEnd().Trim();
            }
        }

        //POST方式发送得结果
        private static String doPostRequest(string url, byte[] bData)
        {
            System.Net.HttpWebRequest hwRequest;
            System.Net.HttpWebResponse hwResponse;

            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                hwRequest.Timeout = 5000;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;

                System.IO.Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch (System.Exception err)
            {
                WriteErrLog(err.ToString());
                return strResult;
            }

            //get response
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (System.Exception err)
            {
                WriteErrLog(err.ToString());
            }
            return strResult;
        }
        private static void WriteErrLog(string strErr)
        {
            Console.WriteLine(strErr);
            System.Diagnostics.Trace.WriteLine(strErr);
        }
    }
}
