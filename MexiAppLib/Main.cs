using MexiAppLib.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MexiAppLib
{
    public partial class Main : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn
            (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        #region AppListData
        //Title
        string[] titleData = { "SMPbeta Launcher", "PC Lister", "Test2" };

        //Version
        string[] versionData = { "1.2.42", "ALPHA 1.0", "TestVersion2" };

        //Image
        string[] imageData = { "" };

        //OS
        string[] osData = { "4", "1", "7"};

        //Number of applications
        int NoA = 3;
        #endregion

        public Main()
        {
            InitializeComponent();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
            appFlowPanel.VerticalScroll.Visible = false;
            populateAppList();
            populateSidebar();

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(10);

            var timer = new System.Threading.Timer((e) =>
            {
                doUpdate();
            }, null, startTimeSpan, periodTimeSpan);
    }

        private void topBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void doUpdate()
        {
            //Gets raw data from the project
            System.Net.WebClient wc = new System.Net.WebClient();

            //Using this in a try/catch so the program won't crash if there is no internet.
            byte[] raw = wc.DownloadData("https://raw.githubusercontent.com/MexiMoo/MexiAppLib/master/MexiAppLib/MexiAppLib.csproj");

            string webData = System.Text.Encoding.UTF8.GetString(raw);

            //Extracts one line
            string GetLine(string text, int lineNo)
            {
                string[] lines = text.Replace("\r", "").Split('\n');
                return lines.Length >= lineNo ? lines[lineNo - 1] : null;
            }

            //Filters out the junk
            int startPos = webData.LastIndexOf("    <Version>") + "    <Version>".Length;
            int length = webData.IndexOf("</Version>") - startPos;
            string onlineAppVersion = webData.Substring(startPos, length);

            //Will ry to receive data from the app that is stored
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"MRO");
            if (key != null)
            {
                var AppIsInstalled = key.GetValue("SMPbetaInstalled");
                var AppVersion = key.GetValue("Version");
                key.Close();

                //Removes unused revision number from version
                string appVersionLocalRaw = (string)AppVersion.ToString();
                string appVersionLocal = appVersionLocalRaw.Remove(appVersionLocalRaw.Length - 2);

                //Compares the versions
                if (appVersionLocal != onlineAppVersion)
                {
                    updatePanel.Visible = true;
                }
                else
                {
                    //Do nothing
                }
            }
        }

        private void updateAppBtn_Click(object sender, EventArgs e)
        {
            updatePanel.Visible = false;
            wSpacer3.Visible = false;
            sdbr.Visible = false;
            pageControl.SelectedTab = UpdateSCRN;
            wSpacer1.Visible = false;
            wSpacer4.Visible = false;
            pageControl.Size = new System.Drawing.Size(2210, 1506);
        }

        private void populateAppList()
        {
            AppList[] appLists = new AppList[NoA];
            for(int i = 0; i < appLists.Length; i++)
            {
                appLists[i] = new AppList();
                //appLists[i].Image = Resources.SMPbetaLauncher;
                appLists[i].Title = titleData[i];
                appLists[i].Version = versionData[i];

                #region OSpick
                if (osData[i] == "1")
                {
                    //W
                    //appLists[i].OS1 = true;
                    //appLists[i].OS2 = false;
                    //appLists[i].OS3 = false;
                }
                else if (osData[i] == "2")
                {
                    //L
                    //appLists[i].OS1 = false;
                    //appLists[i].OS2 = true;
                    //appLists[i].OS3 = false;
                }
                else if (osData[i] == "3")
                {
                    //M
                    //appLists[i].OS1 = false;
                    //appLists[i].OS2 = false;
                    //appLists[i].OS3 = true;
                }
                else if (osData[i] == "4")
                {
                    //W,L
                    //appLists[i].OS1 = true;
                    //appLists[i].OS2 = true;
                    //appLists[i].OS3 = false;
                }
                else if (osData[i] == "5")
                {
                    //W,M
                    //appLists[i].OS1 = true;
                    //appLists[i].OS2 = false;
                    //appLists[i].OS3 = true;
                }
                else if (osData[i] == "6")
                {
                    //L,M
                    //appLists[i].OS1 = false;
                    //appLists[i].OS2 = true;
                    //appLists[i].OS3 = true;
                }
                else
                {
                    //W,L,M
                    //appLists[i].OS1 = true;
                    //appLists[i].OS2 = true;
                    //appLists[i].OS3 = true;
                }
                    #endregion

                if (appFlowPanel.Controls.Count < 0)
                {
                    appFlowPanel.Controls.Clear();
                }
                else
                {
                    appFlowPanel.Controls.Add(appLists[i]);
                }  
            }
        }

        private void populateSidebar()
        {
            SideMenu[] sideBar = new SideMenu[NoA];
            for (int i = 0; i < sideBar.Length; i++)
            {
                sideBar[i] = new SideMenu();
                sideBar[i].Text = titleData[i];
                //sideBar[i].Image = versionData[i];

                if (sidebarFlowPanel.Controls.Count < 0)
                {
                    sidebarFlowPanel.Controls.Clear();
                }
                else
                {
                    sidebarFlowPanel.Controls.Add(sideBar[i]);
                }
            }
        }

        private void label2_MouseHover(object sender, EventArgs e)
        {
            updateAppClose.ForeColor = Color.Red;
        }

        private void updateAppClose_MouseLeave(object sender, EventArgs e)
        {
            updateAppClose.ForeColor = Color.White;
        }

        private void updateAppClose_MouseClick(object sender, MouseEventArgs e)
        {
            updatePanel.Visible = false;
            wSpacer3.Visible = false;
            sdbr.Visible = false;
            pageControl.SelectedTab = Page2;
        }
    }
}
