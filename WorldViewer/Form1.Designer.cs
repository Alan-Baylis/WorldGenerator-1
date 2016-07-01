namespace WorldViewer
{
    partial class Form1
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
            this.worldPictureBox = new System.Windows.Forms.PictureBox();
            this.localPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.worldPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.localPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // worldPictureBox
            // 
            this.worldPictureBox.Location = new System.Drawing.Point(0, 0);
            this.worldPictureBox.Name = "worldPictureBox";
            this.worldPictureBox.Size = new System.Drawing.Size(313, 254);
            this.worldPictureBox.TabIndex = 0;
            this.worldPictureBox.TabStop = false;
            this.worldPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            // 
            // localPictureBox
            // 
            this.localPictureBox.Location = new System.Drawing.Point(319, 0);
            this.localPictureBox.Name = "localPictureBox";
            this.localPictureBox.Size = new System.Drawing.Size(313, 254);
            this.localPictureBox.TabIndex = 1;
            this.localPictureBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 255);
            this.Controls.Add(this.localPictureBox);
            this.Controls.Add(this.worldPictureBox);
            this.Name = "Form1";
            this.Text = "WorldViewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.worldPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.localPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox worldPictureBox;
        private System.Windows.Forms.PictureBox localPictureBox;
    }
}

