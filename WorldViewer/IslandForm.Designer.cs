namespace WorldViewer
{
    partial class IslandForm
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
            this.updateButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.param1 = new System.Windows.Forms.NumericUpDown();
            this.param2 = new System.Windows.Forms.NumericUpDown();
            this.param3 = new System.Windows.Forms.NumericUpDown();
            this.param4 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.param5 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.param6 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.param7 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.param1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.param2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.param3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.param4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.param5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.param6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.param7)).BeginInit();
            this.SuspendLayout();
            // 
            // updateButton
            // 
            this.updateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.updateButton.Location = new System.Drawing.Point(608, 12);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 2;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 512);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // param1
            // 
            this.param1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.param1.DecimalPlaces = 2;
            this.param1.Location = new System.Drawing.Point(608, 41);
            this.param1.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.param1.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.param1.Name = "param1";
            this.param1.Size = new System.Drawing.Size(75, 20);
            this.param1.TabIndex = 7;
            // 
            // param2
            // 
            this.param2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.param2.DecimalPlaces = 2;
            this.param2.Location = new System.Drawing.Point(608, 67);
            this.param2.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.param2.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.param2.Name = "param2";
            this.param2.Size = new System.Drawing.Size(75, 20);
            this.param2.TabIndex = 8;
            // 
            // param3
            // 
            this.param3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.param3.DecimalPlaces = 2;
            this.param3.Location = new System.Drawing.Point(608, 93);
            this.param3.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.param3.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.param3.Name = "param3";
            this.param3.Size = new System.Drawing.Size(75, 20);
            this.param3.TabIndex = 9;
            // 
            // param4
            // 
            this.param4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.param4.DecimalPlaces = 2;
            this.param4.Location = new System.Drawing.Point(608, 119);
            this.param4.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.param4.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.param4.Name = "param4";
            this.param4.Size = new System.Drawing.Size(75, 20);
            this.param4.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(540, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "WaterLevel";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(540, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Octaves";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(540, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "X Offset";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(540, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Frequency";
            // 
            // param5
            // 
            this.param5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.param5.DecimalPlaces = 2;
            this.param5.Location = new System.Drawing.Point(608, 145);
            this.param5.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.param5.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.param5.Name = "param5";
            this.param5.Size = new System.Drawing.Size(75, 20);
            this.param5.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(540, 201);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "6";
            // 
            // param6
            // 
            this.param6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.param6.DecimalPlaces = 2;
            this.param6.Location = new System.Drawing.Point(608, 199);
            this.param6.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.param6.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.param6.Name = "param6";
            this.param6.Size = new System.Drawing.Size(75, 20);
            this.param6.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(540, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "7";
            // 
            // param7
            // 
            this.param7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.param7.DecimalPlaces = 2;
            this.param7.Location = new System.Drawing.Point(608, 225);
            this.param7.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.param7.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.param7.Name = "param7";
            this.param7.Size = new System.Drawing.Size(75, 20);
            this.param7.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(540, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Z Offset";
            // 
            // IslandForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 567);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.param7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.param6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.param5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.param4);
            this.Controls.Add(this.param3);
            this.Controls.Add(this.param2);
            this.Controls.Add(this.param1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.updateButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IslandForm";
            this.Text = "IslandForm";
            this.Load += new System.EventHandler(this.IslandForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.param1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.param2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.param3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.param4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.param5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.param6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.param7)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown param1;
        private System.Windows.Forms.NumericUpDown param2;
        private System.Windows.Forms.NumericUpDown param3;
        private System.Windows.Forms.NumericUpDown param4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown param5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown param6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown param7;
        private System.Windows.Forms.Label label5;
    }
}