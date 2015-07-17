namespace OAH_Evaluation
{
    partial class TaskDisplay
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
            this.labelTaskDesc = new System.Windows.Forms.Label();
            this.trackBarScale = new System.Windows.Forms.TrackBar();
            this.labelLeftMost = new System.Windows.Forms.Label();
            this.labelRightMost = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTaskDesc
            // 
            this.labelTaskDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTaskDesc.Font = new System.Drawing.Font("メイリオ", 23.85276F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelTaskDesc.Location = new System.Drawing.Point(0, 0);
            this.labelTaskDesc.Name = "labelTaskDesc";
            this.labelTaskDesc.Size = new System.Drawing.Size(1044, 250);
            this.labelTaskDesc.TabIndex = 0;
            this.labelTaskDesc.Text = "課題";
            // 
            // trackBarScale
            // 
            this.trackBarScale.Location = new System.Drawing.Point(224, 342);
            this.trackBarScale.Maximum = 1000;
            this.trackBarScale.Name = "trackBarScale";
            this.trackBarScale.Size = new System.Drawing.Size(616, 77);
            this.trackBarScale.TabIndex = 1;
            this.trackBarScale.TickFrequency = 0;
            this.trackBarScale.Value = 500;
            // 
            // labelLeftMost
            // 
            this.labelLeftMost.AutoSize = true;
            this.labelLeftMost.Font = new System.Drawing.Font("メイリオ", 23.85276F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelLeftMost.Location = new System.Drawing.Point(12, 322);
            this.labelLeftMost.Name = "labelLeftMost";
            this.labelLeftMost.Size = new System.Drawing.Size(200, 82);
            this.labelLeftMost.TabIndex = 2;
            this.labelLeftMost.Text = "開放的";
            // 
            // labelRightMost
            // 
            this.labelRightMost.AutoSize = true;
            this.labelRightMost.Font = new System.Drawing.Font("メイリオ", 23.85276F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelRightMost.Location = new System.Drawing.Point(829, 322);
            this.labelRightMost.Name = "labelRightMost";
            this.labelRightMost.Size = new System.Drawing.Size(200, 82);
            this.labelRightMost.TabIndex = 3;
            this.labelRightMost.Text = "閉塞的";
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonOK.Font = new System.Drawing.Font("MS UI Gothic", 27.82822F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonOK.Location = new System.Drawing.Point(0, 596);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(1044, 93);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // TaskDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 689);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelRightMost);
            this.Controls.Add(this.labelLeftMost);
            this.Controls.Add(this.trackBarScale);
            this.Controls.Add(this.labelTaskDesc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TaskDisplay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TaskDisplay";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label labelTaskDesc;
        public System.Windows.Forms.TrackBar trackBarScale;
        public System.Windows.Forms.Label labelLeftMost;
        public System.Windows.Forms.Label labelRightMost;
        public System.Windows.Forms.Button buttonOK;
    }
}