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
            string scriptDirectory;
            SQLiteConnection sqlConnection;

            scriptDirectory = Directory.GetCurrentDirectory();

            if (!Directory.Exists(scriptDirectory + "\\Images"))
            {
                Directory.CreateDirectory(scriptDirectory + "\\Images");
            }

            if (!Directory.Exists(scriptDirectory + "\\Imagedata"))
            {
                Directory.CreateDirectory(scriptDirectory + "\\Imagedata");
            }

            sqlConnection = new SQLiteConnection($@"Data Source={scriptDirectory}\Imagedata\images.sqlite;version=3");
            sqlConnection.Open();

            try
            {
                // trying to create the table
                string sql_command = "CREATE TABLE imageData (imageId INT, yuri INT, loli INT,  kemonomimi INT, nonh INT," +
                    "masturbation INT, tentacle INT, solo INT, toys INT, bigBreast INT, boat INT, blowJob INT, anal INT, " +
                    "touhou INT, ahegao INT, favorite INT)";
                SQLiteCommand command = new SQLiteCommand(sql_command, sqlConnection);
                command.ExecuteNonQuery();
            }
            catch (SQLiteException) { /* If the table already exists, do nothing */ }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Loli_in_form());
            }
            catch (System.ObjectDisposedException)
            {

            }
        }
    }
}
