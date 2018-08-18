using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace BentaiDataBase
{
    public partial class DataBaseStatistics : UserControl
    {
        #region Singleton
        private static DataBaseStatistics _instance;
        public static DataBaseStatistics Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataBaseStatistics();
                }
                return _instance;
            }
        }
        #endregion

        private readonly string[] DataNamesStatic = new string[] 
        {
            "Yuri", "Loli", "Kemonomimi", "nonH", "Masturbation", "Tentacle", "Solo", "Toys", "BigBreast",
            "Boat", "Blowjob", "Anal", "Touhou", "Ahegao"
        };
        public SQLiteConnection sqlConnection;
        private string scriptDirectory;

        public DataBaseStatistics()
        {
            InitializeComponent();

            scriptDirectory = System.IO.Directory.GetCurrentDirectory();

            sqlConnection = new SQLiteConnection($@"Data Source ={scriptDirectory}\Imagedata\images.sqlite; version = 3");
            sqlConnection.Open();

            DataBaseChart.Legends[0].Enabled = false;
        }

        private void TagAmountsButton_Click(object sender, EventArgs e)
        {
            CurrentChartLabel.Text = "Tag Amounts";
            string sqlFieldName;
            List<int> valueSums = new List<int>();
            List<int> FieldSum = new List<int>();

            for (int i = 0; i < 14; i++)
            {
                switch (i)
                {
                    case 0:
                        sqlFieldName = "yuri";
                        break;
                    case 1:
                        sqlFieldName = "loli";
                        break;
                    case 2:
                        sqlFieldName = "kemonomimi";
                        break;
                    case 3:
                        sqlFieldName = "nonh";
                        break;
                    case 4:
                        sqlFieldName = "masturbation";
                        break;
                    case 5:
                        sqlFieldName = "tentacle";
                        break;
                    case 6:
                        sqlFieldName = "solo";
                        break;
                    case 7:
                        sqlFieldName = "toys";
                        break;
                    case 8:
                        sqlFieldName = "bigBreast";
                        break;
                    case 9:
                        sqlFieldName = "boat";
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
                    default:
                        throw new Exception("there are more labels in the label list than accounted for");
                }

                string sqlCommandString = $"select {sqlFieldName} from imageData";
                SQLiteCommand sqlCommand = new SQLiteCommand(sqlCommandString, sqlConnection);
                SQLiteDataReader sqlReader;

                try
                {
                    sqlReader = sqlCommand.ExecuteReader();
                }
                catch (SQLiteException)
                {
                    CurrentChartLabel.Text = "No Database Info yet, sorry";
                    return;
                }

                while (sqlReader.Read())
                {
                    int fieldValue = (int)sqlReader[sqlFieldName];
                    FieldSum.Add(fieldValue);
                }

                valueSums.Add(FieldSum.Sum());
                FieldSum.Clear();
            }

            List<string> DataNames = DataNamesStatic.ToList();

            for (int i = 0; i < valueSums.Count; i++)
            {
                if (valueSums[i] == 0)
                {
                    valueSums.RemoveAt(i);
                    DataNames.RemoveAt(i);
                    i--;
                }
            }

            if (valueSums.Sum() == 0)
            {
                CurrentChartLabel.Text = "No Database Info yet, sorry";
                return;
            }

            DataBaseChart.Series[0].ChartType = SeriesChartType.Pie;
            DataBaseChart.Series[0].Points.DataBindXY(DataNames, valueSums);
            DataBaseChart.Legends[0].Enabled = true;
            DataBaseChart.ChartAreas[0].Area3DStyle.Enable3D = true;
        }
    }
}
