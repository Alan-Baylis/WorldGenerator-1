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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.worldPictureBox = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.localPictureBox = new System.Windows.Forms.PictureBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.heightRadioButton = new System.Windows.Forms.RadioButton();
            this.tempRadioButton = new System.Windows.Forms.RadioButton();
            this.bioRadioButton = new System.Windows.Forms.RadioButton();
            this.waterRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.worldPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.localPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // worldPictureBox
            // 
            this.worldPictureBox.Location = new System.Drawing.Point(12, 264);
            this.worldPictureBox.Name = "worldPictureBox";
            this.worldPictureBox.Size = new System.Drawing.Size(256, 256);
            this.worldPictureBox.TabIndex = 0;
            this.worldPictureBox.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(12, 536);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(839, 47);
            this.textBox1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(274, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 512);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnGlobalMapMouseDoubleClick);
            // 
            // localPictureBox
            // 
            this.localPictureBox.Location = new System.Drawing.Point(12, 2);
            this.localPictureBox.Name = "localPictureBox";
            this.localPictureBox.Size = new System.Drawing.Size(256, 256);
            this.localPictureBox.TabIndex = 1;
            this.localPictureBox.TabStop = false;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "box_blue");
            // 
            // heightRadioButton
            // 
            this.heightRadioButton.AutoSize = true;
            this.heightRadioButton.Checked = true;
            this.heightRadioButton.Location = new System.Drawing.Point(792, 12);
            this.heightRadioButton.Name = "heightRadioButton";
            this.heightRadioButton.Size = new System.Drawing.Size(56, 17);
            this.heightRadioButton.TabIndex = 6;
            this.heightRadioButton.TabStop = true;
            this.heightRadioButton.Text = "Height";
            this.heightRadioButton.UseVisualStyleBackColor = true;
            this.heightRadioButton.CheckedChanged += new System.EventHandler(this.OnRadioButtonCheckedChanged);
            // 
            // tempRadioButton
            // 
            this.tempRadioButton.AutoSize = true;
            this.tempRadioButton.Location = new System.Drawing.Point(792, 35);
            this.tempRadioButton.Name = "tempRadioButton";
            this.tempRadioButton.Size = new System.Drawing.Size(52, 17);
            this.tempRadioButton.TabIndex = 7;
            this.tempRadioButton.Text = "Temp";
            this.tempRadioButton.UseVisualStyleBackColor = true;
            this.tempRadioButton.CheckedChanged += new System.EventHandler(this.OnRadioButtonCheckedChanged);
            // 
            // bioRadioButton
            // 
            this.bioRadioButton.AutoSize = true;
            this.bioRadioButton.Location = new System.Drawing.Point(792, 58);
            this.bioRadioButton.Name = "bioRadioButton";
            this.bioRadioButton.Size = new System.Drawing.Size(72, 17);
            this.bioRadioButton.TabIndex = 8;
            this.bioRadioButton.Text = "Biosphere";
            this.bioRadioButton.UseVisualStyleBackColor = true;
            this.bioRadioButton.CheckedChanged += new System.EventHandler(this.OnRadioButtonCheckedChanged);
            // 
            // waterRadioButton
            // 
            this.waterRadioButton.AutoSize = true;
            this.waterRadioButton.Location = new System.Drawing.Point(792, 81);
            this.waterRadioButton.Name = "waterRadioButton";
            this.waterRadioButton.Size = new System.Drawing.Size(54, 17);
            this.waterRadioButton.TabIndex = 9;
            this.waterRadioButton.Text = "Water";
            this.waterRadioButton.UseVisualStyleBackColor = true;
            this.waterRadioButton.CheckedChanged += new System.EventHandler(this.OnRadioButtonCheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 586);
            this.Controls.Add(this.waterRadioButton);
            this.Controls.Add(this.bioRadioButton);
            this.Controls.Add(this.tempRadioButton);
            this.Controls.Add(this.heightRadioButton);
            this.Controls.Add(this.localPictureBox);
            this.Controls.Add(this.worldPictureBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "WorldViewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.worldPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.localPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox worldPictureBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox localPictureBox;
        //private System.Windows.Forms.PictureBox terrainPictureBox;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.RadioButton heightRadioButton;
        private System.Windows.Forms.RadioButton tempRadioButton;
        private System.Windows.Forms.RadioButton bioRadioButton;
        private System.Windows.Forms.RadioButton waterRadioButton;
    }
}

