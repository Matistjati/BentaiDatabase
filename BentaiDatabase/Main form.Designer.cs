namespace BentaiDataBase
{
    partial class Loli_in_form
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Loli_in_form));
            this.UserControlPanel = new System.Windows.Forms.Panel();
            this.AddImagesButton = new System.Windows.Forms.Button();
            this.ViewImagesButton = new System.Windows.Forms.Button();
            this.DataStatisticsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UserControlPanel
            // 
            this.UserControlPanel.Location = new System.Drawing.Point(0, 0);
            this.UserControlPanel.Name = "UserControlPanel";
            this.UserControlPanel.Size = new System.Drawing.Size(711, 405);
            this.UserControlPanel.TabIndex = 0;
            // 
            // AddImagesButton
            // 
            this.AddImagesButton.Location = new System.Drawing.Point(718, 0);
            this.AddImagesButton.Name = "AddImagesButton";
            this.AddImagesButton.Size = new System.Drawing.Size(62, 23);
            this.AddImagesButton.TabIndex = 1;
            this.AddImagesButton.Text = "Add pics";
            this.AddImagesButton.UseVisualStyleBackColor = true;
            this.AddImagesButton.Click += new System.EventHandler(this.AddImagesButton_Click);
            // 
            // ViewImagesButton
            // 
            this.ViewImagesButton.Location = new System.Drawing.Point(718, 29);
            this.ViewImagesButton.Name = "ViewImagesButton";
            this.ViewImagesButton.Size = new System.Drawing.Size(62, 23);
            this.ViewImagesButton.TabIndex = 2;
            this.ViewImagesButton.Text = "View Pics";
            this.ViewImagesButton.UseVisualStyleBackColor = true;
            this.ViewImagesButton.Click += new System.EventHandler(this.ViewImagesButton_Click);
            // 
            // DataStatisticsButton
            // 
            this.DataStatisticsButton.Location = new System.Drawing.Point(718, 58);
            this.DataStatisticsButton.Name = "DataStatisticsButton";
            this.DataStatisticsButton.Size = new System.Drawing.Size(62, 23);
            this.DataStatisticsButton.TabIndex = 3;
            this.DataStatisticsButton.Text = "Statistics";
            this.DataStatisticsButton.UseVisualStyleBackColor = true;
            this.DataStatisticsButton.Click += new System.EventHandler(this.DataStatisticsButton_Click);
            // 
            // Loli_in_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(780, 404);
            this.Controls.Add(this.DataStatisticsButton);
            this.Controls.Add(this.ViewImagesButton);
            this.Controls.Add(this.AddImagesButton);
            this.Controls.Add(this.UserControlPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Loli_in_form";
            this.Text = "Loli Database";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Loli_in_form_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Loli_in_form_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button AddImagesButton;
        private System.Windows.Forms.Button ViewImagesButton;
        private System.Windows.Forms.Button DataStatisticsButton;
        public System.Windows.Forms.Panel UserControlPanel;
    }
}

