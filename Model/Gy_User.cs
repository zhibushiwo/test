using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace WAES.Model{
	 	//Gy_User
		public class Gy_User
	{
   		public Gy_User()
   		{        
   			this.HInterID = 0;
			this.HNumber = string.Empty;
			this.HName = string.Empty;
			this.HPassword = string.Empty;
			this.HCreateTime = DateTime.Now;
            this.Lines = string.Empty;
        }
      	/// <summary>
		/// HInterID
        /// </summary>
        public int HInterID { get; set; }        
		/// <summary>
		/// HNumber
        /// </summary>
        public string HNumber { get; set; }        
		/// <summary>
		/// HName
        /// </summary>
        public string HName { get; set; }        
		/// <summary>
		/// HPassword
        /// </summary>
        public string HPassword { get; set; }        
		/// <summary>
		/// HCreateTime
        /// </summary>
        public DateTime HCreateTime { get; set; }  
        
        public string Lines { get; set; }  
		   
	}
}