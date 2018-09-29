using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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

        private readonly string[] DataNames = new string[]
        {
            "Yuri", "Loli", "Kemonomimi", "nonH", "Masturbation", "Tentacle", "Solo", "Toys", "BigBreast",
            "Boat", "Blowjob", "Anal", "Touhou", "Ahegao"
        };

        public DataBaseStatistics()
        {
            InitializeComponent();

            DataBaseChart.Legends[0].Enabled = false;
        }

        private readonly string[] DataBaseNames = new string[]
        {
            "yuri", "loli", "kemonomimi", "nonh", "masturbation", "tentacle", "solo", "toys", "bigBreast",
            "boat", "blowJob", "anal", "touhou", "ahegao"
        };

        private void TagAmountsButton_Click(object sender, EventArgs e)
        {
            CurrentChartLabel.Text = "Tag Amounts";
            string sqlFieldName;
            List<int> valueSums = new List<int>();
            List<int> FieldSum = new List<int>();

            for (int i = 0; i < DataBaseNames.Length; i++)
            {
                sqlFieldName = DataBaseNames[i];

                using (SQLiteConnection sqlConnection = new SQLiteConnection(Globals.dataBaseString))
                {
                    sqlConnection.Open();
                    using (SQLiteCommand sqlCommand = new SQLiteCommand($"select {sqlFieldName} from imageData", sqlConnection))
                    using (SQLiteDataReader sqlReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            int fieldValue = (int)sqlReader[sqlFieldName];
                            FieldSum.Add(fieldValue);
                        }

                        valueSums.Add(FieldSum.Sum());
                        FieldSum.Clear();
                    }
                }
            }

            List<string> PlotDataNames = DataNames.ToList();

            for (int i = 0; i < valueSums.Count; i++)
            {
                if (valueSums[i] == 0)
                {
                    valueSums.RemoveAt(i);
                    PlotDataNames.RemoveAt(i);
                    i--;
                }
            }

            if (valueSums.Sum() == 0)
            {
                CurrentChartLabel.Text = "No Database Info yet, sorry";
                return;
            }

            DataBaseChart.Series[0].ChartType = SeriesChartType.Pie;
            DataBaseChart.Series[0].Points.DataBindXY(PlotDataNames, valueSums);
            DataBaseChart.Legends[0].Enabled = true;
            DataBaseChart.ChartAreas[0].Area3DStyle.Enable3D = true;
            DataBaseChart.Series[0].IsValueShownAsLabel = true;
        }
    }
}
