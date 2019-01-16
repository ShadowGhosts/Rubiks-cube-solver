namespace Rubiks_cube_solver_app
{
    partial class ColorSelect
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
            this.Green = new System.Windows.Forms.Button();
            this.Orange = new System.Windows.Forms.Button();
            this.Blue = new System.Windows.Forms.Button();
            this.Yellow = new System.Windows.Forms.Button();
            this.Red = new System.Windows.Forms.Button();
            this.White = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Green
            // 
            this.Green.BackColor = System.Drawing.Color.Green;
            this.Green.Location = new System.Drawing.Point(30, 30);
            this.Green.Name = "Green";
            this.Green.Size = new System.Drawing.Size(100, 100);
            this.Green.TabIndex = 0;
            this.Green.UseVisualStyleBackColor = false;
            this.Green.Click += new System.EventHandler(this.Green_Click);
            // 
            // Orange
            // 
            this.Orange.BackColor = System.Drawing.Color.Orange;
            this.Orange.Location = new System.Drawing.Point(140, 30);
            this.Orange.Name = "Orange";
            this.Orange.Size = new System.Drawing.Size(100, 100);
            this.Orange.TabIndex = 1;
            this.Orange.UseVisualStyleBackColor = false;
            this.Orange.Click += new System.EventHandler(this.Orange_Click);
            // 
            // Blue
            // 
            this.Blue.BackColor = System.Drawing.Color.Blue;
            this.Blue.Location = new System.Drawing.Point(250, 30);
            this.Blue.Name = "Blue";
            this.Blue.Size = new System.Drawing.Size(100, 100);
            this.Blue.TabIndex = 2;
            this.Blue.UseVisualStyleBackColor = false;
            this.Blue.Click += new System.EventHandler(this.Blue_Click);
            // 
            // Yellow
            // 
            this.Yellow.BackColor = System.Drawing.Color.Yellow;
            this.Yellow.Location = new System.Drawing.Point(30, 140);
            this.Yellow.Name = "Yellow";
            this.Yellow.Size = new System.Drawing.Size(100, 100);
            this.Yellow.TabIndex = 3;
            this.Yellow.UseVisualStyleBackColor = false;
            this.Yellow.Click += new System.EventHandler(this.Yellow_Click);
            // 
            // Red
            // 
            this.Red.BackColor = System.Drawing.Color.Red;
            this.Red.Location = new System.Drawing.Point(140, 140);
            this.Red.Name = "Red";
            this.Red.Size = new System.Drawing.Size(100, 100);
            this.Red.TabIndex = 4;
            this.Red.UseVisualStyleBackColor = false;
            this.Red.Click += new System.EventHandler(this.Red_Click);
            // 
            // White
            // 
            this.White.BackColor = System.Drawing.Color.White;
            this.White.Location = new System.Drawing.Point(250, 140);
            this.White.Name = "White";
            this.White.Size = new System.Drawing.Size(100, 100);
            this.White.TabIndex = 5;
            this.White.UseVisualStyleBackColor = false;
            this.White.Click += new System.EventHandler(this.White_Click);
            // 
            // ColorSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 271);
            this.Controls.Add(this.White);
            this.Controls.Add(this.Red);
            this.Controls.Add(this.Yellow);
            this.Controls.Add(this.Blue);
            this.Controls.Add(this.Orange);
            this.Controls.Add(this.Green);
            this.Name = "ColorSelect";
            this.Text = "ColorSelect";
            this.Load += new System.EventHandler(this.ColorSelect_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Green;
        private System.Windows.Forms.Button Orange;
        private System.Windows.Forms.Button Blue;
        private System.Windows.Forms.Button Yellow;
        private System.Windows.Forms.Button Red;
        private System.Windows.Forms.Button White;
    }
}