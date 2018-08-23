namespace BentaiDataBase
{
    partial class DataBaseStatistics
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.TagAmountsButton = new System.Windows.Forms.Button();
            this.DataBaseChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.CurrentChartLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DataBaseChart)).BeginInit();
            this.SuspendLayout();
            // 
            // TagAmountsButton
            // 
            this.TagAmountsButton.Location = new System.Drawing.Point(4, 383);
            this.TagAmountsButton.Name = "TagAmountsButton";
            this.TagAmountsButton.Size = new System.Drawing.Size(77, 23);
            this.TagAmountsButton.TabIndex = 0;
            this.TagAmountsButton.Text = "Tag amounts";
            this.TagAmountsButton.UseVisualStyleBackColor = true;
            this.TagAmountsButton.Click += new System.EventHandler(this.TagAmountsButton_Click);
            // 
            // DataBaseChart
            // 
            this.DataBaseChart.BackColor = System.Drawing.SystemColors.Control;
            this.DataBaseChart.BackImageTransparentColor = System.Drawing.SystemColors.Control;
            this.DataBaseChart.BackSecondaryColor = System.Drawing.SystemColors.Control;
            this.DataBaseChart.BorderlineColor = System.Drawing.SystemColors.Control;
            chartArea1.Name = "ChartArea1";
            this.DataBaseChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.DataBaseChart.Legends.Add(legend1);
            this.DataBaseChart.Location = new System.Drawing.Point(0, 0);
            this.DataBaseChart.Name = "DataBaseChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.DataBaseChart.Series.Add(series1);
            this.DataBaseChart.Size = new System.Drawing.Size(669, 377);
            this.DataBaseChart.TabIndex = 1;
            this.DataBaseChart.Text = "char1";
            // 
            // CurrentChartLabel
            // 
            this.CurrentChartLabel.AutoSize = true;
            this.CurrentChartLabel.Location = new System.Drawing.Point(4, 4);
            this.CurrentChartLabel.Name = "CurrentChartLabel";
            this.CurrentChartLabel.Size = new System.Drawing.Size(0, 13);
            this.CurrentChartLabel.TabIndex = 2;
            // 
            // DataBaseStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CurrentChartLabel);
            this.Controls.Add(this.DataBaseChart);
            this.Controls.Add(this.TagAmountsButton);
            this.Name = "DataBaseStatistics";
            this.Size = new System.Drawing.Size(669, 409);
            ((System.ComponentModel.ISupportInitialize)(this.DataBaseChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TagAmountsButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart DataBaseChart;
        private System.Windows.Forms.Label CurrentChartLabel;
    }
}
