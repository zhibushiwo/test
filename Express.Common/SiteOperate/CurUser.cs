using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;

namespace Express.Common.SiteOperate
{

	/// <summary>
	/// Visiter ��ժҪ˵����
	/// </summary>
	public class CurUser
	{
        public static bool _isSaveCookie = true;
        public static bool IsSaveCookie
        {
            get
            {
                return _isSaveCookie;
            }
            set
            {
                _isSaveCookie = value;
            }
        }
        /// <summary>
        /// �û�ID
        /// </summary>
		public static int Id
		{
			get
			{
				string val = SiteAbout.GetValue("UserId") ;
				return val == ""?0: int.Parse(val);
			}
			set
			{

				SiteAbout.SetValue("UserId",value.ToString()) ;
			}
		}
        /// <summary>
        /// ��¼��
        /// </summary>
		public static  string LoginName
		{
			get
			{
				return SiteAbout.GetValue("LoginName") ;
			}
			set
			{
				SiteAbout.SetValue("LoginName",value) ;
			}
		}

        /// <summary>
        /// �û��˺�
        /// </summary>
        public static string UserName
        {
            get
            {
                return SiteAbout.GetValue("UserName");
            }
            set
            {
                SiteAbout.SetValue("UserName", value);
            }
        }
        /// <summary>
        /// ����
        /// </summary>
		public static  string Password
		{
			get
			{
				return SiteAbout.GetValue("Password") ;
			}
			set
			{
				SiteAbout.SetValue("Password",value) ;
			}
		}

        //�ɱ�����ID
        public static string HCCID
        {
            get
            {
                return SiteAbout.GetValue("HCCID");
            }
            set
            {
                SiteAbout.SetValue("HCCID", value);
            }
        }
        //�ɱ���������
        public static string CostCenter
        {
            get
            {
                return SiteAbout.GetValue("CostCenter");
            }
            set
            {
                SiteAbout.SetValue("CostCenter", value);
            }
        }

        //ְλID
        public static string JobID
        {
            get
            {
                return SiteAbout.GetValue("JobID");
            }
            set
            {
                SiteAbout.SetValue("JobID", value);
            }
        }
        /// <summary>
        /// �û���
        /// </summary>
		public static  string Name
		{
			get
			{
				return SiteAbout.GetValue("UserName") ;
			}
			set
			{

				SiteAbout.SetValue("UserName",value) ;
			}
		}

		/// <summary>
		/// �����û�
		/// </summary>
		public static  string PowerTree
		{
			get
			{
				return SiteAbout.GetValue("PowerTree") ;
			}
			set
			{
				SiteAbout.SetValue("PowerTree",value) ;
			}
		}

		/// <summary>
		/// �����û����Լ�
		/// </summary>
		public static  string PowerTreeAndMy
		{
			get
			{
				return SiteAbout.GetValue("PowerTreeAndMy") ;
			}
			set
			{
				SiteAbout.SetValue("PowerTreeAndMy",value) ;
			}
		}

		public static  string PowerTreeName
		{
			get
			{
				return SiteAbout.GetValue("PowerTreeName") ;
			}
			set
			{
				SiteAbout.SetValue("PowerTreeName",value) ;
			}
		}
        /// <summary>
        /// ��ɫ
        /// </summary>
		public static string Role
		{
			get
			{
				return SiteAbout.GetValue("Role") ;
			}
			set
			{
				SiteAbout.SetValue("Role",value) ;
			}
		}

        public static string Lines
        {
            get
            {
                return SiteAbout.GetValue("Lines");
            }
            set
            {
                SiteAbout.SetValue("Lines", value);
            }
        }
        public static int DeptId
		{
			get
			{
				string val = SiteAbout.GetValue("DeptId");
				if (val == "")
					return 0 ;
				else
					return int.Parse(val) ;
			}
			set
			{
				SiteAbout.SetValue("DeptId", value.ToString());
			}
		}
        //����ID
        public static int HEntityID
        {
            get
            {
                string val = SiteAbout.GetValue("HEntityID");
                if (val == "")
                    return 0;
                else
                    return int.Parse(val);
            }
            set
            {
                SiteAbout.SetValue("HEntityID", value.ToString());
            }
        }

        public static int HLevel
        {
            get
            {
                string val = SiteAbout.GetValue("HLevel");
                if ((val == null) || (val.ToString() == ""))
                    return 0;
                else
                    return int.Parse(val);
            }
            set
            {
                SiteAbout.SetValue("HLevel", value.ToString());
            }
        }
        public static string DeptName
		{
			get
			{
				return SiteAbout.GetValue("DeptName");
			}
			set
			{
				SiteAbout.SetValue("DeptName", value);
			}
		}

		public static string Company
		{
			get
			{
				return SiteAbout.GetValue("Company");
			}
			set
			{
				SiteAbout.SetValue("Company", value);
			}
		}

		public static string Note
		{
			get
			{
				return SiteAbout.GetValue("Note");
			}
			set
			{
				SiteAbout.SetValue("Note", value);
			}
		}

        public static string Lang
        {
            get
            {
                return SiteAbout.GetValue("Lang");
            }
            set
            {
                SiteAbout.SetValueForever("Lang", value);
            }
        }
        /// <summary>
        /// �Ƿ񳬼�����Ա
        /// </summary>
		public static bool IsSuper
		{
			get
			{
				object o =  SiteAbout.GetValue("IsSuper");
				if ((o == null)||(o.ToString() == ""))
					return false ;
				else
					return bool.Parse(o.ToString());
			}
			set
			{
				SiteAbout.SetValue("IsSuper", value.ToString());
			}
		}


        public static int CompanyId
        {
            get
            {
                string val = SiteAbout.GetValue("CompanyId");
                return val == "" ? 0 : int.Parse(val);
            }
            set
            {

                SiteAbout.SetValue("CompanyId", value.ToString());
            }
        }
        //public static WAES.NPPMS.Model.BaseSet.HR_Personnel model
        //{
        //    get
        //    {
        //        string val = SiteAbout.GetValue("Model");
        //        return PackReflectionEntity<WAES.NPPMS.Model.BaseSet.HR_Personnel>.GetEntityStringToEntity(val);
        //        //return DataDeserialize(val);
        //    }
        //    set
        //    {
        //        //string val = DataSerialize(value);
        //        string val = PackReflectionEntity<WAES.NPPMS.Model.BaseSet.HR_Personnel>.GetEntityToString(value);
        //        SiteAbout.SetValue("Model", val.ToString());
        //    }
        //}
        public static string DataSerialize(object myList)
        {
            StringWriter sw = new StringWriter();
            XmlSerializer s = new XmlSerializer(myList.GetType());
            s.Serialize(sw, myList);
            return sw.ToString();
        }

        public static object DataDeserialize(string data)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<int>));
            object newList = (object)xs.Deserialize(new StringReader(data));
            return newList;
        }
        #region LogOut ע��
        /// <summary>
        /// ע��
        /// </summary>
        public static void LogOut()
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current ;
			context.Session.Abandon() ;
            SiteAbout.DeleteCookie("UserId");
            SiteAbout.DeleteCookie("LoginName");
            SiteAbout.DeleteCookie("UserName");
            SiteAbout.DeleteCookie("Password");
            SiteAbout.DeleteCookie("IsSuper");
		}
		#endregion

		/// <summary>
		/// �ܷ����
		/// </summary>
		/// <param name="funName">��������</param>
		/// <param name="operate">���� 1�� 1���� 3���� 15����</param>
		/// <returns></returns>
		public bool CanOperate(string funName,int operate)
		{
			DataView dv = new DataView(SiteAbout.Power) ;
			dv.RowFilter = "FunName='" + funName + "' and UserId='" + CurUser.Id + "'" ;
			foreach(DataRowView  item in dv)
			{
				if (((int)item["Operate"] & operate) == operate)
					return true ;
			}
			return false ;
		}

        public static string Procedure
        {
            get
            {
                string val = SiteAbout.GetValue("Procedure");
                return val;
            }
            set
            {
                SiteAbout.SetValue("Procedure", value.ToString());
            }
        }

    }
}
