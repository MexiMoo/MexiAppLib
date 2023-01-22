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

namespace MexiAppLib
{
    public partial class AppList : UserControl
    {
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

        public AppList()
        {
            InitializeComponent();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
        }

        private string _title;
        private string _version;
        private Image _image;
        private bool _os1;
        private bool _os2;
        private bool _os3;

        [Category("AppList Properties")]
        public string Title
        {
            get { return _title; }
            set { _title = value; applicationName.Text = value; }
        }

        [Category("AppList Properties")]
        public string Version
        {
            get { return _version; }
            set { _version = value; applicationVersion.Text = value; }
        }

        //[Category("AppList Properties")]
        //public Image Image
        //{
            //get { return _image; }
            //set { _image = value; contentImage.Image = value; }
        //}

        //[Category("AppList OS Properties")]
        //public string OS1
        //{
            //get { return _os1; }
            //set { _os1 = value; os1.Visible = value; }
        //}

        //[Category("AppList OS Properties")]
        //public string OS2
        //{
            //get { return _os2; }
            //set { _os2 = value; os2.Visible = value; }
        //}

        //[Category("AppList OS Properties")]
        //public string OS3
        //{
            //get { return _os3; }
            //set { _os3 = value; os3.Visible = value; }
        //}
    }
}
