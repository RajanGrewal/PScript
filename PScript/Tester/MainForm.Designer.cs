namespace Tester
{
    partial class MainForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.m_syntaxRichTextBox = new Tester.RichTextBoxEx();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 226);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(260, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Execute";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // m_syntaxRichTextBox
            // 
            this.m_syntaxRichTextBox.Location = new System.Drawing.Point(12, 12);
            this.m_syntaxRichTextBox.Name = "m_syntaxRichTextBox";
            this.m_syntaxRichTextBox.Size = new System.Drawing.Size(260, 208);
            this.m_syntaxRichTextBox.TabIndex = 2;
            this.m_syntaxRichTextBox.Text = "$txt = \"silly\";\n$old_num = 911;\n\n$new_num = old_num;\ntxt = \"funny\";\n\n\n$final = ms" +
    "gbox(txt,new_num);\n\nlog(final);\nmsgbox(msgbox(final,4),5);\nnothing();";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.m_syntaxRichTextBox);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private RichTextBoxEx m_syntaxRichTextBox;
    }
}

