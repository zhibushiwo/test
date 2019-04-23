using System;
using System.Data ;
namespace Express.Common.SiteOperate
{
	/// <summary>
	/// FDataTable 的摘要说明。
	/// </summary>
	public class FDataTable
	{
		public static float TotleColumn(DataTable dt,string columnName)
		{
			if (dt == null)
				return 0 ;
			float val = 0 ;
			foreach(DataRow dr in dt.Rows)
			{

				string a = dr[columnName].ToString() ;
				val = val + float.Parse(dr[columnName].ToString());
			}
			return val ;
		}
         /// <summary>
        /// 获取dt中columnName的值，以“,”连接(空值将被忽略)
         /// </summary>
         /// <param name="dt"></param>
         /// <param name="columnName"></param>
         /// <returns></returns>
		public static string GetColumnValue(DataTable dt,string columnName)
		{
			if (dt == null)
				return "" ;
			string val = "" ;
			foreach(DataRow dr in dt.Rows)
			{
                if (dr[columnName].ToString() != "")
                {
                    val = val + "," + dr[columnName].ToString().Trim(',');
                }
			}
			if (val != "")
				val = val.Trim(',');
			return val ;
		}
        /// <summary>
        /// 获取dt中columnName的值，以“,”连接(空值将被忽略)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string GetColumnValue(DataTable dt, string columnName, string nullValue)
        {
            if (dt == null)
                return "";
            string val = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[columnName].ToString() != "")
                {
                    val = val + "," + dr[columnName].ToString().Trim(',');
                }
                else
                {
                    val = val + "," + nullValue.Trim(',');
                }
            }
            if (val != "")
                val = val.Trim(',');
            return val;
        }
        public static string FindValue(DataTable dt,string findFldName,string findValue,string returnFldName)
		{
			if (dt == null)
				return "" ;
			object obj ;
			foreach(DataRow dr in dt.Rows)
			{
				if (dr[findFldName].ToString() == findValue)
				{
					obj = dr[returnFldName];
					return obj == null?"":obj.ToString() ;
				}
			}
			return "" ;
		}
		/// <summary>
		/// 得到Excel的字符串
		/// </summary>
		/// <param name="dataControl"></param>
		/// <returns></returns>
		public static string GetExcelExp(System.Data.DataTable dt)
		{
			System.Text.StringBuilder str = new System.Text.StringBuilder();
			int rowCount = dt.Rows.Count ;
			int columnCount = dt.Columns.Count ;
			string val = "" ;

			foreach (DataRow dr in dt.Rows)
			{
				for(int i=0;i<columnCount;i++)
				{
					val = dr[i].ToString() ;
					if (val  == "&nbsp;")
						val = "" ;
					if (i < columnCount)
						str.Append(val + "\t");
					else
						str.Append(val + "\n");
				}
			}
			return str.ToString() ;
		}


		/// <summary>
		/// 得到共几页
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public static int GetTableCount(DataTable dt,int pageSize)
		{
			int pageCount = 0 ;
			int recordCount = dt.Rows.Count ; //去掉头行，头行是表头
			if (recordCount>0)
			{
				pageCount = recordCount / pageSize ; // 如 pageSize = 10 ,recordCount=40则4页 41页5页
				if ((recordCount%pageSize)>0)
					pageCount = pageCount + 1 ;
			}
			return pageCount ;
		}

		/// <summary>
		/// 得到每页数据
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <returns></returns>
		public static DataTable GetTable(DataTable dt ,int pageSize,int pageIndex)
		{

			DataTable dt_Curr = dt.Clone() ;
			int rowBegin = pageIndex * pageSize;
			int rowEnd = rowBegin + pageSize ;
			if (rowEnd > dt.Rows.Count)
				rowEnd = dt.Rows.Count ;

			for(int i=rowBegin ;i<rowEnd;i++)
				dt_Curr.Rows.Add(dt.Rows[i].ItemArray);
			return dt_Curr ;
		}

		/// <summary>
		/// 分隔双层表格
		/// </summary>
		/// <param name="dtParnet">dtParent.Rows.Count = dtSubs.Length</param>
		/// <param name="dtSubs"></param>
		/// <param name="pageSize"></param>
		public static void GetBothTables(DataTable dtParent,DataTable[] dtSubs,int pageSize,out DataTable nowDtParent,out System.Collections.ArrayList arrDtSub)
		{
			nowDtParent = dtParent.Clone() ;
			nowDtParent.Columns.Add(new DataColumn("TableIndex",System.Type.GetType("System.Int32"))) ; //在Parent表中的Index
			nowDtParent.Columns.Add(new DataColumn("SubTableCount",System.Type.GetType("System.Int32"))) ;//共有Sub表个数
			nowDtParent.Columns.Add(new DataColumn("SubTableIndex",System.Type.GetType("System.Int32"))) ;//当前Sub表的Index
			arrDtSub = new System.Collections.ArrayList() ;
			for(int i=0;i<dtSubs.Length;i++)
			{
				System.Collections.ArrayList arrTables = DivideSingleTables(dtSubs[i],pageSize,true) ;
				arrDtSub.AddRange(arrTables) ;
				for(int j=0;j<arrTables.Count ;j++)
				{
					nowDtParent.Rows.Add(dtParent.Rows[i].ItemArray);
					int lastRowIndex = nowDtParent.Rows.Count - 1 ;
					nowDtParent.Rows[lastRowIndex]["TableIndex"] = i+1 ;
					nowDtParent.Rows[lastRowIndex]["SubTableCount"] = arrTables.Count ;
					nowDtParent.Rows[lastRowIndex]["SubTableIndex"] = j+1 ;
				}
			}

		}

		/// <summary>
		/// 分隔单一表格（根据页行数，分隔成若干个表格）
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="pageSize"></param>
		/// <param name="isFillBlank"></param>
		/// <returns></returns>
		public static  System.Collections.ArrayList DivideSingleTables(DataTable dt,int pageSize,bool isFillBlank)
		{
			int count = FDataTable.GetTableCount(dt,pageSize) ;
			System.Collections.ArrayList arrTable = new System.Collections.ArrayList() ;
			for(int i=0 ;i<count;i++)
			{
				DataTable dtCurr = FDataTable.GetTable(dt,pageSize,i) ;
				if (isFillBlank)
					dtCurr = FillBlank(dtCurr,pageSize) ;
				arrTable.Add(dtCurr) ;
			}
			return arrTable ;
		}

		/// <summary>
		/// 用空行填充表格
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public static DataTable FillBlank(DataTable dt,int pageSize)
		{
			if (dt.Rows.Count >= pageSize)
				return dt ;

			int columnCount = dt.Columns.Count ;
			for(int i= dt.Rows.Count ;i<pageSize;i++)
			{
				DataRow dr = dt.NewRow() ;
				for(int j=0 ; j<columnCount ; j++)
					dr[j] = System.DBNull.Value  ;
				dt.Rows.Add(dr) ;
			}
			return dt ;
		}
        /// <summary>
        /// 根据关键字删除dt里的列
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public static DataTable RemoveColBySearch(DataTable dt,string keyWord)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.IndexOf(keyWord) >0)
                {
                    dt.Columns.RemoveAt(i);
                }
            }
            return dt;
        }


        /// <summary>
        /// 比较两个DataTable数据（结构相同,字段名不同）
        /// </summary>
        /// <param name="dtDest">来自数据库的DataTable</param>
        /// <param name="dtSrc">来自文件的DataTable</param>
        /// <param name="dtRetAdd">相同的数据</param>
        /// <param name="dtRetAdd">新增数据（dt2中的数据）</param>
        /// <param name="dtRetDel">删除的数据（dt2中的数据）</param>
        /// <param name="srcKeyFields">源关键字段名</param>
        /// <param name="destKeyFields">目标关键字段名，与源关键字段名对应</param>
        public static void CompareDt(DataTable dtSrc, DataTable dtDest, out DataTable dtSame, out DataTable dtRetAdd, out DataTable dtRetDel, string srcKeyFields, string destKeyFields)
        {
            //源记录集与目标记录集有一个为null则退出
            if (dtSrc == null || dtDest == null)
            {
                dtRetDel = null;
                dtRetAdd = null;
                dtSame = null;
                return;
            }
            //定义返回记录表
            dtRetDel = dtSrc.Clone();
            dtRetAdd = dtRetDel.Clone();
            dtSame = dtSrc.Clone();
            //参照列为空则退出
            if (string.IsNullOrEmpty(srcKeyFields) || string.IsNullOrEmpty(destKeyFields))
                return;
            //获得参照列列表
            string[] srcFields = srcKeyFields.Split(',');//列名数组
            string[] destFields = destKeyFields.Split(',');//列名数组
            //参照列数目不一致则退出
            if (srcFields.Length != destFields.Length)
                return;
            //按参照列排序源表和目标表
            DataRow[] drSrc = dtSrc.Select("", srcKeyFields);
            DataRow[] drDest = dtDest.Select("", destKeyFields);
            //定义源表和目标表长度
            int iSrcCount = drSrc.Length;
            int iDestCount = drDest.Length;
            //源表为空则目标表全部加入删除队列并返回
            if (iSrcCount == 0)
            {
                foreach (DataRow row in drDest)
                {
                    dtRetDel.Rows.Add(row.ItemArray);
                }
                return;
            }
            //目标表为空则源表全部加入新增队列并返回
            if (iDestCount == 0)
            {
                foreach (DataRow row in drSrc)
                {
                    dtRetAdd.Rows.Add(row.ItemArray);
                }
                return;
            }
            //定义源表和目标表指针
            int iSrc = 0;
            int iDest = 0;
            //开始循环比对
            while (iSrc < iSrcCount && iDest < iDestCount)
            {
                //定义列比对结果
                int result = 0;
                object oSrc;
                object oDest;
                //循环比对列值
                for (int colIndex = 0; colIndex < srcFields.Length; colIndex++)
                {
                    //获得列值
                    oSrc = drSrc[iSrc][srcFields[colIndex]];
                    oDest = drDest[iDest][destFields[colIndex]];
                    //比较列值，不相等则退出循环
                    if (oSrc == DBNull.Value)
                    {
                        result = oDest == DBNull.Value ? 0 : -1;
                    }
                    else
                    {
                        result = oDest == DBNull.Value ? 1 : string.Compare(oSrc.ToString(), oDest.ToString(), false);
                    }
                    if (result != 0)
                        break;
                }
                //检查行比较结果
                switch (result)
                {
                    ////源表小则将源表本行加入新增队列，同时移动源表指针
                    case -1:
                        dtRetAdd.Rows.Add(drSrc[iSrc].ItemArray);
                        iSrc++;
                        break;
                    ////相同两表同时移动指针
                    case 0:
                        dtSame.Rows.Add(drSrc[iSrc].ItemArray);
                        iSrc++;
                        iDest++;
                        break;
                    ////目标表小则将目标表本行加入删除队列，同时移动目标表指针
                    case 1:
                        dtRetDel.Rows.Add(drDest[iDest].ItemArray);
                        iDest++;
                        break;
                    default:
                        break;
                }
            }
            //源表到达最后一条，目标表未到达，则目标表剩余行全部加入删除队列
            if (iDest < iDestCount)
            {
                for (int index = iDest; index < iDestCount; index++)
                {
                    dtRetDel.Rows.Add(drDest[index].ItemArray);
                }
            }
            //目标表到达最后一条，源表未到达，则源表剩余行全部加入新增队列
            else if (iSrc < iSrcCount)
            {
                for (int index = iSrc; index < iSrcCount; index++)
                {
                    dtRetAdd.Rows.Add(drSrc[index].ItemArray);
                }
            }
        }
	}
}
