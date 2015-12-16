using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sashulin;
using Sashulin.Core;
using Sashulin.common;
using LitJson;
using System.IO;
using System.Threading;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WinFormDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void setCookiePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.SetCookiePath("c:\\temp");
        }

        private void getElementValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string s = chromeWebBrowser1.GetElementValueById("kw1");
            //MessageBox.Show(s);

            flag = true;
            timer1.Start();
        }

        private void deleteAllCookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.DeleteAllCookies();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.OpenUrl(textBox1.Text);
        }

        private void setElementValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.SetElementValueByid("kw1", "input some data...");
        }

        private void showDevToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.ShowDevTool();
        }

        private void setScreenSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.SetScreenSize(480, 680);
        }

        Dictionary<String, String> categoryList = new Dictionary<String, String>();
        
        private void Form1_Load(object sender, EventArgs e)
        {
            CSharpBrowserSettings settings = new CSharpBrowserSettings();
            settings.DefaultUrl = "";
            //settings.UserAgent = "Mozilla/5.0 (Linux; Android 4.2.1; en-us; Nexus 4 Build/JOP40D) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.166 Mobile Safari/535.19";
            settings.CachePath = @"C:\temp\caches";
            chromeWebBrowser1.Initialize(settings);


          

            DataTable dt = SqlHelper.ExecuteDataSetText("select * from Category", null).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                categoryList.Add(dr[1].ToString(), dr[0].ToString());
            }

        }

        private void resetScreenSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.ResetScreen();
        }

        private void getSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string source = chromeWebBrowser1.GetSource();
            MessageBox.Show(source);
        }

        private void callCSharpMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.OpenUrl("file:///D:/temp/ChromeWebBrowser.net_1.0.311/ChromeWebBrowser.net_1.0.3/example/cachedbTest.html");
        }

        public string ShowMessage(string msg)
        {
            //MessageBox.Show(msg);
            Form2 f = new Form2();

            f.Show();
            return "return : Js call";
        }

        private void evalScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //object o = chromeWebBrowser1.EvaluateScript("getAgent();"); // be ok.
            object o = chromeWebBrowser1.EvaluateScript("document.childNodes[0].childNodes[1].innerHTML;");
            MessageBox.Show(o.ToString());

        }

        private void executeScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.ExecuteScript("alert('executeJavaScript')");
        }

        private void appendElementEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.AppendElementEventListener("su", "click", new ChromeWebBrowser.TCallBackElementEventListener(showmsg));
        }
        private void showmsg()
        {
            MessageBox.Show("element listener");
        }

        private void chromeWebBrowser1_BrowserUrlChange(object sender, UrlChangeEventArgs e)
        {
            textBox1.Text = e.Url;
        }

        private void setPopupMenuVisibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.SetPopupMenuVisible(false);
        }

        private void setPopupMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContextMenu p = new System.Windows.Forms.ContextMenu();
            MenuItem item = new MenuItem("menuItem1");
            item.Click += new EventHandler(ItemClick);
            p.MenuItems.Add(item);
            chromeWebBrowser1.SetPopupMenu(p);

        }

        private void ItemClick(object sender, EventArgs e)
        {
            textBox1.Text = "http://www.163.com";
        }

      

        delegate void NewPageListener(string url, object request);

        private void chromeWebBrowser1_BrowserNewWindow(object sender, NewWindowEventArgs e)
        {
            if (this.InvokeRequired)
            {
                NewPageListener a = new NewPageListener(NewPage);
                this.Invoke(a, new object[] { e.NewUrl, e.Request });
            }
            else
            {
                NewPage(e.NewUrl, e.Request);
            }
        }
        object aaa;
        ChromeWebBrowser browser1;
        private void NewPage(string newUrl, object req)
        {
            TabPage newPage = new TabPage(newUrl);
            tabControl1.TabPages.Add(newPage);
            tabControl1.SelectTab(newPage);
            CSharpBrowserSettings settings = new CSharpBrowserSettings();
            //settings.UserAgent = "Mozilla/5.0 (Linux; Android 4.2.1; en-us; Nexus 4 Build/JOP40D) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.166 Mobile Safari/535.19";
            settings.CachePath = @"C:\temp\caches";
            ChromeWebBrowser browser = new ChromeWebBrowser();
            browser.BrowserNewWindow += new NewWindowEventHandler(chromeWebBrowser1_BrowserNewWindow);
       
            browser.Initialize(settings);
            newPage.Controls.Add(browser);
            browser.Validate();
            browser.Dock = DockStyle.Fill;
            if (!newUrl.Contains("&"))
                browser.OpenUrl(newUrl);
            else
                browser.OpenUrl(req);
            aaa = req;
            browser1 = browser;
        }

        private void loadRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.OpenUrl(aaa);
        }

        private void addPluginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromeWebBrowser1.AddPluginPath("np-mswmp.dll");
        }

        private void documentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //CwbElement root = chromeWebBrowser1.Document.Root;
            //string value = root.ChildElements[0].ChildElements[1].ChildElements[3].Value;
            //MessageBox.Show(value);

            //DownImage("http://image.zzd.sm.cn/11681871185107069808.jpg?id=0&amp;width=510", path, "");
            flag = false;



            //SqlParameter pa = new SqlParameter();
            //pa.Direction = ParameterDirection.Output;
            //pa.ParameterName = "@param";
            //pa.Size = 11;
            //SqlParameter[] param = { new SqlParameter("@title", "sdddddddddd1"), new SqlParameter("@content", "sdddddddddd"), pa };

            //SqlHelper.ExecuteDataSetProducts("pro_addArticle", param);

            //MessageBox.Show(param[2].Value.ToString());
        }

        private void setElementValueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CwbElement root = chromeWebBrowser1.Document.Root;
            root.ChildElements[0].ChildElements[1].ChildElements[3].Value = "test value";
        }

        private void elementListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<CwbElement> buttons = chromeWebBrowser1.Document.GetElementsByTagName("input");
            MessageBox.Show(buttons.Count.ToString());
        }

        private void getElementByIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CwbElement buttons = chromeWebBrowser1.Document.GetElementById("kw1");
            MessageBox.Show(buttons.Value);
        }

        private void setInnerHtmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<CwbElement> buttons = chromeWebBrowser1.Document.GetElementsByTagName("ul");
            buttons[0].InnerHtml = "aaaaaaaa";
        }

        private void getInnerHtmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<CwbElement> buttons = chromeWebBrowser1.Document.GetElementsByTagName("ul");
            MessageBox.Show(buttons[0].InnerHtml);
        }

        private void visitAttrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = string.Empty;
            CwbElement buttons = chromeWebBrowser1.Document.GetElementById("kw1");
            foreach (KeyValuePair<string, string> item in buttons.Attributes)
            {
                value += item.Key + ":" + item.Value + ",";
            }
            MessageBox.Show(value);
        }

        private void getAttributeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = string.Empty;
            CwbElement buttons = chromeWebBrowser1.Document.GetElementById("kw1");
            value = buttons.GetAttribute("value");
            MessageBox.Show(value);
        }

        private void cookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(chromeWebBrowser1.Document.Cookie);
        }

        private void setAttributeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CwbElement buttons = chromeWebBrowser1.Document.GetElementById("kw1");
            buttons.SetAttribute("value", "test value");
        }

        private void clickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CwbElement buttons = chromeWebBrowser1.Document.GetElementById("su");
            buttons.Click();
        }

        private void appElementListenerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CwbElement buttons = chromeWebBrowser1.Document.GetElementById("su");
            buttons.AttachEventListener("click", new ChromeWebBrowser.TCallBackElementEventListener(showmsg));
        }


        public List<string> urls;
      
        List<Article> listArticle;
        int index = 0;

        bool flag;

        private void timer1_Tick(object sender, EventArgs e)
        {
            System.GC.Collect();

            timer1.Stop();

            if (!flag)
                return;

            urls = new List<string>();

            string url = "";
            listArticle = new List<Article>();
            try
            {
                var result = HttpService.Get("http://iflow.sm.cn/iflow/api/v1/channel/100?method=new&ftime=1449665473817&recoid=18168830264812026763&count=20&content_ratio=0&app=uc-iflow&no_op=0&auto=0&_tm=1449665535615&uc_param_str=dnnivebichfrmintcpgieiwidsudsv&dn=6235236332-09202b45&ni=bTkwBMKmWGK5JfTxnwf7uJ7dfGoCGAPO96q7k9ZA%2BF%2BTkw%3D%3D&ve=10.6.0.620&bi=35030&ch=&fr=android&mi=MI+4LTE&nt=2&cp=isp:%E7%A7%BB%E5%8A%A8;prov:%E5%B9%BF%E4%B8%9C;city:%E5%B9%BF%E5%B7%9E;na:%E4%B8%AD%E5%9B%BD;cc:CN;ac:&gi=bTkwBB%2BflGGJaHT8Nxg9slzX8GGHcsfb0Ym51%2FEifwH9oQ%3D%3D&ei=bTkwBNoOq1SYTiCJ8qksT%2B%2BNzXFUyeTjhw%3D%3D&wi=bTkwBC7xZmHEd%2BLYdw%3D%3D&ds=bTkwBN9gLDTznarc9qjSF78arkQq1rblSrNhNmwnVpom%2Fw%3D%3D&ud=bTkwBBsaRBKY6oROM5eeTRqbddvURlgRlh3xdc7AYHacbw%3D%3D&sv=ucrelease&sign=bTkwBAbWYLyOAfxktCSSpgV2biCKynmvr2uwyk2NgXo3XbHcB%2BYXzXAc");
                JsonData js = JsonMapper.ToObject(result);
                if (js != null)
                {


                    JsonData articles = js[0][2];
                    for (int i = 0; i < articles.Count - 1; i++)
                    {
                        url = articles[i][4].ToString();

                        if (url.Contains("m.uczzd.cn/webapp/webview/article/news.html"))
                        {

                            Article aitem = new Article();
                            aitem.Title = articles[i][2].ToString();
                            aitem.Url = url;
                            aitem.Thumbnail = articles[i][9].Count == 0 ? "" : articles[i][9][0][0].ToString();
                            aitem.Images = new List<Images>();
                            for (int j = 0; j < articles[i][10].Count; j++)
                            {
                                Images img = new Images();
                                string src = articles[i][10][j][1].ToString();
                                img.Url = src.Substring(0, src.IndexOf('?'));
                                img.Desc = articles[i][10][j][2].ToString();
                                img.FileName = GenerateNonceStr() + ".jpg";
                                img.Sort = j.ToString();
                                aitem.Images.Add(img);


                            }
                            //aitem.Tags = articles[i][11][0].ToString();
                            
                            aitem.Article_like = articles[i][42].ToString();
                            aitem.Like = articles[i][43].ToString();
                            aitem.Dislike = articles[i][44].ToString();
                            aitem.ViewCount = articles[i][45].ToString();
                            aitem.Tags = Regex.Unescape(articles[i][11].ToJson()).Replace("[", "").Replace("]", "").Replace(",", "  ").Replace("\"", " ");
                            aitem.Category = articles[i][55].ToString();
                            index++;
                            //ListViewItem lv = new ListViewItem();
                            //lv.Text = index.ToString();
                            //lv.SubItems.Add(title);
                            //lv.SubItems.Add(url);
                            //lv.SubItems.Add(category);

                            //this.listView1.Items.Add(lv);

                            //urls.Add(url);

                     

                            listArticle.Add(aitem);
                        }
                    }

                    lblCount.Text = index.ToString();
                
                }
                else
                    Log.Error(this.GetType().ToString(), "json is null");
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.Message + "   1行");
            }
            finally
            {
                if (listArticle.Count == 0)
                {
                    timer1.Start();
                }
                else
                {
                    chromeWebBrowser1.OpenUrl(listArticle[0].Url);
                    //webBrowser1.Navigate(listArticle[0].Url);
                }
            }

        }

        private void chromeWebBrowser1_BrowserDocumentCompleted(object sender, EventArgs e)
        {

            //MessageBox.Show(chromeWebBrowser1.Document.GetElementsByTagName("BODY")[0].InnerHtml);
            timer2.Start();

        }

   

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            List<CwbElement> DIVS = chromeWebBrowser1.Document.GetElementsByTagName("DIV");
            int index = 0;
            foreach (CwbElement elem in DIVS)
            {
                  if (elem.GetAttribute("class") == "img finish")
                  {
                      elem.InnerHtml = "<!--{img:"+index+"}-->";
                      index++;
                  }
            }



            #region

            Article article = listArticle[0];
            try
            {
      
                article.Content = chromeWebBrowser1.Document.GetElementById("contentShow").InnerHtml;

                string Thumbnail = article.Thumbnail;
                if (!Thumbnail.Equals(""))
                {
                    article.Thumbnail=GenerateNonceStr() + ".jpg";
                }

                if (article.Category.Equals(""))
                {
                    article.Category = "1";
                }
                else if (categoryList.ContainsKey(article.Category))
                {
                    article.Category = categoryList[article.Category];
                }
                else
                {
                    Log.Info("", "发现新的Category------------------------------" + article.Category);
                    article.Category = "1";
                }

           

                SqlParameter pa = new SqlParameter();
                pa.Direction = ParameterDirection.Output;
                pa.ParameterName = "@param";
                pa.Size = 11;
                SqlParameter[] param = { new SqlParameter("@title", article.Title.Replace("'", "")), new SqlParameter("@content", article.Content.Replace("'", "")), pa, new SqlParameter("@articlelike", article.Article_like), new SqlParameter("@dislike", article.Dislike), new SqlParameter("@supLike", article.Like), new SqlParameter("@thumbnail", article.Thumbnail), new SqlParameter("@categoryid", article.Category), new SqlParameter("@keywords", article.Tags) };
                SqlHelper.ExecuteDataSetProducts("pro_addArticle", param);


                string result = param[2].Value.ToString();
                if (result != "exists")
                {
                    if (!Thumbnail.Equals(""))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(new Uri(Thumbnail.Substring(0, Thumbnail.IndexOf('?') - 1)), path + article.Thumbnail);
                    }


                    string sql = "insert into Article_imgs values ";

                    for (int i = 0; i < article.Images.Count; i++)
                    {
                        sql += "(" + result + ",'" + article.Images[i].FileName + "','" + article.Images[i].Desc + "'," + article.Images[i].Sort + ",'www.zhangjixi.com'),";
                        StopTimeHandler stop = new StopTimeHandler(DownImage);
                        AsyncCallback callback = new AsyncCallback(onDownLoadFinish);
                        IAsyncResult asyncResult = stop.BeginInvoke(article.Images[i], callback, "--下载完成 \r\n");
                        
                    }

                    if (!sql.Equals("insert into Article_imgs values "))
                    {
                        sql = sql.Substring(0, sql.Length - 1);
                        SqlHelper.ExecteNonQuery(CommandType.Text, sql, null);
                    }

                }

                listArticle.RemoveAt(0);
                if (listArticle.Count > 0)
                {
                    //webBrowser1.Navigate(listArticle[0].Url);
                    chromeWebBrowser1.OpenUrl(listArticle[0].Url);
                }
                else
                    timer1.Start();

            }
            catch (Exception ex)
            {
                Log.Error("", ex.Message + "3333333333333" + article.ToString());
            }
            #endregion


        }

        //在网站根目录下创建日志目录
        public static string path = "E:\\images\\";

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="Url">Url地址</param>
        /// <param name="Path">保存路径</param>
        public void DownImage(Images img)
        {
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(img.Url);
                request.Timeout = 2000;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.1.4322)"; // 
                request.AllowAutoRedirect = true;//是否允许302
                WebResponse response = request.GetResponse();
                //文件流获取图片操作
                Stream reader = response.GetResponseStream();
                string path_img = path + img.FileName;        //图片路径命名 
                FileStream writer = new FileStream(path_img, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] buff = new byte[512];
                int c = 0;                                           //实际读取的字节数   
                while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                {
                    writer.Write(buff, 0, c);
                }
                //释放资源
                writer.Close();
                writer.Dispose();
                reader.Close();
                reader.Dispose();
                response.Close();

            }
            catch (Exception ex)
            {
                Log.Error("下载图片", ex.Message);
            }

        }

        /**
       * 生成随机串，随机串包含字母或数字
       * @return 随机串
       */
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Log.Info("", webBrowser1.Document.GetElementById("contentShow").InnerHtml);


            #region

            try
            {
                Article article = listArticle[0];
                article.Content = webBrowser1.Document.GetElementById("contentShow").InnerHtml;

                SqlParameter pa = new SqlParameter();
                pa.Direction = ParameterDirection.Output;
                pa.ParameterName = "@param";
                pa.Size = 11;
                SqlParameter[] param = { new SqlParameter("@title", article.Title), new SqlParameter("@content", article.Content), pa };

                SqlHelper.ExecuteDataSetProducts("pro_addArticle", param);


                string result = param[2].Value.ToString();
                if (result == "exists")
                    Log.Error("", "-----------------exists-------------------------");
                else
                {
                    string sql = "insert into Article_imgs values ";

                    for (int i = 0; i < article.Images.Count; i++)
                    {
                        sql += "(" + result + ",'" + article.Images[i].FileName + "','" + article.Images[i].Desc + "'," + article.Images[i].Sort+ ",'www.zhangjixi.com'),";
                        StopTimeHandler stop = new StopTimeHandler(DownImage);
                        AsyncCallback callback = new AsyncCallback(onDownLoadFinish);
                        IAsyncResult asyncResult = stop.BeginInvoke(article.Images[i], callback, "--下载完成 \r\n");
                    }

                    if (!sql.Equals("insert into Article_imgs values "))
                    {
                        sql = sql.Substring(0, sql.Length - 1);
                        SqlHelper.ExecteNonQuery(CommandType.Text, sql, null);
                    }
                }

                listArticle.RemoveAt(0);
                if (listArticle.Count > 0)
                {
                    webBrowser1.Navigate(listArticle[0].Url);
                }
                else
                    timer1.Start();

            }
            catch (Exception ex)
            {
                Log.Error("", ex.Message+"33333333333333333333");
            }
            #endregion

        }

        public delegate void StopTimeHandler(Images img);
        /// <summary>
        /// 下载成功
        /// </summary>
        /// <param name="asyncresult"></param>
        private void onDownLoadFinish(IAsyncResult asyncresult)
        {
            AsyncResult result = (AsyncResult)asyncresult;
            StopTimeHandler del = (StopTimeHandler)result.AsyncDelegate;

            //listArticle[0].Images.RemoveAt(0);

            //if (listArticle[0].Images.Count == 0)
            //{
            //    listArticle.RemoveAt(0);

            //    if (listArticle.Count > 0)
            //    {
            //        //chromeWebBrowser1.OpenUrl(urls[0]);
            //        webBrowser1.Navigate(listArticle[0].Url);
            //    }
            //    else
            //        timer1.Start();
            //}

            //string data = (string)result.AsyncState;
            //string name = del.EndInvoke(result);
            //TextResultChange(name + data);
            //_downLoadPicCount++;
            //_downingPicCount--;
            //TipDownLoad();
        }



    }
}
