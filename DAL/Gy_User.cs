using System; 
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;
namespace WAES.DAL  
{
	///<summary>
 	///Gy_User
	///</summary>
	public partial class Gy_User: DALBaseSqlServer<Model.Gy_User>
	{
   		     
		public Gy_User() : base("Gy_User", "HInterID") { }
		
		

   
	}
}
