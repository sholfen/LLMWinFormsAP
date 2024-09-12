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
            SuspendLayout();
            // 
            // btnSend
            // 
            btnSend.Location = new Point(892, 1350);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(188, 58);
            btnSend.TabIndex = 0;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // txtResponse
            // 
            txtResponse.Location = new Point(35, 721);
            txtResponse.Multiline = true;
            txtResponse.Name = "txtResponse";
            txtResponse.ScrollBars = ScrollBars.Vertical;
            txtResponse.Size = new Size(1975, 564);
            txtResponse.TabIndex = 2;
            txtResponse.Text = "Answer";
            // 
            // txtPrompt
            // 
            txtPrompt.Location = new Point(35, 77);
            txtPrompt.Multiline = true;
            txtPrompt.Name = "txtPrompt";
            txtPrompt.Size = new Size(1975, 564);
            txtPrompt.TabIndex = 3;
            txtPrompt.Text = "Prompt: Who is Michael Jackson";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(18F, 38F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2055, 1435);
            Controls.Add(txtPrompt);
            Controls.Add(txtResponse);
            Controls.Add(btnSend);
            Name = "MainForm";
            Text = "AI Demo";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSend;
        private TextBox txtResponse;
        private TextBox txtPrompt;
    }
}
