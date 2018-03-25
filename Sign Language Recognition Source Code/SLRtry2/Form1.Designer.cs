namespace SLRtry2
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
            this.lblName = new System.Windows.Forms.Label();
            this.DepthPicture = new System.Windows.Forms.PictureBox();
            this.RGBPicture = new System.Windows.Forms.PictureBox();
            this.Startbutton = new System.Windows.Forms.Button();
            this.SignTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.DepthPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RGBPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblName.Font = new System.Drawing.Font("Palatino Linotype", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.Green;
            this.lblName.Location = new System.Drawing.Point(369, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(425, 44);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Sign Language Recognition";
            // 
            // DepthPicture
            // 
            this.DepthPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DepthPicture.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.DepthPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DepthPicture.Location = new System.Drawing.Point(24, 60);
            this.DepthPicture.Name = "DepthPicture";
            this.DepthPicture.Size = new System.Drawing.Size(660, 434);
            this.DepthPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DepthPicture.TabIndex = 1;
            this.DepthPicture.TabStop = false;
            this.DepthPicture.Click += new System.EventHandler(this.DepthPicture_Click_1);
            // 
            // RGBPicture
            // 
            this.RGBPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RGBPicture.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.RGBPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.RGBPicture.Location = new System.Drawing.Point(718, 153);
            this.RGBPicture.Name = "RGBPicture";
            this.RGBPicture.Size = new System.Drawing.Size(385, 372);
            this.RGBPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RGBPicture.TabIndex = 2;
            this.RGBPicture.TabStop = false;
            this.RGBPicture.Click += new System.EventHandler(this.RGBPicture_Click_1);
            // 
            // Startbutton
            // 
            this.Startbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Startbutton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Startbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Startbutton.ForeColor = System.Drawing.Color.White;
            this.Startbutton.Location = new System.Drawing.Point(848, 71);
            this.Startbutton.Name = "Startbutton";
            this.Startbutton.Size = new System.Drawing.Size(173, 64);
            this.Startbutton.TabIndex = 3;
            this.Startbutton.Text = "Start Stream";
            this.Startbutton.UseVisualStyleBackColor = false;
            this.Startbutton.Click += new System.EventHandler(this.Startbutton_Click_1);
            // 
            // SignTextBox
            // 
            this.SignTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SignTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.SignTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignTextBox.ForeColor = System.Drawing.Color.White;
            this.SignTextBox.Location = new System.Drawing.Point(12, 500);
            this.SignTextBox.Name = "SignTextBox";
            this.SignTextBox.Size = new System.Drawing.Size(700, 46);
            this.SignTextBox.TabIndex = 4;
            this.SignTextBox.Text = "";
            this.SignTextBox.TextChanged += new System.EventHandler(this.SignTextBox_TextChanged_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(1115, 549);
            this.Controls.Add(this.SignTextBox);
            this.Controls.Add(this.Startbutton);
            this.Controls.Add(this.RGBPicture);
            this.Controls.Add(this.DepthPicture);
            this.Controls.Add(this.lblName);
            this.Name = "Form1";
            this.Text = "SLR";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DepthPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RGBPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.PictureBox DepthPicture;
        private System.Windows.Forms.PictureBox RGBPicture;
        private System.Windows.Forms.Button Startbutton;
        private System.Windows.Forms.RichTextBox SignTextBox;
    }
}

