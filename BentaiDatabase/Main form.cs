using System;
using System.Windows.Forms;


namespace BentaiDataBase
{
    public partial class Loli_in_form : Form
    {
        private static Loli_in_form _instance;
        public static Loli_in_form Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Loli_in_form();  
                }
                return _instance;
            }
        }

        public Loli_in_form()
        {
            InitializeComponent();
        }

        private void Loli_in_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (panel.Controls.Contains(ViewDatabase.Instance))
            {
                ViewDatabase.Instance.ViewDatabase_Leave(sender, e);
            }
            else
            {
                if (Type.GetType("PopulateDataBase.PopulateDataBase") != null)
                {
                    if (PopulateDataBase.Instance.picsToBeMoved.Count != 0)
                    {

                        PopulateDataBase.Instance.SaveFiles();
                    }
                }
            }
        }

        private void Loli_in_form_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            if (Type.GetType("PopulateDataBase") != null)
            {
                PopulateDataBase.Instance.sqlConnection.Close();
            }
            if (Type.GetType("ViewDatabase") != null)
            {
                ViewDatabase.Instance.sqlConnection.Close();
            }
            if (Type.GetType("DataBaseStatistics") != null)
            {
                DataBaseStatistics.Instance.sqlConnection.Close();
            }
        }

        private void Loli_in_form_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    foreach (var check in PopulateDataBase.Instance.tagCheckBoxes)
                    {
                        if (check.Checked)
                        {
                            PopulateDataBase.Instance.SubmitTags_Click(sender, e);
                        }
                    }
                    break;
                case Keys.Delete:
                    PopulateDataBase.Instance.button1_Click(sender, e);
                    break;
                case Keys.D1:
                    PopulateDataBase.Instance.RevertLoliCheck();
                    break;
                case Keys.D2:
                    PopulateDataBase.Instance.RevertSoloCheck();
                    break;
                case Keys.D3:
                    PopulateDataBase.Instance.RevertMasturbationCheck();
                    break;
                case Keys.D4:
                    PopulateDataBase.Instance.RevertBigBreastCheck();
                    break;
                case Keys.D5:
                    PopulateDataBase.Instance.RevertYuriCheck();
                    break;
                case Keys.D6:
                    PopulateDataBase.Instance.RevertNonHCheck();
                    break;
                case Keys.D7:
                    PopulateDataBase.Instance.RevertKemonomimiCheck();
                    break;
                case Keys.D8:
                    PopulateDataBase.Instance.RevertBlowJobCheck();
                    break;
                case Keys.D9:
                    PopulateDataBase.Instance.RevertAnalCheck();
                    break;
                case Keys.D0:
                    PopulateDataBase.Instance.RevertToysCheck();
                    break;
            }
        }

        public void EmptyPanel()
        {
            this.panel.Controls.Clear();
        }

        private void AddImagesButton_Click(object sender, EventArgs e)
        {
            if (!panel.Controls.Contains(PopulateDataBase.Instance))
            {
                panel.Controls.Add(PopulateDataBase.Instance);
                PopulateDataBase.Instance.Dock = DockStyle.Fill;
                PopulateDataBase.Instance.BringToFront();
            }
            else
            {
                PopulateDataBase.Instance.BringToFront();
            }
        }
        
        private void ViewImagesButton_Click(object sender, EventArgs e)
        {
            if (!panel.Controls.Contains(ViewDatabase.Instance))
            {
                panel.Controls.Add(ViewDatabase.Instance);
                ViewDatabase.Instance.Dock = DockStyle.Fill;
                ViewDatabase.Instance.BringToFront();
            }
            else
            {
                ViewDatabase.Instance.BringToFront();
            }
        }

        private void DataStatisticsButton_Click(object sender, EventArgs e)
        {
            if (!panel.Controls.Contains(DataBaseStatistics.Instance))
            {
                panel.Controls.Add(DataBaseStatistics.Instance);
                DataBaseStatistics.Instance.Dock = DockStyle.Fill;
                DataBaseStatistics.Instance.BringToFront();
            }
            else
            {
                DataBaseStatistics.Instance.BringToFront();
            }
        }
    }
}
