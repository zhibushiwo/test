using Express.Common.SiteOperate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataAnalysis.Controllers
{
    public class LoginController : Controller
    {
        private WAES.BLL.Gy_User _bllUser = new WAES.BLL.Gy_User();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CheckUser()
        {
            string userName = SiteAbout.gStr("userName");
            string password = SiteAbout.gStr("password");
            WAES.Model.Gy_User model = _bllUser.GetModel("HNumber='" + userName + "'");
            List<WAES.Model.Gy_User> models = _bllUser.GetModelList("HNumber='" + userName + "'");
            if (models.Count == 1)
            {
                string czymm = Express.Common.DEncrypt.DEncrypt.Encrypt(password);
                if (models[0].HPassword.Trim() == czymm.Trim())
                {
                    CurUser.LoginName = models[0].HNumber;
                    CurUser.Name = models[0].HName;
                    CurUser.Id = models[0].HInterID;
                    return Json(new { status = true, info = "登陆成功" });
                }
            }
            return Json(new { status = false, info = "登陆失败" });
        }
    }
}