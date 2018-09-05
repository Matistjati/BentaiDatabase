namespace BentaiDataBase
{
    partial class Front_page
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
            this.GithubButton = new System.Windows.Forms.Button();
            this.FrontPagePicture = new System.Windows.Forms.PictureBox();
            this.ThankYouLabel = new System.Windows.Forms.Label();
            this.ReportBugsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FrontPagePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // GithubButton
            // 
            this.GithubButton.Location = new System.Drawing.Point(45, 379);
            this.GithubButton.Name = "GithubButton";
            this.GithubButton.Size = new System.Drawing.Size(75, 23);
            this.GithubButton.TabIndex = 0;
            this.GithubButton.Text = "Github";
            this.GithubButton.UseVisualStyleBackColor = true;
            this.GithubButton.Click += new System.EventHandler(this.GithubButton_Click);
            // 
            // FrontPagePicture
            // 
            this.FrontPagePicture.Image = global::BentaiDatabase.Properties.Resources.Sagiri_overload;
            this.FrontPagePicture.Location = new System.Drawing.Point(45, 26);
            this.FrontPagePicture.Name = "FrontPagePicture";
            this.FrontPagePicture.Size = new System.Drawing.Size(637, 347);
            this.FrontPagePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FrontPagePicture.TabIndex = 1;
            this.FrontPagePicture.TabStop = false;
            // 
            // ThankYouLabel
            // 
            this.ThankYouLabel.AutoSize = true;
            this.ThankYouLabel.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThankYouLabel.Location = new System.Drawing.Point(103, 0);
            this.ThankYouLabel.Name = "ThankYouLabel";
            this.ThankYouLabel.Size = new System.Drawing.Size(544, 23);
            this.ThankYouLabel.TabIndex = 2;
            this.ThankYouLabel.Text = "Thank you for using BentaiDatabase v 1.0 (name subject to change)";
            // 
            // ReportBugsLabel
            // 
            this.ReportBugsLabel.AutoSize = true;
            this.ReportBugsLabel.Location = new System.Drawing.Point(126, 384);
            this.ReportBugsLabel.Name = "ReportBugsLabel";
            this.ReportBugsLabel.Size = new System.Drawing.Size(104, 13);
            this.ReportBugsLabel.TabIndex = 3;
            this.ReportBugsLabel.Text = "<- Report Bugs Here";
            // 
            // Front_page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ReportBugsLabel);
            this.Controls.Add(this.ThankYouLabel);
            this.Controls.Add(this.FrontPagePicture);
            this.Controls.Add(this.GithubButton);
            this.Name = "Front_page";
            this.Size = new System.Drawing.Size(711, 405);
            ((System.ComponentModel.ISupportInitialize)(this.FrontPagePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GithubButton;
        private System.Windows.Forms.PictureBox FrontPagePicture;
        private System.Windows.Forms.Label ThankYouLabel;
        private System.Windows.Forms.Label ReportBugsLabel;
    }
}
