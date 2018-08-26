using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BentaiDataBase
{
    public partial class Front_page : UserControl
    {
        #region Singleton
        private static Front_page _instance;
        public static Front_page Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Front_page();
                }
                return _instance;
            }
        }
        #endregion

        public Front_page()
        {
            InitializeComponent();
        }

        private void GithubButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Matistjati/BentaiDatabase");
        }
    }
}
