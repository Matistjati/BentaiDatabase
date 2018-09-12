using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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

                    // Finding the picture name
                    if (!File.Exists($@"{Globals.scriptDirectory}\Imagedata\currentimage.txt"))
                    {
                        latestPicInt = 0;
                    }
                    else if (!(new FileInfo($@"{Globals.scriptDirectory}\Imagedata\currentimage.txt").Length == 0))
                    {
                        string[] lines = File.ReadAllLines($@"{Globals.scriptDirectory}\Imagedata\currentimage.txt");
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

        private int currentPic;

        internal CheckBox[] tagCheckBoxes;

        private List<string> loliPics = new List<string>();

        internal Dictionary<int, Dictionary<string, int>> imageData = new Dictionary<int, Dictionary<string, int>>();

        private List<string> imagesToAdd = new List<string>();

        internal void LoadNewPic(string FileName)
        {
            if (string.IsNullOrEmpty(FileName) || !File.Exists(FileName))
            {
                throw new ArgumentException("Invalid file");
            }

            using (var sourceImage = Image.FromFile(FileName))
            {
                var targetImage = new Bitmap(sourceImage.Width, sourceImage.Height,
                  System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (var canvas = Graphics.FromImage(targetImage))
                {
                    canvas.DrawImageUnscaled(sourceImage, 0, 0);
                }
                PopulatePicBox.Image = targetImage;
            }
        }

        public PopulateDataBase(int ImageNamenum)
        {
            InitializeComponent();

            tagCheckBoxes = new CheckBox[]
            {
                YuriCheck, KemonomimiCheck, NonhCheck, MasturbationCheck, TentacleCheck, SoloCheck,
                ToysCheck, bigbreastCheck, BoatCheck, LoliCheck, BlowJobCheck, AnalCheck,
                TouhouCheck, AhegaoCheck, FavoriteCheck
            };

            try
            {
                loliPics = getImagesToAdd();
            }
            catch (FileNotFoundException)
            {
                loliPics = getImageFilesFromDialog();
            }

            try
            {
                this.LoadNewPic(this.loliPics[currentPic]);
            }
            catch (ArgumentOutOfRangeException)
            {
                List<string> newImages = getImageFilesFromDialog();
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

        internal List<string> getImagesToAdd()
        {
            string[] lolis = Directory.GetFiles(Globals.scriptDirectory);

            List<string> loliImages = new List<string>();
            foreach (string loli in lolis)
            {
                if (Path.GetExtension(loli) == ".png" || Path.GetExtension(loli) == ".jpg" || Path.GetExtension(loli) == ".jpeg")
                {
                    loliImages.Add(loli);
                }
            }

            if (loliImages.Count == 0)
            {
                throw new FileNotFoundException("No files found in the programs directory");
            }
            else
            {
                return loliImages;
            }
        }

        public List<String> getImageFilesFromDialog()
        {
            // MessageBoxManager lets me change the messagebox buttons
            MessageBoxManager.Yes = "File";
            MessageBoxManager.No = "Folder";
            MessageBoxManager.Register();

            DialogResult result = MessageBox.Show("No valid image files were found. Want to select one manually?",
                "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);

            MessageBoxManager.Unregister();

            if (result == DialogResult.Yes)
            {
                OpenFileDialog fileDialog = new OpenFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png",
                    FilterIndex = 2,
                    RestoreDirectory = true
                };

                // Checking that a valid path was returned from the file dialog
                if (fileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fileDialog.FileName))
                {
                    return new List<String>() { fileDialog.FileName };
                }
                else
                {
                    // If an invalid path was selected we return an empty list
                    return new List<string>();
                }
            }
            else if (result == DialogResult.No)
            {
                using (var folderDialog = new FolderBrowserDialog())
                {
                    DialogResult folderResult = folderDialog.ShowDialog();

                    // Checking that a valid path was returned from the file dialog
                    if (folderResult == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                    {
                        string[] files = Directory.GetFiles(folderDialog.SelectedPath);
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
                // If the user cancels adding files we return an empty list
                return new List<string>();
            }
        }

        public void SubmitTags_Click(object sender, EventArgs e)
        {
            bool check = false;
            foreach (CheckBox checkbox in tagCheckBoxes)
            {
                if (checkbox.Checked)
                {
                    check = true;
                    break;
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

            int keyName = imageData.Count == 0 ? 0 : imageData.Keys.ToArray<int>().Max() + 1;
            imageData.Add(keyName, tagsChecked);
            imagesToAdd.Add(loliPics[currentPic]);

            SubmitTags.Cursor = Cursors.No;
            if (currentPic + 1 >= loliPics.Count)
            {
                SaveFiles();
                MessageBox.Show("Files saved successfully");
            }
            else
            {
                LoadNewPic(loliPics[currentPic + 1]);
            }

            this.currentPic++;
            if (QueueLabel.Text != "0")
            {
                // How many pics are unchecked
                QueueLabel.Text = ((loliPics.Count() - 1) - currentPic).ToString();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                SaveFiles();
                MessageBox.Show("Files saved successfully");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
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
                SubmitTags.Cursor = Cursors.Hand;
            }
            else
            {
                SubmitTags.Cursor = Cursors.No;
            }
        }

        private void AddFile_Click(object sender, EventArgs e)
        {
            List<string> newImages = getImageFilesFromDialog();
            if (newImages.Count != 0)
            {
                loliPics = loliPics.Concat(newImages).ToList();
                LoadNewPic(loliPics[currentPic]);
                QueueLabel.Text = ((loliPics.Count() - 1) - currentPic).ToString();
            }
            else
            {
                this.QueueLabel.Text = "0";
            }
        }

        internal void SaveFiles()
        {
            if (imageData.Count == 0)
            {
                return;
            }

            Image[] imagesToAddArray = new Image[imagesToAdd.Count];
            for (int i = 0; i < this.imagesToAdd.Count; i++)
            {
                if (File.Exists(this.imagesToAdd[i]))
                {
                    imagesToAddArray[i] = Image.FromFile(imagesToAdd[i]);
                }
            }

            BentaiDataBaseHandler.AddDataBaseEntryWithImage(imagesToAddArray, imageData);

            imageData.Clear();
            this.imagesToAdd.Clear();
        }

        private void saveFiles_Click(object sender, EventArgs e)
        {
            SaveFiles();
            MessageBox.Show("Files saved successfully");
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            if (currentPic + 1 >= loliPics.Count || currentPic >= loliPics.Count)
            {
                SaveFiles();
                MessageBox.Show("Images saved successfully");
                return;
            }

            currentPic++;
            if (QueueLabel.Text != "0")
            {
                QueueLabel.Text = ((loliPics.Count() - 1) - currentPic).ToString();
            }

            LoadNewPic(loliPics[currentPic + 1]);
        }
    }
}
