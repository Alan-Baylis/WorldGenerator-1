namespace WorldViewer
{
    partial class ParametersForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.param1TextBox = new System.Windows.Forms.TextBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.param2TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.param3TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.param4TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.param5TextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Parameter 1";
            // 
            // param1TextBox
            // 
            this.param1TextBox.Location = new System.Drawing.Point(107, 6);
            this.param1TextBox.Name = "param1TextBox";
            this.param1TextBox.Size = new System.Drawing.Size(100, 20);
            this.param1TextBox.TabIndex = 1;
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(132, 136);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 2;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // param2TextBox
            // 
            this.param2TextBox.Location = new System.Drawing.Point(107, 32);
            this.param2TextBox.Name = "param2TextBox";
            this.param2TextBox.Size = new System.Drawing.Size(100, 20);
            this.param2TextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Parameter 2";
            // 
            // param3TextBox
            // 
            this.param3TextBox.Location = new System.Drawing.Point(107, 58);
            this.param3TextBox.Name = "param3TextBox";
            this.param3TextBox.Size = new System.Drawing.Size(100, 20);
            this.param3TextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Parameter 3";
            // 
            // param4TextBox
            // 
            this.param4TextBox.Location = new System.Drawing.Point(107, 84);
            this.param4TextBox.Name = "param4TextBox";
            this.param4TextBox.Size = new System.Drawing.Size(100, 20);
            this.param4TextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Parameter 4";
            // 
            // param5TextBox
            // 
            this.param5TextBox.Location = new System.Drawing.Point(107, 110);
            this.param5TextBox.Name = "param5TextBox";
            this.param5TextBox.Size = new System.Drawing.Size(100, 20);
            this.param5TextBox.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Parameter 5";
            // 
            // ParametersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 167);
            this.Controls.Add(this.param5TextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.param4TextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.param3TextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.param2TextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.param1TextBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParametersForm";
            this.Text = "ParametersForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox param1TextBox;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.TextBox param2TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox param3TextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox param4TextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox param5TextBox;
        private System.Windows.Forms.Label label5;
    }
}