using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;

using WAES.Model;
namespace WAES.BLL {
	 	//Gy_User
		public partial class Gy_User
	{
   		     
		private readonly WAES.DAL.Gy_User dal=new WAES.DAL.Gy_User();
		public Gy_User()
		{}
		
		#region  Method
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int HInterID)
		{
			return dal.Exists(HInterID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(WAES.Model.Gy_User model)
		{
						return dal.Add(model);
						
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(WAES.Model.Gy_User model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int HInterID)
		{
			
			return dal.Delete(HInterID);
		}
				/// <summary>
		/// 批量删除一批数据
		/// </summary>
		public bool DeleteList(string HInterIDlist )
		{
			return dal.DeleteList(HInterIDlist );
		}
		
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public WAES.Model.Gy_User GetModel(int HInterID)
		{
			
			return dal.GetModelTop1("HInterID="+HInterID);
		}
        public WAES.Model.Gy_User GetModel(string strWhere)
        {

            return dal.GetModelTop1(strWhere);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public WAES.Model.Gy_User GetModelByCache(int HInterID)
		{
			
			string CacheKey = "Gy_UserModel-" + HInterID;
			object objModel = Express.Common.WebCache.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModelTop1("HInterID="+HInterID);
					if (objModel != null)
					{
						int ModelCache = Express.Common.FileOperate.ConfigHelper.GetConfigInt("ModelCache");
						Express.Common.WebCache.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (WAES.Model.Gy_User)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<WAES.Model.Gy_User> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<WAES.Model.Gy_User> DataTableToList(DataTable dt)
		{
			List<WAES.Model.Gy_User> modelList = new List<WAES.Model.Gy_User>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				WAES.Model.Gy_User model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);		
				
					modelList.Add(model);
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
#endregion
   
	}
}