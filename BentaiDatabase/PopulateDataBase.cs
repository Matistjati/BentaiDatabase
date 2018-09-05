using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace BentaiDataBase
{
    public partial class PopulateDataBase : UserControl
    {
        private static PopulateDataBase _instance;
        public static PopulateDataBase Instance
        {
            get
            {
                if (_instance == null)
                {
                    int latestPicInt;
                    string path = Directory.GetCurrentDirectory();
                    if (!File.Exists(path + "\\Imagedata\\currentimage.txt"))
                    {
                        var currentImage = File.Create(path + "\\Imagedata\\currentimage.txt");
                        currentImage.Close();
                        latestPicInt = 0;
                    }
                    else if (!(new FileInfo(path + "\\Imagedata\\currentimage.txt").Length == 0))
                    {
                        string[] lines = File.ReadAllLines(path + "\\Imagedata\\currentimage.txt");
                        if (Int32.TryParse(lines[0], out int tempPicInt))
                        {
                            latestPicInt = tempPicInt;
                        }
                        else
                        {
                            latestPicInt = 0;
                        }
                    }
                    else
                    {
                        latestPicInt = 0;
                    }
                    _instance = new PopulateDataBase(latestPicInt);
                }
                return _instance;
            }
        }

        private int ImagenameNumber;
        private int currentPic;
        private string scriptDirectory;

        private List<string> loliPics = new List<string>();

        private List<string> picsToDelete = new List<string>();
        private List<Image> loadedPics = new List<Image>();
        public IDictionary<string, int> picsToBeMoved = new Dictionary<string, int>();
        private Dictionary<int, Dictionary<string, int>> imageData = new Dictionary<int, Dictionary<string, int>>();

        public SQLiteConnection sqlConnection;
        public CheckBox[] tagCheckBoxes;

        private void LoadNewPic(string FileName)
        {
            Image imageToLoad = Image.FromFile(FileName);
            loadedPics.Add(imageToLoad);
            pictureBox1.Image = imageToLoad;
        }

        public PopulateDataBase(int ImageNamenum)
        {
            InitializeComponent();

            tagCheckBoxes = new CheckBox[]
            {
                this.YuriCheck, this.KemonomimiCheck, this.NonhCheck, this.MasturbationCheck, this.TentacleCheck, this.SoloCheck,
                this.ToysCheck, this.bigbreastCheck, this.BoatCheck, this.LoliCheck, this.BlowJobCheck, this.AnalCheck,
                this.TouhouCheck, this.AhegaoCheck, this.FavoriteCheck
            };

            ImagenameNumber += ImageNamenum;
            scriptDirectory = Directory.GetCurrentDirectory();

            sqlConnection = new SQLiteConnection($@"Data Source={scriptDirectory}\Imagedata\images.sqlite;version=3");
            sqlConnection.Open();

            string[] lolis = Directory.GetFiles(scriptDirectory);

            foreach (string loli in lolis)
            {
                if (Path.GetExtension(loli) == ".png" || Path.GetExtension(loli) == ".jpg" || Path.GetExtension(loli) == ".jpeg")
                {
                    loliPics.Add(loli);
                }
            }

            try
            {
                this.LoadNewPic(this.loliPics[currentPic]);
            }
            catch (ArgumentOutOfRangeException)
            {
                List<string> newImages = getImageFiles();
                if (newImages.Count != 0)
                {
                    loliPics = loliPics.Concat(newImages).ToList();
                    this.LoadNewPic(this.loliPics[currentPic]);
                    this.QueueLabel.Text = (this.loliPics.Count() - 1).ToString();
                }
                else
                {
                    Loli_in_form.Instance.Close();
                    this.QueueLabel.Text = "0";
                }
            }

            if (QueueLabel.Text != "0")
            {
                this.QueueLabel.Text = (this.loliPics.Count() - 1).ToString();
            }
        }

        public List<String> getImageFiles()
        {
            MessageBoxManager.Yes = "File";
            MessageBoxManager.No = "Folder";
            MessageBoxManager.Register();
            DialogResult result = MessageBox.Show("No valid image files were found. Want to select one manually?",
                "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
            MessageBoxManager.Unregister();
            if (result == DialogResult.Yes)
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png",
                    FilterIndex = 2,
                    RestoreDirectory = true
                };

                if (openFileDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog1.FileName))
                {
                    return new List<String>() { openFileDialog1.FileName };
                }
                else
                {
                    return new List<string>();
                }
            }
            else if (result == DialogResult.No)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult folderResult = fbd.ShowDialog();

                    if (folderResult == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string[] files = Directory.GetFiles(fbd.SelectedPath);
                        List<string> imageFiles = new List<string>();
                        foreach (string file in files)
                        {
                            if (Path.GetExtension(file) == ".png" || Path.GetExtension(file) == ".jpg" || Path.GetExtension(file) == ".jpeg")
                            {
                                imageFiles.Add(file);
                            }

                        }
                        return imageFiles;
                    }
                    else
                    {
                        return new List<string>();
                    }
                }
            }
            else
            {
                return new List<string>();
            }
        }

        public void SubmitTags_Click(object sender, EventArgs e)
        {
            bool check = false;
            foreach (var checkbox in tagCheckBoxes)
            {
                if (checkbox.Checked)
                {
                    check = true;
                }
            }
            if (!check)
            {
                return;
            }

            Dictionary<string, int> tagsChecked = new Dictionary<string, int>();
            foreach (CheckBox checkbox in tagCheckBoxes)
            {
                tagsChecked.Add(checkbox.Text, Convert.ToInt32(checkbox.Checked));
                checkbox.Checked = false;
            }
            tagsChecked.Add("id", ImagenameNumber);

            imageData.Add(currentPic, tagsChecked);


            this.SubmitTags.Cursor = Cursors.No;
            if (loadedPics.Count > 0)
            {
                try
                {
                    picsToBeMoved.Add(loliPics[currentPic], ImagenameNumber);
                    this.LoadNewPic(this.loliPics[currentPic + 1]);
                }
                catch (ArgumentOutOfRangeException)
                {
                    SaveFiles();
                    Loli_in_form.Instance.EmptyPanel();
                }
            }

            this.ImagenameNumber++;
            this.currentPic++;
            if (QueueLabel.Text != "0")
            {
                this.QueueLabel.Text = ((this.loliPics.Count() - 1) - currentPic).ToString();
            }

            if (loadedPics.Count == 0)
            {
                Loli_in_form.Instance.EmptyPanel();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                SaveFiles();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadNewPic(this.loliPics[this.currentPic + 1]);
            }
            catch (ArgumentOutOfRangeException)
            {
                SaveFiles();
                Loli_in_form.Instance.EmptyPanel();
            }
            picsToDelete.Add(loliPics[this.currentPic]);
            currentPic++;
            if (QueueLabel.Text != "0")
            {
                this.QueueLabel.Text = ((this.loliPics.Count() - 1) - currentPic).ToString();
            }

            foreach (CheckBox checkbox in tagCheckBoxes)
            {
                checkbox.Checked = false;
            }

        }

        private void CheckBoxChaged(object sender, EventArgs e)
        {
            bool anyBoxChecked = false;
            foreach (CheckBox checkbox in tagCheckBoxes)
            {
                if (checkbox.Checked)
                {
                    anyBoxChecked = true;
                    break;
                }
            }

            if (anyBoxChecked)
            {
                this.SubmitTags.Cursor = System.Windows.Forms.Cursors.Hand;
            }
            else
            {
                this.SubmitTags.Cursor = System.Windows.Forms.Cursors.No;
            }
        }

        private void AddFile_Click(object sender, EventArgs e)
        {
            List<string> newImages = getImageFiles();
            if (newImages.Count != 0)
            {
                loliPics = loliPics.Concat(newImages).ToList();
                this.LoadNewPic(this.loliPics[currentPic]);
                this.QueueLabel.Text = (this.loliPics.Count() - 1).ToString();
            }
            else
            {
                this.QueueLabel.Text = "0";
            }
        }

        public void SaveFiles()
        {
            if (this.loadedPics.Count != 0)
            {
                MessageBoxManager.Yes = "Move";
                MessageBoxManager.No = "Copy";
                MessageBoxManager.Register();
                DialogResult saveMethod = MessageBox.Show(
                    "In what way do you want the files to be moved to the image folder?",
                    "", MessageBoxButtons.YesNoCancel);
                MessageBoxManager.Unregister();

                if (saveMethod == DialogResult.Yes) // Move
                {
                    bool moveFile = true;
                    SaveFiles(moveFile);
                }
                else if (saveMethod == DialogResult.No) // Copy
                {
                    bool moveFile = false;
                    SaveFiles(moveFile);
                }
            }
        }

        private void SaveFiles(bool moveFile)
        {
            if (imageData.Count == 0)
            {
                return;
            }
            using (var cmd = new SQLiteCommand(sqlConnection))
            using (var transation = sqlConnection.BeginTransaction())
            {
                foreach (KeyValuePair<int, Dictionary<string, int>> dict in imageData)
                {
                    cmd.CommandText = string.Format("insert into imageData (imageId, yuri, loli, kemonomimi, nonh, " +
                        "masturbation, tentacle, solo, toys, bigBreast, boat, blowJob, anal, touhou, ahegao, favorite) " +
                        "values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15})",
                        dict.Value["id"], dict.Value["Yuri"], dict.Value["Loli"], dict.Value["Kemonomimi"],
                        dict.Value["Non-h"], dict.Value["Masturbation"], dict.Value["Tentacles"], dict.Value["Solo"],
                        dict.Value["Toys"], dict.Value["Big Breasts"], dict.Value["Boat"], dict.Value["BlowJob"],
                        dict.Value["Anal"], dict.Value["Touhou"], dict.Value["Ahegao"], dict.Value["Favorite"]);
                    cmd.ExecuteNonQuery();
                }
                transation.Commit();
            }

            foreach (Image imageToFree in loadedPics)
            {
                imageToFree.Dispose();
            }

            try
            {
                foreach (KeyValuePair<string, int> item in picsToBeMoved)
                {
                    File.Copy(item.Key, string.Format(@"{0}\Images\{1}.png",
                        scriptDirectory, item.Value));

                    File.Delete(string.Format(@"{0}\Images\{1}.png",
                        scriptDirectory, item.Value));
                    break;
                }
            }
            catch (IOException)
            {
                string[] savedImages = Directory.GetFiles(scriptDirectory + "\\Images");

                List<int> imageNamesInt = new List<int>();

                foreach (string imagePath in savedImages)
                {
                    string imageName = Path.GetFileNameWithoutExtension(imagePath);
                    if (int.TryParse(imageName, out int imageNumber))
                    {
                        imageNamesInt.Add(imageNumber);
                    }
                }
                int max;

                if (imageNamesInt.Count != 0)
                {
                    max = imageNamesInt.Max();

                    if ((max + 1) != ImagenameNumber)
                    {
                        ImagenameNumber = max + 1;
                    }
                }

                List<string> keys = new List<string>(picsToBeMoved.Keys);
                int tempImageNameNumber = ImagenameNumber;
                tempImageNameNumber++;
                foreach (string key in keys)
                {
                    picsToBeMoved[key] = tempImageNameNumber;
                    tempImageNameNumber++;
                }

                List<int> keysImageData = new List<int>(imageData.Keys);
                int tempImageNameNumber2 = ImagenameNumber;
                tempImageNameNumber++;
                foreach (int key in keysImageData)
                {
                    imageData[key]["id"] = tempImageNameNumber2;
                    tempImageNameNumber2++;
                }
            }



            foreach (KeyValuePair<string, int> picToMove in picsToBeMoved)
            {
                if (moveFile)
                {
                    File.Move(picToMove.Key, string.Format(@"{0}\Images\{1}.png",
                    scriptDirectory, picToMove.Value));
                }
                else
                {
                    File.Copy(picToMove.Key, string.Format(@"{0}\Images\{1}.png",
                        scriptDirectory, picToMove.Value));
                }
            }

            foreach (string picToDelete in picsToDelete)
            {
                File.Delete(picToDelete);
            }

            string currentPicStr = ImagenameNumber.ToString();
            List<string> Lines = new List<string>() { currentPicStr, "Dont touch this file" };
            File.WriteAllLines(scriptDirectory + "\\ImageData\\currentimage.txt", Lines);

            imageData.Clear();
            loadedPics.Clear();
            picsToDelete.Clear();
            picsToBeMoved.Clear();
            this.pictureBox1.Image = null;
        }

        private void saveFiles_Click(object sender, EventArgs e)
        {
            SaveFiles();
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            this.currentPic++;
            if (QueueLabel.Text != "0")
            {
                this.QueueLabel.Text = ((this.loliPics.Count() - 1) - currentPic).ToString();
            }
            try
            {
                this.LoadNewPic(loliPics[currentPic + 1]);
            }
            catch (ArgumentOutOfRangeException)
            {
                SaveFiles();
                Loli_in_form.Instance.EmptyPanel();
            }
        }
    }
}
