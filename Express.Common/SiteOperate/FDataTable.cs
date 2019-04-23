using System;
using System.Data ;
namespace Express.Common.SiteOperate
{
	/// <summary>
	/// FDataTable ��ժҪ˵����
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
        /// ��ȡdt��columnName��ֵ���ԡ�,������(��ֵ��������)
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
        /// ��ȡdt��columnName��ֵ���ԡ�,������(��ֵ��������)
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
		/// �õ�Excel���ַ���
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
		/// �õ�����ҳ
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public static int GetTableCount(DataTable dt,int pageSize)
		{
			int pageCount = 0 ;
			int recordCount = dt.Rows.Count ; //ȥ��ͷ�У�ͷ���Ǳ�ͷ
			if (recordCount>0)
			{
				pageCount = recordCount / pageSize ; // �� pageSize = 10 ,recordCount=40��4ҳ 41ҳ5ҳ
				if ((recordCount%pageSize)>0)
					pageCount = pageCount + 1 ;
			}
			return pageCount ;
		}

		/// <summary>
		/// �õ�ÿҳ����
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
		/// �ָ�˫����
		/// </summary>
		/// <param name="dtParnet">dtParent.Rows.Count = dtSubs.Length</param>
		/// <param name="dtSubs"></param>
		/// <param name="pageSize"></param>
		public static void GetBothTables(DataTable dtParent,DataTable[] dtSubs,int pageSize,out DataTable nowDtParent,out System.Collections.ArrayList arrDtSub)
		{
			nowDtParent = dtParent.Clone() ;
			nowDtParent.Columns.Add(new DataColumn("TableIndex",System.Type.GetType("System.Int32"))) ; //��Parent���е�Index
			nowDtParent.Columns.Add(new DataColumn("SubTableCount",System.Type.GetType("System.Int32"))) ;//����Sub�����
			nowDtParent.Columns.Add(new DataColumn("SubTableIndex",System.Type.GetType("System.Int32"))) ;//��ǰSub���Index
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
		/// �ָ���һ��񣨸���ҳ�������ָ������ɸ����
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
		/// �ÿ��������
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
        /// ���ݹؼ���ɾ��dt�����
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
        /// �Ƚ�����DataTable���ݣ��ṹ��ͬ,�ֶ�����ͬ��
        /// </summary>
        /// <param name="dtDest">�������ݿ��DataTable</param>
        /// <param name="dtSrc">�����ļ���DataTable</param>
        /// <param name="dtRetAdd">��ͬ������</param>
        /// <param name="dtRetAdd">�������ݣ�dt2�е����ݣ�</param>
        /// <param name="dtRetDel">ɾ�������ݣ�dt2�е����ݣ�</param>
        /// <param name="srcKeyFields">Դ�ؼ��ֶ���</param>
        /// <param name="destKeyFields">Ŀ��ؼ��ֶ�������Դ�ؼ��ֶ�����Ӧ</param>
        public static void CompareDt(DataTable dtSrc, DataTable dtDest, out DataTable dtSame, out DataTable dtRetAdd, out DataTable dtRetDel, string srcKeyFields, string destKeyFields)
        {
            //Դ��¼����Ŀ���¼����һ��Ϊnull���˳�
            if (dtSrc == null || dtDest == null)
            {
                dtRetDel = null;
                dtRetAdd = null;
                dtSame = null;
                return;
            }
            //���巵�ؼ�¼��
            dtRetDel = dtSrc.Clone();
            dtRetAdd = dtRetDel.Clone();
            dtSame = dtSrc.Clone();
            //������Ϊ�����˳�
            if (string.IsNullOrEmpty(srcKeyFields) || string.IsNullOrEmpty(destKeyFields))
                return;
            //��ò������б�
            string[] srcFields = srcKeyFields.Split(',');//��������
            string[] destFields = destKeyFields.Split(',');//��������
            //��������Ŀ��һ�����˳�
            if (srcFields.Length != destFields.Length)
                return;
            //������������Դ���Ŀ���
            DataRow[] drSrc = dtSrc.Select("", srcKeyFields);
            DataRow[] drDest = dtDest.Select("", destKeyFields);
            //����Դ���Ŀ�����
            int iSrcCount = drSrc.Length;
            int iDestCount = drDest.Length;
            //Դ��Ϊ����Ŀ���ȫ������ɾ�����в�����
            if (iSrcCount == 0)
            {
                foreach (DataRow row in drDest)
                {
                    dtRetDel.Rows.Add(row.ItemArray);
                }
                return;
            }
            //Ŀ���Ϊ����Դ��ȫ�������������в�����
            if (iDestCount == 0)
            {
                foreach (DataRow row in drSrc)
                {
                    dtRetAdd.Rows.Add(row.ItemArray);
                }
                return;
            }
            //����Դ���Ŀ���ָ��
            int iSrc = 0;
            int iDest = 0;
            //��ʼѭ���ȶ�
            while (iSrc < iSrcCount && iDest < iDestCount)
            {
                //�����бȶԽ��
                int result = 0;
                object oSrc;
                object oDest;
                //ѭ���ȶ���ֵ
                for (int colIndex = 0; colIndex < srcFields.Length; colIndex++)
                {
                    //�����ֵ
                    oSrc = drSrc[iSrc][srcFields[colIndex]];
                    oDest = drDest[iDest][destFields[colIndex]];
                    //�Ƚ���ֵ����������˳�ѭ��
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
                //����бȽϽ��
                switch (result)
                {
                    ////Դ��С��Դ���м����������У�ͬʱ�ƶ�Դ��ָ��
                    case -1:
                        dtRetAdd.Rows.Add(drSrc[iSrc].ItemArray);
                        iSrc++;
                        break;
                    ////��ͬ����ͬʱ�ƶ�ָ��
                    case 0:
                        dtSame.Rows.Add(drSrc[iSrc].ItemArray);
                        iSrc++;
                        iDest++;
                        break;
                    ////Ŀ���С��Ŀ����м���ɾ�����У�ͬʱ�ƶ�Ŀ���ָ��
                    case 1:
                        dtRetDel.Rows.Add(drDest[iDest].ItemArray);
                        iDest++;
                        break;
                    default:
                        break;
                }
            }
            //Դ�������һ����Ŀ���δ�����Ŀ���ʣ����ȫ������ɾ������
            if (iDest < iDestCount)
            {
                for (int index = iDest; index < iDestCount; index++)
                {
                    dtRetDel.Rows.Add(drDest[index].ItemArray);
                }
            }
            //Ŀ��������һ����Դ��δ�����Դ��ʣ����ȫ��������������
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
