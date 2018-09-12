using System;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace BentaiDataBase
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string scriptDirectory = Globals.scriptDirectory;

            // Checking that all the necessary folders and files exist
            if (!Directory.Exists($@"{scriptDirectory}\Images"))
            {
                Directory.CreateDirectory($@"{scriptDirectory}\Images");
            }

            // Creating our databases table if it doesn't exist
            using (SQLiteConnection sqlConnection = new SQLiteConnection(Globals.dataBaseString))
            using (SQLiteCommand sqlCommand = new SQLiteCommand(sqlConnection))
            {
                sqlConnection.Open();

                sqlCommand.CommandText = "CREATE TABLE IF NOT EXISTS imageData (imageId INT, yuri INT, loli INT,  kemonomimi INT, nonh INT," +
                    "masturbation INT, tentacle INT, solo INT, toys INT, bigBreast INT, boat INT, blowJob INT, anal INT, " +
                    "touhou INT, ahegao INT, favorite INT, image BLOB)";
                sqlCommand.ExecuteNonQuery();
            }

            // Pregenerated code, no idea what it does
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Filling the form with the front page before starting
            Loli_in_form loliDatabase = new Loli_in_form();
            loliDatabase.UserControlPanel.Controls.Add(Front_page.Instance);
            Application.Run(loliDatabase);               
        }
    }
}
