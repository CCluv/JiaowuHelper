﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Web;

namespace JiaowuHelper
{
	public partial class FormLogin : Form
	{
		Login login;
		FormHelper helper;
		bool show_checkCode = false;
		bool trueLoginPage = false;
		bool firstRun = true;
		
		public FormLogin()
		{
			this.login = Login.get();
			login.init();
			InitializeComponent();
		}

		private void button_cancal_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void loadConfig()
		{
			ConfigInfo ci = Login.get().ci;
            if (ci.url == "") {
                Random random = new Random();
                switch(random.Next(3)){
                    case 0:
                {
                    textBox_url.Text = "http://210.44.159.4/";
                            break;
                }
                        case 1:
                {
                    textBox_url.Text = "http://210.44.159.20/";
                            break;
                }
                    default:
                        {
                            textBox_url.Text = "http://210.44.159.4/";
                            break;
                        }
                 }
			}
			else
			{
				textBox_url.Text = ci.url;
			}
			textBox_username.Text = ci.username;
			textBox_password.Text = ci.password;
		}
		private void setSiteUrl()
		{
			if (textBox_url.Text.ToLower().IndexOf("http") != 0)
			{
				textBox_url.Text = "http://" + textBox_url.Text;
			}
			if (textBox_url.Text != "")
			{
				this.Cursor = Cursors.WaitCursor;
				Uri uri = Url.GetTrueUrl(textBox_url.Text);
				this.Cursor = Cursors.Default;

				if (uri == null)
				{
					MessageBox.Show("非法地址，或地址无法连接", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				Regex reg = new Regex("http(.+?)/\\(([a-z0-9]+)\\)/");
				Match match = reg.Match(uri.ToString());
				if ((login.cookie.Count == 0 && match.Value == "") || login.loginPage.IndexOf("name=\"__VIEWSTATE\"") < 1)
				{
					MessageBox.Show("这不是一个正确的教务系统地址", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
					trueLoginPage = false;
				}
				else
				{
					//自动检测验证码

					//checkBox_checkCode.Checked = show_checkCode = login.requirdCheckCode;
					//checkBox_checkCode_CheckedChanged(null, null);

					if (match.Value == "" || match.Value == null)
					{
						login.homeUrl = uri.ToString();
                        login.staticUrl = login.homeUrl;
                    }
					else
					{
						login.homeUrl = match.Value;
                        login.staticUrl = login.homeUrl;
					}
					trueLoginPage = true;
					setCheckCode();
				}
			}
		}

		private void setCheckCode()
		{
			if (checkBox_checkCode.Checked == false) return;
			//if (!Login.get().requirdCheckCode)
			//{
			//	if (MessageBox.Show("检测到登录界面不需要验证码，是否继续显示？", "异常操作", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
			//		return;
			//}
			if (show_checkCode)
			{
				while (true)
				{
					this.Cursor = Cursors.WaitCursor;
					pictureBox_checkCode.Image = Url.GetImageWithCookie(login.homeUrl + "CheckCode.aspx");
					this.Cursor = Cursors.Default;
					textBox_checkcode.Text = "";
					if (pictureBox_checkCode.Image == null)
					{
						if (MessageBox.Show("Cookie验证码加载出错，也许地址错误，是否重试？", "验证码错误",
							MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
							continue;
					}
					break;
				}
			}
		}

		private void button_set_Click(object sender, EventArgs e)
		{
			setSiteUrl();
		}

		private void pictureBox_checkCode_Click(object sender, EventArgs e)
		{
			setCheckCode();
		}

		private void button_login_Click(object sender, EventArgs e)
		{
			if (trueLoginPage == false)
			{
				MessageBox.Show("当前教务地址不正确，请设置正确的地址", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (textBox_url.Text == "" || textBox_username.Text == "" || textBox_password.Text == "")
			{
				MessageBox.Show("登录表单有误,地址 用户名 密码三项必填", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (Url.PostLogin(getForm()))
			{
				Login.get().setLogin(true);
				Close();
			}
			else
			{
				if (login.LoginError == "")
					MessageBox.Show("登录异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else
					MessageBox.Show(login.LoginError);
				setCheckCode();
			}
		}
		private string getForm()
		{
			Regex reg = new Regex("action=\"(.+?)\" ");
			Match match = reg.Match(login.loginPage);
			int n1 = "action=\"".Length;
			int n2 = "\" ".Length;
			login.loginActionPage = login.homeUrl + match.Value.Substring(n1, match.Value.Length - n1 - n2);
			reg = new Regex("\"__VIEWSTATE\" value=\"(.+?)\"");
            /*
            <input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="/wEPDwUKMTMzMzIxNTg3OWRk6klhbJMzNV8MFPW5Znuu4duJhdw=" />
__VIEWSTATE=dDwtMTMxNjk0NzYxNTs7PpK7CYMIAY8gja8M8G8YpGL8ZEAL__VIEWSTATEGENERATOR=92719903&txtUserName=201301011166&TextBox2=ling77&TextBox3=&Button1=&lbLanguage=&RadioButtonList1=%d1%a7%c9%fa
            */
            match = reg.Match(login.loginPage);
			n1 = "\"__VIEWSTATE\" value=\"".Length;
			Encoding encoding = Encoding.GetEncoding("gb2312");
			string rt = "";
            if (match.Value.Length == 0)
            {
                MessageBox.Show("Regex match failed in getForm()", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return rt;
            }
			rt = HttpUtility.UrlEncode("__VIEWSTATE", encoding) 
                + "="
                + HttpUtility.UrlEncode(
                match.Value.Substring(n1, match.Value.Length - n1 - 1
                ), encoding);
            reg = new Regex("__VIEWSTATEGENERATOR\" value=\"(.+?)\"");
            match = reg.Match(login.loginPage);
            n1 = "__VIEWSTATEGENERATOR\" value=\"".Length;
            if (match.Value != "") {
                rt += "&"+HttpUtility.UrlEncode("__VIEWSTATEGENERATOR", encoding)
                                + "="
                                + HttpUtility.UrlEncode(
                                match.Value.Substring(n1, match.Value.Length - n1 - 1
                                ), encoding);
            }
            reg = new Regex("hidPdrs(.+?)hidPdrs");
            match = reg.Match(login.loginPage);
            if (match.Value != "") {
                rt += "&" 
                    + HttpUtility.UrlEncode("hidPdrs", encoding)
                    + "=";
            }
            reg = new Regex("hidsc(.+?)hidsc");
            match = reg.Match(login.loginPage);
            if (match.Value != "") {
                rt += "&"
                    + HttpUtility.UrlEncode("hidsc", encoding)
                    + "=";
            }
            reg = new Regex("__EVENTVALIDATION\" value=\"(.+?)\"");
            match = reg.Match(login.loginPage);
            n1 = "__EVENTVALIDATION\" value=\"".Length;
            if (match.Value != "") {
                rt += "&" + HttpUtility.UrlEncode("__EVENTVALIDATION", encoding)
                                + "="
                                + HttpUtility.UrlEncode(
                                match.Value.Substring(n1, match.Value.Length - n1 - 1
                                ), encoding);
            }
            rt += "&" + HttpUtility.UrlEncode("txtUserName", encoding) + "=" + HttpUtility.UrlEncode(textBox_username.Text, encoding);
			login.id = textBox_username.Text;
			rt += "&" + HttpUtility.UrlEncode("TextBox2", encoding) + "=" + HttpUtility.UrlEncode(textBox_password.Text, encoding);
            rt += "&" + HttpUtility.UrlEncode("TextBox3", encoding) + "=" + HttpUtility.UrlEncode(textBox_checkcode.Text, encoding);
			rt += "&" + HttpUtility.UrlEncode("Button1", encoding) + "=" + HttpUtility.UrlEncode("", encoding);
			rt += "&" + HttpUtility.UrlEncode("lbLanguage", encoding) + "=" + HttpUtility.UrlEncode("", encoding);
			rt += "&" + HttpUtility.UrlEncode("RadioButtonList1", encoding) + "=" + HttpUtility.UrlEncode("学生", encoding);
			return rt;
		}
       
        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 13)
			{
				button_login_Click(sender, e);
			}
		}

		private void button_confSave_Click(object sender, EventArgs e)
		{
			if (Config.save(new ConfigInfo
			{
				url = textBox_url.Text,
				username = textBox_username.Text,
				password = textBox_password.Text,
				autoLogin = checkBox_autoLogin.Checked
			}))
			{
				MessageBox.Show("配置保存成功");
			}
			else
			{
				MessageBox.Show("失败，无法保存配置文件", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void button_confDel_Click(object sender, EventArgs e)
		{
			if (Config.delete())
			{
				MessageBox.Show("删除成功");
			}
			else
			{
				MessageBox.Show("删除失败", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void checkBox_checkCode_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox_checkCode.Checked)
			{
				if (show_checkCode == false)
				{
					show_checkCode = true;
					setCheckCode();
				}
				panel_checkCode.Enabled = true;

			}
			else
			{
				show_checkCode = false;
				panel_checkCode.Enabled = false;
				pictureBox_checkCode.Image = null;
			}
		}

		private void button_help_Click(object sender, EventArgs e)
		{
			if (helper == null || helper.IsDisposed)
			{
				helper = new FormHelper();
				helper.Show();
				helper.Location = Postion.getPostion(helper, 0.5f, 0.5f, this);
			}
			else
			{
				helper.Activate();
			}
		}

		private void FormLogin_Activated(object sender, EventArgs e)
		{
			//首次执行时进行自动登录检测
			if (firstRun == false || trueLoginPage == false)return;
			firstRun = false;
			if (Login.get().ci.autoLogin && login.requirdCheckCode == false)
			{
				if (textBox_url.Text != "" && textBox_username.Text != "" && textBox_password.Text != "")
					button_login_Click(null, null);
			}
		}

		private void FormLogin_Load(object sender, EventArgs e)
		{
           // this.Icon = JiaowuHelper.Resource.jiaowu;
			Location = Postion.getPostion(this, 0.4f, 0.4f, null);
			loadConfig();
			setSiteUrl();
            checkBox_checkCode.CheckState = CheckState.Unchecked;
            checkBox_checkCode.Checked = false;
        }

		private void button_about_Click(object sender, EventArgs e)
		{
			new AboutBox().ShowDialog(); setSiteUrl();
        }

        private void Url1_Click(object sender, EventArgs e)
        {
            textBox_url.Text = "http://210.44.159.4/"; setSiteUrl();
        }

        private void Url2_Click(object sender, EventArgs e)
        {
            textBox_url.Text = "http://210.44.159.22/"; setSiteUrl();
        }

        private void Url3_Click(object sender, EventArgs e)
        {
            textBox_url.Text = "http://210.44.159.20/"; setSiteUrl();
        }
    }
}
