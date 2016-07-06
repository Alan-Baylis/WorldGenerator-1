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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.globalPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.worldPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.localPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.globalPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // worldPictureBox
            // 
            this.worldPictureBox.Location = new System.Drawing.Point(23, 3);
            this.worldPictureBox.Name = "worldPictureBox";
            this.worldPictureBox.Size = new System.Drawing.Size(235, 228);
            this.worldPictureBox.TabIndex = 0;
            this.worldPictureBox.TabStop = false;
            // 
            // localPictureBox
            // 
            this.localPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localPictureBox.Location = new System.Drawing.Point(0, 0);
            this.localPictureBox.Name = "localPictureBox";
            this.localPictureBox.Size = new System.Drawing.Size(395, 390);
            this.localPictureBox.TabIndex = 1;
            this.localPictureBox.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(12, 408);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(794, 47);
            this.textBox1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.globalPictureBox);
            this.splitContainer1.Panel1.Controls.Add(this.worldPictureBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.localPictureBox);
            this.splitContainer1.Size = new System.Drawing.Size(794, 390);
            this.splitContainer1.SplitterDistance = 395;
            this.splitContainer1.TabIndex = 4;
            this.splitContainer1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.splitContainer1_KeyPress);
            // 
            // globalPictureBox
            // 
            this.globalPictureBox.Location = new System.Drawing.Point(136, 129);
            this.globalPictureBox.Name = "globalPictureBox";
            this.globalPictureBox.Size = new System.Drawing.Size(256, 256);
            this.globalPictureBox.TabIndex = 1;
            this.globalPictureBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 466);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "WorldViewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.worldPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.localPictureBox)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.globalPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox worldPictureBox;
        private System.Windows.Forms.PictureBox localPictureBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox globalPictureBox;
    }
}

