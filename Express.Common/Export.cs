using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Express.Common
{
    public static class Export
    {
        //public static string GetGridTableHtml(Grid grid)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

        //    sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

        //    sb.Append("<tr>");
        //    foreach (GridColumn column in grid.Columns)
        //    {
        //        sb.AppendFormat("<td>{0}</td>", column.HeaderText);
        //    }
        //    sb.Append("</tr>");


        //    foreach (GridRow row in grid.Rows)
        //    {
        //        sb.Append("<tr>");
        //        foreach (object value in row.Values)
        //        {
        //            string html = value.ToString();
        //            // 处理CheckBox
        //            if (html.Contains("f-grid-static-checkbox"))
        //            {
        //                if (html.Contains("uncheck"))
        //                {
        //                    html = "×";
        //                }
        //                else
        //                {
        //                    html = "√";
        //                }
        //            }

        //            // 处理图片
        //            if (html.Contains("<img"))
        //            {
        //                string prefix = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "");
        //                html = html.Replace("src=\"", "src=\"" + prefix);
        //            }

        //            sb.AppendFormat("<td>{0}</td>", html);
        //        }
        //        sb.Append("</tr>");
        //    }

        //    sb.Append("</table>");

        //    return sb.ToString();
        //}
        public static string GetTabletml(DataTable grid)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            sb.Append("<tr>");
            foreach (DataColumn column in grid.Columns)
            {
                sb.AppendFormat("<td>{0}</td>", column.ColumnName);
            }
            sb.Append("</tr>");


            foreach (DataRow row in grid.Rows)
            {
                sb.Append("<tr>");
                for (int i = 0; i < grid.Columns.Count; i++)
                {


                    string html = row[i].ToString();
                    // 处理CheckBox
                    if (html.Contains("f-grid-static-checkbox"))
                    {
                        if (html.Contains("uncheck"))
                        {
                            html = "×";
                        }
                        else
                        {
                            html = "√";
                        }
                    }

                    // 处理图片
                    if (html.Contains("<img"))
                    {
                        string prefix = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "");
                        html = html.Replace("src=\"", "src=\"" + prefix);
                    }

                    sb.AppendFormat("<td>{0}</td>", html);
                }
                sb.Append("</tr>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }

        /// <summary>
        /// 获取控件渲染后的HTML源代码
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string GetRenderedHtmlSource(Control ctrl)
        {
            if (ctrl != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        ctrl.RenderControl(htw);

                        return sw.ToString();
                    }
                }
            }
            return String.Empty;
        }


    }
}
