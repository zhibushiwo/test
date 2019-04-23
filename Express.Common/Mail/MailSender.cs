using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Express.Common.Mail
{
    public class MailSender
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailAddress">发件地址</param>
        /// <param name="strdisplayname">显示名字</param>
        /// <param name="mailServer">SMTP服务器</param>
        /// <param name="mailLoginName">登录名</param>
        /// <param name="mailPassword">密码</param>
        /// <param name="recvEmail">收件人</param>
        /// <param name="ccEmail">抄送给</param>
        /// <param name="subject">主题</param>
        /// <param name="content">内容</param>
        /// <param name="attach">附件，用{and}连接</param>
        /// <returns></returns>
        public static int SendMail2(string mailAddress, string strdisplayname, string mailServer, string mailLoginName, string mailPassword,
        string recvEmail, string ccEmail, string subject, string content, string attach)
        {
            ////构建MailMessage对象
            MailAddress from = new MailAddress(mailAddress, strdisplayname, System.Text.Encoding.UTF8);
            //MailAddress to = new MailAddress(recvEmail);
            System.Net.Mail.MailMessage mailobj = new System.Net.Mail.MailMessage();
            //完善MailMessage对象
            mailobj.From = from;
            if (recvEmail != "")
            {
                string[] arrrecvEmail = Regex.Split(recvEmail, ";", RegexOptions.IgnoreCase);
                for (int i = 0; i < arrrecvEmail.Length; i++)
                {
                    if (!string.IsNullOrEmpty(arrrecvEmail[i].Replace(" ", "")))
                        mailobj.To.Add(new MailAddress(arrrecvEmail[i]));
                }
            }
            if (ccEmail != "")
            {
                string[] arrccEmail = Regex.Split(ccEmail, ";", RegexOptions.IgnoreCase);
                for (int i = 0; i < arrccEmail.Length; i++)
                {
                    mailobj.CC.Add(new MailAddress(arrccEmail[i]));
                }
            }
            mailobj.SubjectEncoding = System.Text.Encoding.UTF8;
            mailobj.Subject = subject;
            /*20120711 修改了Body的内容 */
            mailobj.Body = content;
            mailobj.IsBodyHtml = true;
            mailobj.BodyEncoding = System.Text.Encoding.UTF8;// System.Text.Encoding.GetEncoding("GB2312");
            //mailobj.IsBodyHtml = true;
            //AlternateView BodyBase64 = AlternateView.CreateAlternateViewFromString(content, System.Text.Encoding.UTF8, "text/plain");
            //BodyBase64.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            //mailobj.AlternateViews.Add(BodyBase64);
            //mailobj.BodyEncoding = System.Text.Encoding.UTF8;

            mailobj.Priority = System.Net.Mail.MailPriority.High;
            //if (FileUpload1.PostedFile.ContentLength > 0)
            if (attach != "")
            {
                string[] arrAttach = Regex.Split(attach, "{AND}", RegexOptions.IgnoreCase);
                Attachment data;
                System.Net.Mime.ContentDisposition disposition;
                for (int i = 0; i < arrAttach.Length; i++)
                {
                    //mailobj.Attachments.Add(new Attachment(arrAttach[i]));
                    data = new Attachment(arrAttach[i], System.Net.Mime.MediaTypeNames.Application.Octet);
                    disposition = data.ContentDisposition;
                    //disposition.CreationDate = System.IO.File.GetCreationTime(arrAttach[i]);
                    //disposition.ModificationDate = System.IO.File.GetLastWriteTime(arrAttach[i]); // 获取附件的修改日期
                    //disposition.ReadDate = System.IO.File.GetLastAccessTime(arrAttach[i]); //获取附件的读取日期
                    data.NameEncoding = System.Text.Encoding.UTF8;
                    //data.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                    mailobj.Attachments.Add(data);
                }
            }


            //构建SmtpClient对象
            SmtpClient smtp = new SmtpClient(mailServer);   // smtp.Host = "smtp.163.com";
            if (mailServer.Substring(0, 4) == "mail") //设置非Smtp开头的邮件服务器的验证方式
            {
                FieldInfo transport = smtp.GetType().GetField("transport", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo authModules = transport.GetValue(smtp).GetType().GetField("authenticationModules", BindingFlags.NonPublic | BindingFlags.Instance);
                Array modulesArray = authModules.GetValue(transport.GetValue(smtp)) as Array;
                modulesArray.SetValue(modulesArray.GetValue(3), 0);
                modulesArray.SetValue(modulesArray.GetValue(3), 1);
                modulesArray.SetValue(modulesArray.GetValue(3), 2);
            }
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(mailLoginName, mailPassword);//账号密码自己填
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Timeout = 20 * 60 * 1000;

            try
            {
                //smtp.Send(mailobj);
                object objmail = mailobj;
                smtp.SendCompleted += new SendCompletedEventHandler(SendCompleted);
                smtp.SendAsync(mailobj, mailobj);
                return 1;
            }
            catch (Exception e)
            {
                //DocViewer.Common.SysDefine.ErrorCollection.Add(e.Message);
                return 0;
            }
            finally
            {
                //mailobj.Dispose();
            }

        }

        public static void SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //异步回调函数
            System.Net.Mail.MailMessage token = (System.Net.Mail.MailMessage)e.UserState;
            if (e.Error != null)
            {
                //DocViewer.Common.SysDefine.ErrorCollection.Add(e.Error.ToString());
            }
            token.Dispose();
        } 
    }
}
