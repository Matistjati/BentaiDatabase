﻿using System;
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
            if (UserControlPanel.Controls.Contains(ViewDatabase.Instance))
            {
                ViewDatabase.Instance.ViewDatabase_Leave(sender, e);
            }
            else
            {
                if (Type.GetType("PopulateDataBase.PopulateDataBase") != null)
                {
                    if (PopulateDataBase.Instance.imageData.Count != 0)
                    {
                        PopulateDataBase.Instance.SaveFiles();
                    }
                }
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
            this.UserControlPanel.Controls.Clear();
        }

        private void AddImagesButton_Click(object sender, EventArgs e)
        {
            if (!UserControlPanel.Controls.Contains(PopulateDataBase.Instance))
            {
                UserControlPanel.Controls.Add(PopulateDataBase.Instance);
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
            if (!UserControlPanel.Controls.Contains(ViewDatabase.Instance))
            {
                UserControlPanel.Controls.Add(ViewDatabase.Instance);
                ViewDatabase.Instance.Dock = DockStyle.Fill;
                ViewDatabase.Instance.BringToFront();
                ViewDatabase.Instance.ViewDatabase_Load();
            }
            else
            {
                ViewDatabase.Instance.BringToFront();
            }
        }

        private void DataStatisticsButton_Click(object sender, EventArgs e)
        {
            if (!UserControlPanel.Controls.Contains(DataBaseStatistics.Instance))
            {
                UserControlPanel.Controls.Add(DataBaseStatistics.Instance);
                DataBaseStatistics.Instance.Dock = DockStyle.Fill;
                DataBaseStatistics.Instance.BringToFront();
            }
            else
            {
                DataBaseStatistics.Instance.BringToFront();
            }
        }

        private void ImageExporterButton_Click(object sender, EventArgs e)
        {
            if (!UserControlPanel.Controls.Contains(Image_Exporter.Instance))
            {
                UserControlPanel.Controls.Add(Image_Exporter.Instance);
                Image_Exporter.Instance.Dock = DockStyle.Fill;
                Image_Exporter.Instance.BringToFront();
            }
            else
            {
                Image_Exporter.Instance.BringToFront();
            }
        }
    }
}
