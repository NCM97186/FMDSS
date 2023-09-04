using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.App_Start
{
	public static class FmdssStaticData
	{
		
		public static List<LoggUserMembers> LoggUserList = new List<LoggUserMembers>();
        public static List<ReturnURLs> ReturnUrlList = new List<ReturnURLs>();
        public static void AddReturnURL(string IpAddress,string ReturnUrl)
        {
            ReturnURLs addObj = new ReturnURLs {  IpAddress = IpAddress, ReturnUrl= ReturnUrl, inTime = DateTime.Now };
            ReturnUrlList.Add(addObj);
        }

        public static string GetReturnURLByIp(string IpAddress)
        {
            ReturnURLs mem = ReturnUrlList.Where(c =>  c.IpAddress == IpAddress).FirstOrDefault();
            if (mem == null)
                return "";

            string strUrl = mem.ReturnUrl;
            ReturnUrlList.RemoveAll(x => x.IpAddress.ToLower().Trim() == IpAddress.ToLower().Trim());

            return strUrl;
        }
        public static void RemoveIdleURL(int maxTimeLimit)
        {
            
            ReturnUrlList.RemoveAll(c => ((DateTime.Now - c.inTime).TotalSeconds > 0 ? (DateTime.Now - c.inTime).TotalMinutes : 0) > maxTimeLimit);
        }

        public static void  AddLoggedUsers(string SSOId,string IpAddress,string MobileNo)
		{
            MobileNo = (MobileNo == null ? SSOId : MobileNo);
            string browserDetails = string.Empty;
            System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
            browserDetails = browser.Browser + " " + browser.Type + " " + browser.Version;
            LoggUserMembers addObj = new LoggUserMembers { SSOId = SSOId, IpAddress = IpAddress, MobileNo = MobileNo ,BrowserName= browserDetails, LoginTime = DateTime.Now };
			LoggUserList.Add(addObj);
		}
		public static void RemoveLoggedUsers(string SSOId)
		{
			//LoggUserMembers removeObj = new LoggUserMembers { SSOId = SSOId, IpAddress = IpAddress, MobileNo = MobileNo };
			LoggUserList.RemoveAll(x => x.SSOId.ToLower().Trim() == SSOId.ToLower().Trim());            
        }
        public static void RemoveLoggedUsers(string SSOId, string IpAddress)
        {
            //LoggUserMembers removeObj = new LoggUserMembers { SSOId = SSOId, IpAddress = IpAddress, MobileNo = MobileNo };

            LoggUserList.RemoveAll(x => x.SSOId.ToLower().Trim() == SSOId.ToLower().Trim());
            LoggUserList.RemoveAll(x => x.IpAddress.ToLower().Trim() == IpAddress.ToLower().Trim());
        }
        public static bool IsAlreadyLogged(ref string Msg,string SSOId, string IpAddress, string MobileNo)
		{
            MobileNo = (MobileNo == null ? SSOId : MobileNo);
            Msg = "";
			bool isLogged = false;
			LoggUserMembers mem = LoggUserList.Where(c => c.SSOId.ToLower() == SSOId.ToLower()).FirstOrDefault();
			if (mem != null)
			{
				if (mem.SSOId != null)
				{
					Msg = "This SSO Id is already login in any browser";
					isLogged = true;
				}
			}
			LoggUserMembers mem2 = LoggUserList.Where(c => c.IpAddress.ToLower() == IpAddress.ToLower()).FirstOrDefault();
			if (mem2 != null)
			{
				if (mem2.IpAddress != null)
				{
					if(Msg.Length==0 || Msg.Trim()=="")
						Msg = "This Ip Address is already login in FMDSS Application";
					else
						Msg = Msg + ", This Ip Address is already login in FMDSS Application";
					isLogged = true;
				}
			}
			LoggUserMembers mem3 = LoggUserList.Where(c => c.MobileNo.ToLower() == MobileNo.ToLower()).FirstOrDefault();
			if (mem3 != null)
			{
				if (mem3.MobileNo != null)
				{
					if (Msg.Length == 0 || Msg.Trim() == "")
						Msg = "This Mobile Number is already login in any browser";
					else
						Msg = Msg + ", This Mobile Number is already login in any browser";
					isLogged = true;
				}
			}
			return isLogged;
		}
		public static string GetCurrentBrowser()
		{
			string browserDetails = string.Empty;
			System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
			browserDetails = browser.Browser + " " + browser.Type + " " + browser.Version;
			//browserDetails =
			//"Name = " + browser.Browser + "," +
			//"Type = " + browser.Type + ","
			//+ "Version = " + browser.Version + ","
			//+ "Major Version = " + browser.MajorVersion + ","
			//+ "Minor Version = " + browser.MinorVersion + ","
			//+ "Platform = " + browser.Platform + ","
			//+ "Is Beta = " + browser.Beta + ","
			//+ "Is Crawler = " + browser.Crawler + ","
			//+ "Is AOL = " + browser.AOL + ","
			//+ "Is Win16 = " + browser.Win16 + ","
			//+ "Is Win32 = " + browser.Win32 + ","
			//+ "Supports Frames = " + browser.Frames + ","
			//+ "Supports Tables = " + browser.Tables + ","
			//+ "Supports Cookies = " + browser.Cookies + ","
			//+ "Supports VBScript = " + browser.VBScript + ","
			//+ "Supports JavaScript = " + "," +
			//browser.EcmaScriptVersion.ToString() + ","
			//+ "Supports Java Applets = " + browser.JavaApplets + ","
			//+ "Supports ActiveX Controls = " + browser.ActiveXControls
			//+ ","
			//+ "Supports JavaScript Version = " +
			//browser["JavaScriptVersion"];
			return browserDetails;
		}
		public static string GetBrowserName(string SSOId, string IpAddress, string MobileNo)
		{
            MobileNo = (MobileNo == null ? SSOId : MobileNo);
            LoggUserMembers mem = LoggUserList.Where(c =>( c.SSOId.ToLower() == SSOId.ToLower() || c.MobileNo.ToLower() == MobileNo.ToLower()) && c.IpAddress== IpAddress).FirstOrDefault();
            if(mem==null)
                return "";
            return mem.BrowserName;
		}
		public static string GetLoggedIPAddress(string SSOId,  string MobileNo)
		{
            MobileNo = (MobileNo == null ? SSOId : MobileNo);
            LoggUserMembers mem = LoggUserList.Where(c => c.SSOId.ToLower() == SSOId.ToLower()  || c.MobileNo.ToLower() == MobileNo.ToLower()).FirstOrDefault();

            if (mem == null)
                return "";
            return mem.IpAddress;
		}
		public static int GetIPAddressCount(string IpAddress)
		{
			int Cnt = LoggUserList.Where(c => c.IpAddress.ToLower() == IpAddress.ToLower()).Count();
			return Cnt;
		}
      
        public static int GetUserLoggedCount(string SSOId, string MobileNo)
        {
            MobileNo = (MobileNo == null ? SSOId : MobileNo);
            int Cnt = LoggUserList.Where(c => c.SSOId.ToLower() == SSOId.ToLower() || c.MobileNo.ToLower() == MobileNo.ToLower() ).Count();
            return Cnt;
        }
        public static DateTime GetLoginTime(string SSOId,string IpAddress)
		{
			LoggUserMembers mem = LoggUserList.Where(c => c.SSOId.ToLower() == SSOId.ToLower()).FirstOrDefault();
            if (mem == null)
                mem = LoggUserList.Where(c => c.IpAddress.ToLower() == IpAddress.ToLower()).FirstOrDefault();

            if (mem == null)
				return DateTime.Now;

			return mem.LoginTime;
		}
        public static int GetActiveUserCount()
        {
            int Cnt = LoggUserList.Count();
            return Cnt;
        }

    }
	public class LoggUserMembers
	{
		public string SSOId { get; set; }
		public string IpAddress { get; set; }
		public string MobileNo { get; set; }
		public string BrowserName { get; set; }
		public DateTime LoginTime { get; set; }
	}
    public class ReturnURLs
    {
        public string IpAddress { get; set; }
        public string ReturnUrl { get; set; }
        public DateTime inTime { get; set; }
    }
}