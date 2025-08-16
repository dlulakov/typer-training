namespace Typer_Training
{
    partial class AddScore
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
            this.cancelScore = new System.Windows.Forms.Button();
            this.btnAddScore = new System.Windows.Forms.Button();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lbAddScoreWpm = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancelScore
            // 
            this.cancelScore.Location = new System.Drawing.Point(26, 103);
            this.cancelScore.Name = "cancelScore";
            this.cancelScore.Size = new System.Drawing.Size(75, 23);
            this.cancelScore.TabIndex = 0;
            this.cancelScore.Text = "Cancel";
            this.cancelScore.UseVisualStyleBackColor = true;
            this.cancelScore.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAddScore
            // 
            this.btnAddScore.Location = new System.Drawing.Point(148, 103);
            this.btnAddScore.Name = "btnAddScore";
            this.btnAddScore.Size = new System.Drawing.Size(75, 23);
            this.btnAddScore.TabIndex = 1;
            this.btnAddScore.Text = "Add";
            this.btnAddScore.UseVisualStyleBackColor = true;
            this.btnAddScore.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(26, 31);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(197, 20);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(23, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name";
            // 
            // lbAddScoreWpm
            // 
            this.lbAddScoreWpm.AutoEllipsis = true;
            this.lbAddScoreWpm.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAddScoreWpm.Location = new System.Drawing.Point(24, 67);
            this.lbAddScoreWpm.Name = "lbAddScoreWpm";
            this.lbAddScoreWpm.Size = new System.Drawing.Size(199, 33);
            this.lbAddScoreWpm.TabIndex = 15;
            this.lbAddScoreWpm.Text = "WPM: ";
            this.lbAddScoreWpm.Visible = false;
            this.lbAddScoreWpm.Click += new System.EventHandler(this.lbWpm_Click);
            // 
            // AddScore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 164);
            this.Controls.Add(this.lbAddScoreWpm);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.btnAddScore);
            this.Controls.Add(this.cancelScore);
            this.Name = "AddScore";
            this.Text = "Add Score";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelScore;
        private System.Windows.Forms.Button btnAddScore;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lbAddScoreWpm;
    }
}