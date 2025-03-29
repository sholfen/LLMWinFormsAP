namespace LLMWinFormsAP
{
    partial class MainForm
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
            btnSend = new Button();
            txtResponse = new TextBox();
            txtPrompt = new TextBox();
            btnStop = new Button();
            picConver = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picConver).BeginInit();
            SuspendLayout();
            // 
            // btnSend
            // 
            btnSend.Location = new Point(263, 528);
            btnSend.Margin = new Padding(1);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(73, 23);
            btnSend.TabIndex = 0;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // txtResponse
            // 
            txtResponse.Location = new Point(14, 285);
            txtResponse.Margin = new Padding(1);
            txtResponse.Multiline = true;
            txtResponse.Name = "txtResponse";
            txtResponse.ScrollBars = ScrollBars.Vertical;
            txtResponse.Size = new Size(436, 225);
            txtResponse.TabIndex = 2;
            txtResponse.Text = "Answer";
            // 
            // txtPrompt
            // 
            txtPrompt.Location = new Point(14, 30);
            txtPrompt.Margin = new Padding(1);
            txtPrompt.Multiline = true;
            txtPrompt.Name = "txtPrompt";
            txtPrompt.Size = new Size(436, 225);
            txtPrompt.TabIndex = 3;
            txtPrompt.Text = "請妳自我介紹";
            // 
            // btnStop
            // 
            btnStop.Location = new Point(417, 528);
            btnStop.Margin = new Padding(1);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(73, 23);
            btnStop.TabIndex = 4;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // picConver
            // 
            picConver.BackgroundImageLayout = ImageLayout.None;
            picConver.Image = Properties.Resources._0228_800x1200_nofix_00010_;
            picConver.Location = new Point(464, 30);
            picConver.Margin = new Padding(1);
            picConver.Name = "picConver";
            picConver.Size = new Size(311, 474);
            picConver.SizeMode = PictureBoxSizeMode.StretchImage;
            picConver.TabIndex = 5;
            picConver.TabStop = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(794, 565);
            Controls.Add(picConver);
            Controls.Add(btnStop);
            Controls.Add(txtPrompt);
            Controls.Add(txtResponse);
            Controls.Add(btnSend);
            Margin = new Padding(1);
            Name = "MainForm";
            Text = "AI Demo";
            ((System.ComponentModel.ISupportInitialize)picConver).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSend;
        private TextBox txtResponse;
        private TextBox txtPrompt;
        private Button btnStop;
        private PictureBox picConver;
    }
}
