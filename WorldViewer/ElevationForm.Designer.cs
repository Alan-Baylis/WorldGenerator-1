namespace WorldViewer
{
    partial class ElevationForm
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
            this.xPictureBox = new System.Windows.Forms.PictureBox();
            this.zPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.xPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // xPictureBox
            // 
            this.xPictureBox.Location = new System.Drawing.Point(12, 12);
            this.xPictureBox.Name = "xPictureBox";
            this.xPictureBox.Size = new System.Drawing.Size(512, 256);
            this.xPictureBox.TabIndex = 0;
            this.xPictureBox.TabStop = false;
            // 
            // zPictureBox
            // 
            this.zPictureBox.Location = new System.Drawing.Point(12, 274);
            this.zPictureBox.Name = "zPictureBox";
            this.zPictureBox.Size = new System.Drawing.Size(512, 256);
            this.zPictureBox.TabIndex = 1;
            this.zPictureBox.TabStop = false;
            // 
            // ElevationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 544);
            this.Controls.Add(this.zPictureBox);
            this.Controls.Add(this.xPictureBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ElevationForm";
            this.Text = "ElevationForm";
            ((System.ComponentModel.ISupportInitialize)(this.xPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox xPictureBox;
        private System.Windows.Forms.PictureBox zPictureBox;
    }
}