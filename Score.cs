using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace JiaowuHelper
{
	class Score
	{
		Login login;
		string url = "";
		string postData = "";
		Dictionary<int, string> HeaderList;
		ArrayList scoreList;
		public Score()
		{
			login = Login.get();

		}
		public TabPage[] getAll()
		{
			url = login.homeUrl + "xscjcx.aspx?xh=" + login.id + "&xm=" + login.urlName + "&gnmkdm=N121613";
            if (postData == "")
                postData = getPostData();
                //postData = "__VIEWSTATE=%2FwEPDwUJOTY2ODcyOTkwDxYSHghjamN4X2xzYmQeC3N0cl90YWJfYmpnBRZ6Zl9jeGNqdGpfMjAxMzAxMDExMTY2HghTb3J0RGlyZQUDYXNjHgZzZmRjYmtlHgh6eGNqY3h4cwUBMB4HZHlieXNjamUeClNvcnRFeHByZXMFBGtjbWMeAnhoBQwyMDEzMDEwMTExNjYeA2RnMwUDYmpnFgICAQ9kFhwCBA8QZBAVEAAJMjAwMS0yMDAyCTIwMDItMjAwMwkyMDAzLTIwMDQJMjAwNC0yMDA1CTIwMDUtMjAwNgkyMDA2LTIwMDcJMjAwNy0yMDA4CTIwMDgtMjAwOQkyMDA5LTIwMTAJMjAxMC0yMDExCTIwMTEtMjAxMgkyMDEyLTIwMTMJMjAxMy0yMDE0CTIwMTQtMjAxNQkyMDE1LTIwMTYVEAAJMjAwMS0yMDAyCTIwMDItMjAwMwkyMDAzLTIwMDQJMjAwNC0yMDA1CTIwMDUtMjAwNgkyMDA2LTIwMDcJMjAwNy0yMDA4CTIwMDgtMjAwOQkyMDA5LTIwMTAJMjAxMC0yMDExCTIwMTEtMjAxMgkyMDEyLTIwMTMJMjAxMy0yMDE0CTIwMTQtMjAxNQkyMDE1LTIwMTYUKwMQZ2dnZ2dnZ2dnZ2dnZ2dnZ2RkAgoPEA8WBh4NRGF0YVRleHRGaWVsZAUGa2N4em1jHg5EYXRhVmFsdWVGaWVsZAUGa2N4emRtHgtfIURhdGFCb3VuZGdkEBUGCeW%2FheS%2FruivvgnpgInkv67or74J6ZmQ6YCJ6K%2B%2BCemAmumAieivvgzlvIDmlL7lrp7pqowAFQYCMDECMDICMDMCMDQCMDUAFCsDBmdnZ2dnZ2RkAhMPDxYCHgdWaXNpYmxlaGRkAhgPDxYCHwxoZGQCIA8PFgIeBFRleHRlZGQCIg8PFgQfDQUV5a2m5Y%2B377yaMjAxMzAxMDExMTY2HwxnZGQCJA8PFgQfDQUS5aeT5ZCN77ya5a2Z5YWG5pS%2FHwxnZGQCJg8PFgQfDQUk5a2m6Zmi77ya5py65qKw5LiO5rG96L2m5bel56iL5a2m6ZmiHwxnZGQCKA8PFgQfDQUJ5LiT5Lia77yaHwxnZGQCKg8PFgQfDQUh5py65qKw6K6%2B6K6h5Yi26YCg5Y%2BK5YW26Ieq5Yqo5YyWHwxnZGQCLA8PFgIfDQUN5LiT5Lia5pa55ZCROmRkAi4PDxYEHw0FFuihjOaUv%2BePre%2B8muacuuaisDEzLTMfDGdkZAIwD2QWKAIBDw8WBB8MZx8NBVLmiYDpgInlrabliIYxMDfvvJvojrflvpflrabliIY5NO%2B8m%2BmHjeS%2FruWtpuWIhjbvvJvmraPogIPmnKrpgJrov4flrabliIYgMjbjgII8YnI%2BZGQCAw88KwALAQAPFgIfDGgWAh4Fc3R5bGUFDERJU1BMQVk6bm9uZWQCBQ9kFgICDQ88KwALAGQCBw8PFgQfDQUe6Iez5LuK5pyq6YCa6L%2BH6K%2B%2B56iL5oiQ57up77yaHwxoZGQCCQ88KwALAQAPFgoeCERhdGFLZXlzFgAeC18hSXRlbUNvdW50AgUeCVBhZ2VDb3VudAIBHhVfIURhdGFTb3VyY2VJdGVtQ291bnQCBR8MaBYCHw4FDERJU1BMQVk6bm9uZRYCZg9kFgoCAQ9kFgxmDw8WAh8NBQd4MTEyMTAyZGQCAQ8PFgIfDQUT5aSn5a2m54mp55CG77yIMu%2B8iWRkAgIPDxYCHw0FCeW%2FheS%2FruivvmRkAgMPDxYCHw0FAzIuMGRkAgQPDxYCHw0FAjUwZGQCBQ8PFgIfDQUGJm5ic3A7ZGQCAg9kFgxmDw8WAh8NBQd4MTA0MDA4ZGQCAQ8PFgIfDQUT5aSn5a2m6Iux6K%2Bt77yINO%2B8iWRkAgIPDxYCHw0FCeW%2FheS%2FruivvmRkAgMPDxYCHw0FAzQuMGRkAgQPDxYCHw0FAjM5ZGQCBQ8PFgIfDQUGJm5ic3A7ZGQCAw9kFgxmDw8WAh8NBQd4MTEzMTc2ZGQCAQ8PFgIfDQUb5qaC546H6K665LiO5pWw55CG57uf6K6hSUlJZGQCAg8PFgIfDQUJ5b%2BF5L%2Bu6K%2B%2BZGQCAw8PFgIfDQUDMi4wZGQCBA8PFgIfDQUCMzhkZAIFDw8WAh8NBQYmbmJzcDtkZAIED2QWDGYPDxYCHw0FB3gwMTE1MDZkZAIBDw8WAh8NBRLmjqfliLblt6XnqIvln7rnoYBkZAICDw8WAh8NBQnpgInkv67or75kZAIDDw8WAh8NBQMzLjBkZAIEDw8WAh8NBQEyZGQCBQ8PFgIfDQUGJm5ic3A7ZGQCBQ9kFgxmDw8WAh8NBQd4MTEzMTczZGQCAQ8PFgIfDQUP57q%2F5oCn5Luj5pWwSUlJZGQCAg8PFgIfDQUJ5b%2BF5L%2Bu6K%2B%2BZGQCAw8PFgIfDQUDMi4wZGQCBA8PFgIfDQUCMjBkZAIFDw8WAh8NBQYmbmJzcDtkZAILDzwrAAsBAA8WCh8MZx8PFgAfEAIFHxECAR8SAgUWAh8OBQ1ESVNQTEFZOmJsb2NrFgJmD2QWCgIBD2QWDGYPDxYCHw0FCeW%2FheS%2FruivvmRkAgEPDxYCHw0FATBkZAICDw8WAh8NBQI4NmRkAgMPDxYCHw0FAjEwZGQCBA8PFgIfDQUBMGRkAgUPDxYCHw0FBiZuYnNwO2RkAgIPZBYMZg8PFgIfDQUJ6YCJ5L%2Bu6K%2B%2BZGQCAQ8PFgIfDQUBMGRkAgIPDxYCHw0FATJkZAIDDw8WAh8NBQEzZGQCBA8PFgIfDQUBMGRkAgUPDxYCHw0FBiZuYnNwO2RkAgMPZBYMZg8PFgIfDQUJ6ZmQ6YCJ6K%2B%2BZGQCAQ8PFgIfDQUBMGRkAgIPDxYCHw0FATBkZAIDDw8WAh8NBQEwZGQCBA8PFgIfDQUBMGRkAgUPDxYCHw0FBiZuYnNwO2RkAgQPZBYMZg8PFgIfDQUJ6YCa6YCJ6K%2B%2BZGQCAQ8PFgIfDQUBMGRkAgIPDxYCHw0FATZkZAIDDw8WAh8NBQEwZGQCBA8PFgIfDQUBMGRkAgUPDxYCHw0FBiZuYnNwO2RkAgUPZBYMZg8PFgIfDQUM5byA5pS%2B5a6e6aqMZGQCAQ8PFgIfDQUBMGRkAgIPDxYCHw0FATBkZAIDDw8WAh8NBQEwZGQCBA8PFgIfDQUBMGRkAgUPDxYCHw0FBiZuYnNwO2RkAg0PPCsACwEADxYKHwxnHw8WAB8QAgYfEQIBHxICBhYCHw4FDURJU1BMQVk6YmxvY2sWAmYPZBYMAgEPZBYKZg8PFgIfDQUP5Lq65paH56S%2B56eR57G7ZGQCAQ8PFgIfDQUBMGRkAgIPDxYCHw0FATBkZAIDDw8WAh8NBQEwZGQCBA8PFgIfDQUBMGRkAgIPZBYKZg8PFgIfDQUP57uP5rWO566h55CG57G7ZGQCAQ8PFgIfDQUBMGRkAgIPDxYCHw0FATBkZAIDDw8WAh8NBQEwZGQCBA8PFgIfDQUBMGRkAgMPZBYKZg8PFgIfDQUP6Ieq54S256eR5a2m57G7ZGQCAQ8PFgIfDQUBMGRkAgIPDxYCHw0FATRkZAIDDw8WAh8NBQEwZGQCBA8PFgIfDQUBMGRkAgQPZBYKZg8PFgIfDQUP6Im65pyv5L2T6IKy57G7ZGQCAQ8PFgIfDQUBMGRkAgIPDxYCHw0FATJkZAIDDw8WAh8NBQEwZGQCBA8PFgIfDQUBMGRkAgUPZBYKZg8PFgIfDQUJ5aSW6K%2Bt57G7ZGQCAQ8PFgIfDQUBMGRkAgIPDxYCHw0FATBkZAIDDw8WAh8NBQEwZGQCBA8PFgIfDQUBMGRkAgYPZBYKZg8PFgIfDQUM5byA5pS%2B5a6e6aqMZGQCAQ8PFgIfDQUBMGRkAgIPDxYCHw0FATBkZAIDDw8WAh8NBQEwZGQCBA8PFgIfDQUBMGRkAg8PPCsACwBkAhEPPCsACwEADxYKHwxnHw8WAB8QAgEfEQIBHxICARYCHw4FDURJU1BMQVk6YmxvY2sWAmYPZBYCAgEPZBYMZg8PFgIfDQUG5ZCI6K6hZGQCAQ8PFgIfDQUGJm5ic3A7ZGQCAg8PFgIfDQUGJm5ic3A7ZGQCAw8PFgIfDQUGJm5ic3A7ZGQCBA8PFgIfDQUGJm5ic3A7ZGQCBQ8PFgIfDQUD5qyhZGQCEg88KwALAQAPFgIfDGgWAh8OBQxESVNQTEFZOm5vbmVkAhMPPCsACwEADxYCHwxoZGQCFQ88KwALAQAPFgofDGgfDxYAHxBmHxECAR8SZhYCHw4FDERJU1BMQVk6bm9uZWQCFw88KwALAQAPFgofDGgfDxYAHxBmHxECAR8SZhYCHw4FDERJU1BMQVk6bm9uZWQCGQ88KwALAgAPFgofDxYAHxBmHxECAR8SZh8MaGQBFCsABzwrAAQBABYCHgpGb290ZXJUZXh0BQnmgLvorqHvvJpkPCsABAEAFgIeCkhlYWRlclRleHQFDOWIm%2BaWsOWGheWuuTwrAAQBABYCHxQFDOWIm%2BaWsOWtpuWIhjwrAAQBABYCHxQFDOWIm%2BaWsOasoeaVsGRkZAIbDw8WBB8NBRLmnKzkuJPkuJrlhbEyNDjkurofDGdkZAIdDw8WBB8MZx8NZWRkAh8PDxYEHwxnHw1lZGQCIQ8PFgIfDGdkZAIjDw8WAh8NBQVTRElMSWRkAiQPDxYCHghJbWFnZVVybGRkZAIyDxYCHwxoFgICAw88KwALAGRkUBwX9HxaNnyS0Sj%2BFCFUCRJyRvY%3D&__EVENTVALIDATION=%2FwEWIwKfr4KUAQLd4qCqDwLuwOmEBQL86fHqBQL%2F6e38CwL26ZnpBAL56bXzCgL46aHpBwL76d3zBQLy6cnpBgL16aWQBgL66c2rCAKgq8rFDgKjq46ECAKiq5LECQK9q9aECwK8q7rECAK%2Fq%2F6ECgLfwOmEBQLQr8PqCQLRr8PqCQLSr8PqCQKfsIjIDgKfsIzIDgKfsLDIDgKfsLTIDgKfsLjIDgKP3%2B6lAgLwkp3BDALwksHADAKKxdH8DALukuXADAK7q7GGCAKM54rGBgKMk%2F3ADAznkwecXs3t1J6V%2FmcOu7G5x5Zu&hidLanguage=&ddlXN=&ddlXQ=&ddl_kcxz=&btn_zcj=%C0%FA%C4%EA%B3%C9%BC%A8";
            Debug.WriteLine(postData);
			string page = getPostPage(postData);
			//Url.writeFile(page, "E:\\socre.post.html");
			if (parse(page))
			{
				Dictionary<string, ListView> list = createListView();
				TabPage[] tabPages = new TabPage[list.Count];
				int i = 0;
				foreach (string name in list.Keys)
				{
					tabPages[i] = new TabPage();
					tabPages[i].Text = name;
					tabPages[i].Controls.Add(list[name]);
					i++;
				}
				return tabPages;
			}
			return null;
		}
		private Dictionary<string, ListView> createListView()
		{
			Dictionary<string, ListView> dlv = new Dictionary<string, ListView>();
			foreach (Dictionary<string, string> list in scoreList)
			{
				string name = list[HeaderList[0]] + " " + list[HeaderList[1]];
				if (!dlv.Keys.Contains(name))
				{
					ListView lv = new ListView();
					lv.View = View.Details;
					lv.FullRowSelect = true;
					lv.GridLines = true;
					foreach (string str in HeaderList.Values)
					{
						ColumnHeader header1 = new ColumnHeader();
						header1.Width = -2;
						header1.Text = str;
						header1.TextAlign = HorizontalAlignment.Left;
						lv.Columns.Add(header1);
					}
					lv.Dock = DockStyle.Fill;
					dlv.Add(name, lv);
				}
				dlv[name].Items.Add(new ListViewItem(list.Values.ToArray()));
			}
			return dlv;
		}
		private string getPostData()
		{
			string page = Url.readHtml(Url.getPageStream(url));
			//Url.writeFile(page, "E:\\score.html");
			if (page == "") return "";
            Debug.WriteLine(page);
			string rt = "__EVENTTARGET=&__EVENTARGUMENT=&hidLanguage=&ddlXN=&ddlXQ=&ddl_kcxz=&btn_zcj=%C0%FA%C4%EA%B3%C9%BC%A8&__VIEWSTATE=";//btn_zcj
            Regex reg = new Regex("__VIEWSTATE\" value=\"(.+?)\"");
			Match match = reg.Match(page);
			if (match.Value == "") return "";
			rt += HttpUtility.UrlEncode(match.Value.Substring("__VIEWSTATE\" value=\"".Length, match.Length - "__VIEWSTATE\" value=\"".Length - "\"".Length), Encoding.GetEncoding("gb2312"));
            reg = new Regex("__EVENTVALIDATION\" value=\"(.+?)\"");
            match = reg.Match(page);
            if (match.Value != "") {
                rt += "&"+HttpUtility.UrlEncode(match.Value.Substring("__EVENTVALIDATION\" value=\"".Length, match.Length - "__EVENTVALIDATION\" value=\"".Length - "\"".Length), Encoding.GetEncoding("gb2312"));
            }
            //__EVENTVALIDATION
            return rt;
		}
		private string getPostPage(string postData)
		{
			Stream stream = Url.getPostStream(url, postData);
			return Url.readHtml(stream);
		}
		private bool parse(string page)
		{
			Regex reg = new Regex("<table class=\"datelist\" (.+?)</table>", RegexOptions.Singleline);
			Match match = reg.Match(page);
            Debug.WriteLine(page);
            if (match.Value == "")
			{
				return false;
			}
			string rt = Regex.Replace(match.Value, "\\s", "", RegexOptions.Singleline).Replace("</td><td>", "@").Replace("class=\"alt\"", "").Replace("</tr><tr>", "\n");
			rt = Regex.Replace(rt, "<(.+?)>", "", RegexOptions.None).Replace("&nbsp;", "");
			string[] split = rt.Split('\n');
			string[] split2;
			bool header = true;
			HeaderList = new Dictionary<int, string>();
			scoreList = new ArrayList();
			try
			{
				foreach (string tmp in split)
				{
					split2 = tmp.Split('@');
					if (header)
					{
						for (int i = 0; i < split2.Length; i++)
						{
							if (split2[i] != "")
								HeaderList.Add(i, split2[i]);
						}
						header = false;
						continue;
					}
					Dictionary<string, string> sc = new Dictionary<string, string>();
					foreach (int i in HeaderList.Keys)
					{
						sc.Add(HeaderList[i], split2[i]);
					}
					scoreList.Add(sc);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
