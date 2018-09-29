using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace BentaiDataBase
{
    public partial class Image_Exporter : UserControl
    {
        public Image_Exporter()
        {
            InitializeComponent();

            tagLabels = new Label[]
            {
                YuriLabel, KemonomimiLabel, NonHLabel, MasturbationLabel, TentaclesLabel,
                SoloLabel, ToysLabel, BigBreastsLabel, BoatLabel, LoliLabel,
                BlowJobLabel, AnalLabel, TouhouLabel, AhegaoLabel, FavoritesLabel
            };
        }

        #region Singleton
        private static Image_Exporter _instance;
        public static Image_Exporter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Image_Exporter();
                }
                return _instance;
            }
        }
        #endregion


        #region SearchButtonTextUpdates

        private sbyte LoliCheckState, SoloCheckState, MasturbationCheckState, BigBreastCheckState, YuriCheckState, NonHCheckState,
        KemonomimicheckState, BlowJobCheckState, AnalCheckState, ToysCheckState, TentaclesCheckState, BoatCheckState, TouhouCheckState,
        AhegaoCheckState, FavoritesCheckState;

        private readonly string[] buttonStates = new string[]
        {
            "Any",
            "Yes",
            "No"
        };

        private void changeLabelText(Label associatedLabel, ref sbyte checkState)
        {
            /*
             * {0, "No" },
             * {1, "Any" },
             * {2, "Yes" }
             */
            checkState++;
            // The current label's state
            switch (checkState)
            {
                case 0:
                    associatedLabel.Text = buttonStates[checkState];
                    associatedLabel.ForeColor = Color.Empty;
                    break;

                case 1:
                    associatedLabel.Text = buttonStates[checkState];
                    associatedLabel.ForeColor = Color.Green;
                    break;

                case 2:
                    associatedLabel.Text = buttonStates[checkState];
                    associatedLabel.ForeColor = Color.Red;
                    // CheckState will be incremented by one, but we still want to trigger case 0
                    checkState = -1;
                    break;
            }
        }

        private void LoliButton_Click(object sender, EventArgs e) => changeLabelText(LoliLabel, ref LoliCheckState);

        private void SoloButton_Click(object sender, EventArgs e) => changeLabelText(SoloLabel, ref SoloCheckState);

        private void MasturbationButton_Click(object sender, EventArgs e) => changeLabelText(MasturbationLabel, ref MasturbationCheckState);

        private void BigBreastsButton_Click(object sender, EventArgs e) => changeLabelText(BigBreastsLabel, ref BigBreastCheckState);

        private void YuriButton_Click(object sender, EventArgs e) => changeLabelText(YuriLabel, ref YuriCheckState);

        private void NonHButton_Click(object sender, EventArgs e) => changeLabelText(NonHLabel, ref NonHCheckState);

        private void KemonomimiButton_Click(object sender, EventArgs e) => changeLabelText(KemonomimiLabel, ref KemonomimicheckState);

        private void BlowJobButton_Click(object sender, EventArgs e) => changeLabelText(BlowJobLabel, ref BlowJobCheckState);

        private void AnalButton_Click(object sender, EventArgs e) => changeLabelText(AnalLabel, ref AnalCheckState);

        private void ToysButton_Click(object sender, EventArgs e) => changeLabelText(ToysLabel, ref ToysCheckState);

        private void TentaclesButton_Click(object sender, EventArgs e) => changeLabelText(TentaclesLabel, ref TentaclesCheckState);

        private void BoatButton_Click(object sender, EventArgs e) => changeLabelText(BoatLabel, ref BoatCheckState);

        private void TouhouButton_Click(object sender, EventArgs e) => changeLabelText(TouhouLabel, ref TouhouCheckState);

        private void AhegaoButton_Click(object sender, EventArgs e) => changeLabelText(AhegaoLabel, ref AhegaoCheckState);

        private void FavoritesButton_Click(object sender, EventArgs e) => changeLabelText(FavoritesLabel, ref FavoritesCheckState);

        #endregion

        Label[] tagLabels;

        private readonly string[] DataBaseNames = new string[]
        {
            "yuri", "loli", "kemonomimi", "nonh", "masturbation", "tentacle", "solo", "toys", "bigBreast",
            "boat", "blowJob", "anal", "touhou", "ahegao"
        };

        private void ExportButton_Click(object sender, EventArgs e)
        {
            {
                string sqlCommandString = "select imageId from imageData";

                Dictionary<string, int> selectedSearch = new Dictionary<string, int>();

                string sqlFieldName;
                for (int i = 0; i < DataBaseNames.Length; i++)
                {
                    sqlFieldName = DataBaseNames[i];
                    selectedSearch.Add(sqlFieldName, tagLabels[i].Text == "Yes" ? 1 : tagLabels[i].Text == "No" ? 0 : 2);
                }

                if (selectedSearch.Count != 0)
                {
                    sqlCommandString += " where";
                    foreach (KeyValuePair<string, int> searchCriteria in selectedSearch)
                    {
                        if (searchCriteria.Value != 2)
                        {
                            sqlCommandString += $" {searchCriteria.Key} = {searchCriteria.Value} and";
                        }
                    }

                    int lastAndIndex = sqlCommandString.LastIndexOf("and");
                    if (lastAndIndex != -1)
                    {
                        sqlCommandString = sqlCommandString.Remove(lastAndIndex, "and".Length);
                    }
                }
                if (sqlCommandString == "select imageId from imageData where")
                {
                    sqlCommandString = "select imageId from imageData";
                }

                List<int> imageIds = new List<int>();
                using (SQLiteConnection sqlConnection = new SQLiteConnection(Globals.dataBaseString))
                {
                    sqlConnection.Open();
                    using (SQLiteCommand sqlCommand = new SQLiteCommand(sqlCommandString, sqlConnection))
                    using (SQLiteDataReader sqlReader = sqlCommand.ExecuteReader())
                    {

                        while (sqlReader.Read())
                        {
                            int imageId = (int)sqlReader["imageId"];
                            imageIds.Add(imageId);
                        }
                    }
                }

                List<Bitmap> imagesToExport = new List<Bitmap>();
                for (int i = 0; i < imageIds.Count; i++)
                {
                    imagesToExport.Add(BentaiDataBaseHandler.GetBitmap(imageIds[i]));
                }

                if (imagesToExport.Count == 0)
                {
                    MessageBox.Show("No images matched the query");
                    return;
                }

                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Select the folder to export the files";
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        Directory.CreateDirectory(fbd.SelectedPath + "/BentaiImages");
                        for (int i = 0; i < imagesToExport.Count; i++)
                        {
                            imagesToExport[i].Save($"{fbd.SelectedPath}/BentaiImages/{i}.png");
                        }

                        string file = "File";
                        if (imagesToExport.Count > 1)
                            file = "Files";

                        MessageBox.Show($@"{imagesToExport.Count} {file} saved under {fbd.SelectedPath}\BentaiImages");
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }
}