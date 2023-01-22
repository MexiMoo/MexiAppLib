using Microsoft.VisualBasic.ApplicationServices;
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
    public partial class SideMenu : UserControl
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

        public SideMenu()
        {
            InitializeComponent();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
        }

        private string _text;
        //private Image _image;

        [Category("Sidebar Properties")]
        public string Text
        {
            get { return _text; }
            set { _text = value; Title.Text = value; }
        }

        private void SideMenu_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(84, 84, 84);
        }

        private void SideMenu_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(64, 64, 64);
        }

        //[Category("Sidebar Properties")]
        //public Image Image
        //{
        //get { return _image; }
        //set { _image = value; ImageFrame.Image = value; }
        //}
    }
}