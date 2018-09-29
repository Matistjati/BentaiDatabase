using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BentaiDataBase
{
    public partial class ViewDatabase : UserControl
    {
        #region Singleton
        private static ViewDatabase _instance;
        public static ViewDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ViewDatabase();
                }
                return _instance;
            }
        }
        #endregion

        private readonly string[] buttonStates = new string[]
        {
            "Any",
            "Yes",
            "No"
        };

        private sbyte LoliCheckState, SoloCheckState, MasturbationCheckState, BigBreastCheckState, YuriCheckState, NonHCheckState,
        KemonomimicheckState, BlowJobCheckState, AnalCheckState, ToysCheckState, TentaclesCheckState, BoatCheckState, TouhouCheckState,
        AhegaoCheckState, FavoritesCheckState;

        private List<int> imageIds = new List<int>();
        private int currentImage;
        private List<int> imagesToDelete = new List<int>();

        public Label[] tagLabels;
        public CheckBox[] tagCheckBoxes;

        private Dictionary<int, Dictionary<string, int>> imageTags = new Dictionary<int, Dictionary<string, int>>();
        private Dictionary<int, Dictionary<string, int>> initialImageTags = new Dictionary<int, Dictionary<string, int>>();

        private void LoadNewPic(int pictureId)
        {
            using (var sourceImage = BentaiDataBaseHandler.GetImage(pictureId))
            {
                var targetImage = new Bitmap(sourceImage.Width, sourceImage.Height,
                  System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (var canvas = Graphics.FromImage(targetImage))
                {
                    canvas.DrawImageUnscaled(sourceImage, 0, 0);
                }
                hentaiPicBox.Image = targetImage;
            }
        }

        public ViewDatabase()
        {
            InitializeComponent();

            tagLabels = new Label[]
            {
                YuriLabel, KemonomimiLabel, NonHLabel, MasturbationLabel, TentaclesLabel,
                SoloLabel, ToysLabel, BigBreastsLabel, BoatLabel, LoliLabel,
                BlowJobLabel, AnalLabel, TouhouLabel, AhegaoLabel, FavoritesLabel
            };

            tagCheckBoxes = new CheckBox[]
            {
                YuriCheck, KemonomimiCheck, NonhCheck, MasturbationCheck, TentacleCheck, SoloCheck,
                ToysCheck, bigbreastCheck, BoatCheck, LoliCheck, BlowJobCheck, AnalCheck,
                TouhouCheck, AhegaoCheck, FavoriteCheck
            };
        }

        #region SearchButtonTextUpdates

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


        // TODO fix this shit
        private void updateCheckboxes()
        {
            if (imageIds.Count == 0)
            {
                return;
            }

            try
            {
                for (int i = 0; i < tagCheckBoxes.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            YuriCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["yuri"]);
                            break;
                        case 1:
                            KemonomimiCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["kemonomimi"]);
                            break;
                        case 2:
                            NonhCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["nonh"]);
                            break;
                        case 3:
                            MasturbationCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["masturbation"]);
                            break;
                        case 4:
                            TentacleCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["tentacle"]);
                            break;
                        case 5:
                            SoloCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["solo"]);
                            break;
                        case 6:
                            ToysCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["toys"]);
                            break;
                        case 7:
                            bigbreastCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["bigBreast"]);
                            break;
                        case 8:
                            BoatCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["boat"]);
                            break;
                        case 9:
                            LoliCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["loli"]);
                            break;
                        case 10:
                            BlowJobCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["blowJob"]);
                            break;
                        case 11:
                            AnalCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["anal"]);
                            break;
                        case 12:
                            TouhouCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["touhou"]);
                            break;
                        case 13:
                            AhegaoCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["ahegao"]);
                            break;
                        case 14:
                            FavoriteCheck.Checked = Convert.ToBoolean(imageTags[imageIds[currentImage]]["favorite"]);
                            break;
                        default:
                            throw new IndexOutOfRangeException("there are more labels in the label list than accounted for");
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                ViewDatabase_Load();
                updateCheckboxes();
            }
        }

        private readonly string[] DataBaseNames = new string[]
        {
            "yuri", "loli", "kemonomimi", "nonh", "masturbation", "tentacle", "solo", "toys", "bigBreast",
            "boat", "blowJob", "anal", "touhou", "ahegao"
        };

        private void SearchButton_Click(object sender, EventArgs e)
        {
            imageIds.Clear();
            currentImage = 0;

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
                if (sqlCommandString == "select imageId from imageData where")
                {
                    sqlCommandString = "select imageId from imageData";
                }

                int lastAndIndex = sqlCommandString.LastIndexOf("and");
                if (lastAndIndex != -1)
                {
                    sqlCommandString = sqlCommandString.Remove(lastAndIndex, "and".Length);
                }
            }

            
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

                    if (imageIds.Count != 0)
                    {
                        LoadNewPic(imageIds[currentImage]);
                        CurrentImageLabel.Text = $"Image {currentImage + 1}/{imageIds.Count}";
                    }
                    SearchResultLabel.Text = $"{imageIds.Count} Results";
                }
            }

            updateCheckboxes();
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            try
            {
                currentImage--;
                LoadNewPic(imageIds[currentImage]);
            }
            catch (ArgumentOutOfRangeException) { currentImage++; }

            if (imageIds.Count != 0)
            {
                CurrentImageLabel.Text = $"Image {currentImage + 1}/{imageIds.Count}";
            }

            updateCheckboxes();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            try
            {
                currentImage++;
                LoadNewPic(imageIds[currentImage]);
            }
            catch (ArgumentOutOfRangeException) { currentImage--; }

            if (imageIds.Count != 0)
            {
                CurrentImageLabel.Text = $"Image {currentImage + 1}/{imageIds.Count}";
            }

            updateCheckboxes();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            imagesToDelete.Add(imageIds[currentImage]);
        }

        public void ViewDatabase_Leave(object sender, EventArgs e)
        {
            if (imageTags.Count != 0)
            {
                using (SQLiteConnection sqlConnection = new SQLiteConnection(Globals.dataBaseString))
                {
                    sqlConnection.Open();
                    using (SQLiteTransaction transaction = sqlConnection.BeginTransaction())
                    {
                        foreach (KeyValuePair<int, Dictionary<string, int>> imageId in imageTags)
                        {
                            foreach (KeyValuePair<string, int> tag in imageId.Value)
                            {
                                if (initialImageTags[imageId.Key][tag.Key] != tag.Value)
                                {
                                    string sqlCommandString = $"UPDATE imageData SET {tag.Key} = 1 WHERE imageId = {imageId.Key}";

                                    SQLiteCommand sqlCommand = new SQLiteCommand(sqlCommandString, sqlConnection);
                                    sqlCommand.ExecuteNonQuery();
                                }
                            }
                        }
                        transaction.Commit();
                    }
                }
            }

            // Deleting all the stuff the user wanted to delete
            if (imagesToDelete.Count != 0)
            {
                using (SQLiteConnection sqlConnection = new SQLiteConnection(Globals.dataBaseString))
                {
                    sqlConnection.Open();
                    using (SQLiteTransaction transaction = sqlConnection.BeginTransaction())
                    {
                        foreach (int imageId in imagesToDelete)
                        {
                            using (SQLiteCommand sqlCommand = new SQLiteCommand(sqlConnection))
                            {
                                sqlCommand.CommandText = $"DELETE from imageData where imageId = {imageId}";
                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }
            }

            ResetSearchButton_Click(sender, e);
        }

        private void ResetSearchButton_Click(object sender, EventArgs e)
        {
            // We do not need to dispose any images as loadnewpic duplicates the image
            imagesToDelete.Clear();
            imageTags.Clear();
            initialImageTags.Clear();

            foreach (CheckBox tagCheckBox in tagCheckBoxes)
            {
                tagCheckBox.Checked = false;
            }
        }

        private void ToggleTheBoxes()
        {
            foreach (CheckBox tagCheckBox in tagCheckBoxes)
            {
                tagCheckBox.Enabled = !tagCheckBox.Enabled;
                tagCheckBox.Visible = !tagCheckBox.Visible;
            }
        }

        private void ShowTagsButton_click(object sender, EventArgs e)
        {
            {
                if (hentaiPicBox.Width == 600)
                {
                    for (int i = 0; i < 84; i++)
                    {
                        hentaiPicBox.Width -= 1;
                        Point newLocation = new Point(hentaiPicBox.Location.X + 1, hentaiPicBox.Location.Y);
                        hentaiPicBox.Location = newLocation;
                        hentaiPicBox.Refresh();
                    }
                    ToggleTheBoxes();
                }

                else if (hentaiPicBox.Width == 600 - 84)
                {
                    ToggleTheBoxes();

                    for (int i = 0; i < 84; i++)
                    {
                        hentaiPicBox.Width += 1;
                        Point newLocation = new Point(hentaiPicBox.Location.X - 1, hentaiPicBox.Location.Y);
                        hentaiPicBox.Location = newLocation;
                        hentaiPicBox.Refresh();
                    }
                }
            }
        }

        private const string SelectAllTags = "select imageId, yuri, kemonomimi, nonh, masturbation, tentacle, solo, toys, bigBreast, boat, loli, blowJob, anal, touhou, ahegao, favorite from imageData";

        public void ViewDatabase_Load()
        {
            using (SQLiteConnection sqlConnection = new SQLiteConnection(Globals.dataBaseString))
            using (SQLiteCommand sqlCommand = new SQLiteCommand(SelectAllTags, sqlConnection))
            {
                sqlConnection.Open();
                using (SQLiteDataReader sqlReader = sqlCommand.ExecuteReader())
                {
                    sqlCommand.CommandText = SelectAllTags;
                    while (sqlReader.Read())
                    {
                        Dictionary<string, int> rowValues = new Dictionary<string, int>
                        {
                            { "yuri", (int)sqlReader["yuri"] },
                            { "kemonomimi", (int)sqlReader["kemonomimi"] },
                            { "nonh", (int)sqlReader["nonh"] },
                            { "masturbation", (int)sqlReader["masturbation"] },
                            { "tentacle", (int)sqlReader["tentacle"] },
                            { "solo", (int)sqlReader["solo"] },
                            { "toys", (int)sqlReader["toys"] },
                            { "bigBreast", (int)sqlReader["bigBreast"] },
                            { "boat", (int)sqlReader["boat"] },
                            { "loli", (int)sqlReader["loli"] },
                            { "blowJob", (int)sqlReader["blowJob"] },
                            { "anal", (int)sqlReader["anal"] },
                            { "touhou", (int)sqlReader["touhou"] },
                            { "ahegao", (int)sqlReader["ahegao"] },
                            { "favorite", (int)sqlReader["favorite"] }
                        };
                        imageTags.Add((int)sqlReader["imageId"], rowValues);
                        initialImageTags.Add((int)sqlReader["imageId"], rowValues.ToDictionary(entry => entry.Key,
                                                       entry => entry.Value));
                    }
                }
            }

        }

        private void ViewDatabase_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    NextButton_Click(sender, e);
                    break;
                case Keys.Left:
                    PreviousButton_Click(sender, e);
                    break;
            }
        }

        private readonly Dictionary<string, string> displayTextToVariable = new Dictionary<string, string>()
        {
            { "Loli", "loli"},
            { "Solo", "solo"},
            { "Masturbation", "masturbation"},
            { "Big Breasts", "bigBreast"},
            { "Yuri", "yuri" },
            { "Non-h", "nonh"},
            { "Kemonomimi", "kemonomimi"},
            { "BlowJob", "blowJob"},
            { "Anal", "anal"},
            { "Toys", "toys"},
            { "Tentacles", "tentacle"},
            { "Boat", "boat"},
            { "Touhou", "touhou"},
            { "Ahegao", "ahegao"},
            { "Favorite", "favorite"},
        };
        private void CheckBoxChanged(object sender, EventArgs e)
        {
            CheckBox check = sender as CheckBox;
            imageTags[imageIds[currentImage]][displayTextToVariable[check.Text]] = Convert.ToInt32(check.Checked);
        }
    }
}
