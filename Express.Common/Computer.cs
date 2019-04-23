using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Express.Common
{
    public class Computer
    {
        public string CpuID { get; set; } //CPU的ID
        public int CpuCount { get; set; } //CPU的个数
        public string[] CpuMHZ { get; set; }//CPU频率  单位：hz
        public string BIOSSN { get; set; }//主板
        public string MacAddress { get; set; }//计算机的MAC地址
        public string DiskSerialNumber { get; set; }//硬盘的ID
        public string DiskSize { get; set; }//硬盘大小  单位：bytes
        public string IpAddress { get; set; }//计算机的IP地址
        public string LoginUserName { get; set; }//操作系统登录用户名
        public string ComputerName { get; set; }//计算机名
        public string SystemType { get; set; }//系统类型
        public string MemorySerialNumber { get; set; }//系统类型
        public string TotalPhysicalMemory { get; set; } //总共的内存  单位：M
        private static Computer _instance;
        public static Computer Instance()
        {
            if (_instance == null)
                _instance = new Computer();
            return _instance;
        }
        public Computer()
        {
            CpuID = GetCpuID();
            CpuCount = GetCpuCount();
            CpuMHZ = GetCpuMHZ();
            MacAddress = GetMacAddress();
            DiskSerialNumber = GetDiskSerialNumber();
            DiskSize = GetSizeOfDisk();
            IpAddress = GetIPAddress();
            LoginUserName = GetUserName();
            SystemType = GetSystemType();
            TotalPhysicalMemory = GetTotalPhysicalMemory();
            ComputerName = GetComputerName();
            BIOSSN = GetBiosSerialNumber();
            MemorySerialNumber = GetPhysicalMemorySerialNumber();
        }
        public Computer(int isCheckAuth)
        {
            CpuID = GetCpuID();
            MacAddress = GetMacAddress();
            DiskSerialNumber = GetDiskSerialNumber();
            MemorySerialNumber = GetPhysicalMemorySerialNumber();
        }
        private string GetBiosSerialNumber()
        {
            try
            {
                //获取主板序列号代码
                string biosInfo = "";//主板序列号
                ManagementClass mc = new ManagementClass("Win32_BIOS");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    biosInfo = mo.Properties["SerialNumber"].Value.ToString();
                    break;
                }
                moc = null;
                mc = null;
                return biosInfo;
            }
            catch
            {
                return "unknow";
            }
        }//
        private string GetPhysicalMemorySerialNumber()
        {
            try
            {
                string biosInfo = "";//内存序列号
                ManagementClass mc = new ManagementClass("Win32_PhysicalMemory");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    biosInfo = mo.Properties["SerialNumber"].Value.ToString();
                    break;
                }
                moc = null;
                mc = null;
                return biosInfo;
            }
            catch
            {
                return "unknow";
            }
        }
        string GetCpuID()
        {
            try
            {
                //获取CPU序列号代码
                string cpuInfo = "";//cpu序列号
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
        public static int GetCpuCount()
        {
            try
            {
                using (ManagementClass mCpu = new ManagementClass("Win32_Processor"))
                {
                    ManagementObjectCollection cpus = mCpu.GetInstances();
                    return cpus.Count;
                }
            }
            catch
            {
            }
            return -1;
        }
        public static string[] GetCpuMHZ()
        {
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection cpus = mc.GetInstances();

            string[] mHz = new string[cpus.Count];
            int c = 0;
            ManagementObjectSearcher mySearch = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject mo in mySearch.Get())
            {
                mHz[c] = mo.Properties["CurrentClockSpeed"].Value.ToString();
                c++;
            }
            mc.Dispose();
            mySearch.Dispose();
            return mHz;
        }
        public static string GetSizeOfDisk()
        {
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moj = mc.GetInstances();
            foreach (ManagementObject m in moj)
            {
                return m.Properties["Size"].Value.ToString();
            }
            return "-1";
        }
        string GetMacAddress()
        {
            try
            {
                //获取网卡硬件地址
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true && mo["Description"].ToString().ToLower().IndexOf("virtual") < 0)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
        string GetIPAddress()
        {
            try
            {
                //获取IP地址
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //st=mo[ "IpAddress "].ToString();
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
        string GetDiskSerialNumber()
        {
            try
            {
                //获取硬盘ID
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["SerialNumber"].Value;
                    break;
                }
                if (string.IsNullOrEmpty(HDid))
                {
                    foreach (ManagementObject mo in moc)
                    {
                        HDid = (string)mo.Properties["Model"].Value;
                        break;
                    }
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
        ///    <summary>
        ///   操作系统的登录用户名
        ///    </summary>
        ///    <returns>  </returns>
        string GetUserName()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["UserName"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
        string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["SystemType"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
        string GetTotalPhysicalMemory()
        {
            try
            {

                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["TotalPhysicalMemory"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }
        string GetComputerName()
        {
            try
            {
                return System.Environment.GetEnvironmentVariable("ComputerName");
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }
    }
}
