using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

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

        private Dictionary<int, string> buttonStates = new Dictionary<int, string>()
        {
            {0, "No" },
            {1, "Any" },
            {2, "Yes" }
        };

        private int LoliCheckState = 1;
        private int SoloCheckState = 1;
        private int MasturbationCheckState = 1;
        private int BigBreastCheckState = 1;
        private int YuriCheckState = 1;
        private int NonHCheckState = 1;
        private int KemonomimicheckState = 1;
        private int BlowJobCheckState = 1;
        private int AnalCheckState = 1;
        private int ToysCheckState = 1;
        private int TentaclesCheckState = 1;
        private int BoatCheckState = 1;
        private int TouhouCheckState = 1;
        private int AhegaoCheckState = 1;
        private int FavoritesCheckState = 1;

        private string scriptDirectory;
        private List<int> imageIds = new List<int>();
        private int currentImage;
        private List<int> imagesToDelete = new List<int>();
        public Label[] tagLabels;
        public CheckBox[] tagCheckBoxes;

        public SQLiteConnection sqlConnection;
        private Dictionary<int, Dictionary<string, int>> imageTags = new Dictionary<int, Dictionary<string, int>>();
        private Dictionary<int, Dictionary<string, int>> initialImageTags = new Dictionary<int, Dictionary<string, int>>();

        private void LoadNewPic(int pictureId)
        {
            string imageType = TestPicId(pictureId);
            if (!string.IsNullOrEmpty(imageType))
            {
                using (var sourceImage = Image.FromFile(Path.Combine(scriptDirectory, $@"Images\{pictureId}.{imageType}")))
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
        }

        private string TestPicId(int picId)
        {
            try
            {
                Image imageToLoad = Image.FromFile(Path.Combine(scriptDirectory, string.Format(@"Images\{0}.png", picId)));
                imageToLoad.Dispose();
                return "png";
            }
            catch (FileNotFoundException)
            {
                try
                {
                    Image imageToLoad = Image.FromFile(Path.Combine(scriptDirectory, string.Format(@"Images\{0}.jpg", picId)));
                    imageToLoad.Dispose();
                    return "jpg";
                }
                catch (Exception)
                {
                    return "";
                }
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

            scriptDirectory = System.IO.Directory.GetCurrentDirectory();

            sqlConnection = new SQLiteConnection($@"Data Source ={scriptDirectory}\Imagedata\images.sqlite; version = 3");
            sqlConnection.Open();
        }

        #region ButtonUpdates

        private void changeLabel(Label associatedLabel, ref int checkState)
        {
            checkState++;
            try
            {
                associatedLabel.Text = buttonStates[checkState];
                switch (associatedLabel.Text)
                {
                    case "Any":
                        associatedLabel.ForeColor = Color.Empty;
                        break;
                    case "Yes":
                        associatedLabel.ForeColor = Color.Green;
                        break;
                }
            }
            catch (KeyNotFoundException)
            {
                checkState = 0;
                associatedLabel.Text = buttonStates[checkState];
                associatedLabel.ForeColor = Color.Red;
            }
        }

        private void LoliButton_Click(object sender, EventArgs e) => changeLabel(LoliLabel, ref LoliCheckState);

        private void SoloButton_Click(object sender, EventArgs e) => changeLabel(SoloLabel, ref SoloCheckState);

        private void MasturbationButton_Click(object sender, EventArgs e) => changeLabel(MasturbationLabel, ref MasturbationCheckState);

        private void BigBreastsButton_Click(object sender, EventArgs e) => changeLabel(BigBreastsLabel, ref BigBreastCheckState);

        private void YuriButton_Click(object sender, EventArgs e) => changeLabel(YuriLabel, ref YuriCheckState);

        private void NonHButton_Click(object sender, EventArgs e) => changeLabel(NonHLabel, ref NonHCheckState);

        private void KemonomimiButton_Click(object sender, EventArgs e) => changeLabel(KemonomimiLabel, ref KemonomimicheckState);

        private void BlowJobButton_Click(object sender, EventArgs e) => changeLabel(BlowJobLabel, ref BlowJobCheckState);

        private void AnalButton_Click(object sender, EventArgs e) => changeLabel(AnalLabel, ref AnalCheckState);

        private void ToysButton_Click(object sender, EventArgs e) => changeLabel(ToysLabel, ref ToysCheckState);

        private void TentaclesButton_Click(object sender, EventArgs e) => changeLabel(TentaclesLabel, ref TentaclesCheckState);

        private void BoatButton_Click(object sender, EventArgs e) => changeLabel(BoatLabel, ref BoatCheckState);

        private void TouhouButton_Click(object sender, EventArgs e) => changeLabel(TouhouLabel, ref TouhouCheckState);

        private void AhegaoButton_Click(object sender, EventArgs e) => changeLabel(AhegaoLabel, ref AhegaoCheckState);

        private void FavoritesButton_Click(object sender, EventArgs e) => changeLabel(FavoritesLabel, ref FavoritesCheckState);

        #endregion

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

        private void SearchButton_Click(object sender, EventArgs e)
        {
            imageIds.Clear();
            currentImage = 0;

            string sqlCommandString = "select imageId from imageData";

            Dictionary<string, int> selectedSearch = new Dictionary<string, int>();

            string sqlFieldName;
            for (int i = 0; i < tagLabels.Length; i++)
            {
                /*
                 * YuriLabel, KemonomimiLabel, NonHLabel, MasturbationLabel, TentaclesLabel,
                 * SoloLabel, ToysLabel, BigBreastsLabel, BoatLabel, LoliLabel,
                 * BlowJobLabel, AnalLabel, TouhouLabel, AhegaoLabel, FavoritesLabel
                 */
                switch (i)
                {
                    case 0:
                        sqlFieldName = "yuri";
                        break;
                    case 1:
                        sqlFieldName = "kemonomimi";
                        break;
                    case 2:
                        sqlFieldName = "nonh";
                        break;
                    case 3:
                        sqlFieldName = "masturbation";
                        break;
                    case 4:
                        sqlFieldName = "tentacle";
                        break;
                    case 5:
                        sqlFieldName = "solo";
                        break;
                    case 6:
                        sqlFieldName = "toys";
                        break;
                    case 7:
                        sqlFieldName = "bigBreast";
                        break;
                    case 8:
                        sqlFieldName = "boat";
                        break;
                    case 9:
                        sqlFieldName = "loli";
                        break;
                    case 10:
                        sqlFieldName = "blowJob";
                        break;
                    case 11:
                        sqlFieldName = "anal";
                        break;
                    case 12:
                        sqlFieldName = "touhou";
                        break;
                    case 13:
                        sqlFieldName = "ahegao";
                        break;
                    case 14:
                        sqlFieldName = "favorite";
                        break;
                    default:
                        throw new Exception("there are more labels in the label list than accounted for");
                }
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

            try
            {
                SQLiteCommand sqlCommand = new SQLiteCommand(sqlCommandString, sqlConnection);
                SQLiteDataReader sqlReader = sqlCommand.ExecuteReader();

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
            catch (SQLiteException)
            {
                SearchResultLabel.Text = "0 Results";
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
                using (var transation = sqlConnection.BeginTransaction())
                {

                }
            }

            if (imagesToDelete.Count != 0)
            {
                try
                {
                    hentaiPicBox.Image.Dispose();
                }
                catch (NullReferenceException) { }

                hentaiPicBox.Image = null;
                foreach (int imageId in imagesToDelete)
                {
                    string fileExtension = TestPicId(imageId);
                    if (!string.IsNullOrEmpty(fileExtension))
                    {
                        File.Delete(Path.Combine(scriptDirectory, $@"Images\{imageId}.{fileExtension}"));
                        string sqlCommandString = $"delete from imageData where imageId = {imageId}";
                        SQLiteCommand sqlCommand = new SQLiteCommand(sqlCommandString, sqlConnection);
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            imagesToDelete.Clear();
            imageIds.Clear();
            hentaiPicBox.Image = null;
            currentImage = 0;
            imageTags.Clear();
            initialImageTags.Clear();
            SearchResultLabel.Text = "";
            CurrentImageLabel.Text = "";

            foreach (CheckBox tagCheckBox in tagCheckBoxes)
            {
                tagCheckBox.Checked = false;
            }
        }

        private void ResetSearchButton_Click(object sender, EventArgs e)
        {
            imagesToDelete.Clear();
            imageIds.Clear();
            hentaiPicBox.Image = null;
            currentImage = 0;
            imageTags.Clear();
            initialImageTags.Clear();
            SearchResultLabel.Text = "";
            CurrentImageLabel.Text = "";

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

        public void ViewDatabase_Load()
        {
            string sqlCommandString = "select imageId, yuri, kemonomimi, nonh, masturbation, tentacle, solo, toys, bigBreast, boat, " +
            "loli, blowJob, anal, touhou, ahegao, favorite from imageData";

            SQLiteCommand sqlCommand = new SQLiteCommand(sqlCommandString, sqlConnection);
            SQLiteDataReader sqlReader = sqlCommand.ExecuteReader();

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
                initialImageTags.Add((int)sqlReader["imageId"], rowValues);
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
