namespace FineTuningToolWinFormsAP
{
    partial class mainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblSystem = new Label();
            txtSystem = new TextBox();
            btnInsert = new Button();
            lalUser = new Label();
            txtUser = new TextBox();
            lblAssistant = new Label();
            txtAssistant = new TextBox();
            btnExport = new Button();
            SuspendLayout();
            // 
            // lblSystem
            // 
            lblSystem.AutoSize = true;
            lblSystem.Location = new Point(73, 55);
            lblSystem.Name = "lblSystem";
            lblSystem.Size = new Size(120, 38);
            lblSystem.TabIndex = 0;
            lblSystem.Text = "System";
            // 
            // txtSystem
            // 
            txtSystem.Location = new Point(245, 55);
            txtSystem.Name = "txtSystem";
            txtSystem.Size = new Size(902, 46);
            txtSystem.TabIndex = 1;
            // 
            // btnInsert
            // 
            btnInsert.Location = new Point(73, 459);
            btnInsert.Name = "btnInsert";
            btnInsert.Size = new Size(188, 58);
            btnInsert.TabIndex = 3;
            btnInsert.Text = "寫入";
            btnInsert.UseVisualStyleBackColor = true;
            btnInsert.Click += btnInsert_Click;
            // 
            // lalUser
            // 
            lalUser.AutoSize = true;
            lalUser.Location = new Point(73, 139);
            lalUser.Name = "lalUser";
            lalUser.Size = new Size(81, 38);
            lalUser.TabIndex = 0;
            lalUser.Text = "User";
            // 
            // txtUser
            // 
            txtUser.Location = new Point(245, 131);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(902, 46);
            txtUser.TabIndex = 1;
            // 
            // lblAssistant
            // 
            lblAssistant.AutoSize = true;
            lblAssistant.Location = new Point(73, 227);
            lblAssistant.Name = "lblAssistant";
            lblAssistant.Size = new Size(144, 38);
            lblAssistant.TabIndex = 0;
            lblAssistant.Text = "Assistant";
            // 
            // txtAssistant
            // 
            txtAssistant.Location = new Point(245, 219);
            txtAssistant.Multiline = true;
            txtAssistant.Name = "txtAssistant";
            txtAssistant.ScrollBars = ScrollBars.Vertical;
            txtAssistant.Size = new Size(902, 200);
            txtAssistant.TabIndex = 1;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(345, 459);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(188, 58);
            btnExport.TabIndex = 4;
            btnExport.Text = "匯出";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // mainForm
            // 
            AutoScaleDimensions = new SizeF(18F, 38F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1310, 566);
            Controls.Add(btnExport);
            Controls.Add(btnInsert);
            Controls.Add(txtAssistant);
            Controls.Add(txtUser);
            Controls.Add(txtSystem);
            Controls.Add(lblAssistant);
            Controls.Add(lalUser);
            Controls.Add(lblSystem);
            Name = "mainForm";
            Text = "主畫面";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblSystem;
        private TextBox txtSystem;
        private Button btnInsert;
        private Label lalUser;
        private TextBox txtUser;
        private Label lblAssistant;
        private TextBox txtAssistant;
        private Button btnExport;
    }
}
