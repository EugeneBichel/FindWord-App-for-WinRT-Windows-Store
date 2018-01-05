namespace JsonEditor
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
            this.ListBoxCategories = new System.Windows.Forms.ListBox();
            this.TextBoxCategories = new System.Windows.Forms.TextBox();
            this.ButtonInsertCategory = new System.Windows.Forms.Button();
            this.ButtonDeleteCategory = new System.Windows.Forms.Button();
            this.ListBoxWords = new System.Windows.Forms.ListBox();
            this.ButtonDeleteWord = new System.Windows.Forms.Button();
            this.ButtonInsertWord = new System.Windows.Forms.Button();
            this.TextBoxWords = new System.Windows.Forms.TextBox();
            this.TextBoxSeveralWords = new System.Windows.Forms.TextBox();
            this.ButtonInsertSeveralWords = new System.Windows.Forms.Button();
            this.ButtonUpdateCategory = new System.Windows.Forms.Button();
            this.ButtonUpdateWord = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ListBoxCategories
            // 
            this.ListBoxCategories.FormattingEnabled = true;
            this.ListBoxCategories.Location = new System.Drawing.Point(12, 12);
            this.ListBoxCategories.Name = "ListBoxCategories";
            this.ListBoxCategories.ScrollAlwaysVisible = true;
            this.ListBoxCategories.Size = new System.Drawing.Size(239, 251);
            this.ListBoxCategories.TabIndex = 0;
            this.ListBoxCategories.SelectedIndexChanged += new System.EventHandler(this.ListBoxCategories_SelectedIndexChanged);
            // 
            // TextBoxCategories
            // 
            this.TextBoxCategories.Location = new System.Drawing.Point(12, 276);
            this.TextBoxCategories.Name = "TextBoxCategories";
            this.TextBoxCategories.Size = new System.Drawing.Size(239, 20);
            this.TextBoxCategories.TabIndex = 1;
            this.TextBoxCategories.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBoxCategories_KeyUp);
            // 
            // ButtonInsertCategory
            // 
            this.ButtonInsertCategory.Location = new System.Drawing.Point(12, 302);
            this.ButtonInsertCategory.Name = "ButtonInsertCategory";
            this.ButtonInsertCategory.Size = new System.Drawing.Size(76, 23);
            this.ButtonInsertCategory.TabIndex = 2;
            this.ButtonInsertCategory.Text = "Insert";
            this.ButtonInsertCategory.UseVisualStyleBackColor = true;
            this.ButtonInsertCategory.Click += new System.EventHandler(this.ButtonAddCategory_Click);
            // 
            // ButtonDeleteCategory
            // 
            this.ButtonDeleteCategory.Location = new System.Drawing.Point(93, 302);
            this.ButtonDeleteCategory.Name = "ButtonDeleteCategory";
            this.ButtonDeleteCategory.Size = new System.Drawing.Size(76, 23);
            this.ButtonDeleteCategory.TabIndex = 3;
            this.ButtonDeleteCategory.Text = "Delete";
            this.ButtonDeleteCategory.UseVisualStyleBackColor = true;
            this.ButtonDeleteCategory.Click += new System.EventHandler(this.ButtonDeleteCategory_Click);
            // 
            // ListBoxWords
            // 
            this.ListBoxWords.FormattingEnabled = true;
            this.ListBoxWords.Location = new System.Drawing.Point(283, 12);
            this.ListBoxWords.Name = "ListBoxWords";
            this.ListBoxWords.ScrollAlwaysVisible = true;
            this.ListBoxWords.Size = new System.Drawing.Size(239, 251);
            this.ListBoxWords.TabIndex = 4;
            // 
            // ButtonDeleteWord
            // 
            this.ButtonDeleteWord.Location = new System.Drawing.Point(365, 302);
            this.ButtonDeleteWord.Name = "ButtonDeleteWord";
            this.ButtonDeleteWord.Size = new System.Drawing.Size(76, 23);
            this.ButtonDeleteWord.TabIndex = 7;
            this.ButtonDeleteWord.Text = "Delete";
            this.ButtonDeleteWord.UseVisualStyleBackColor = true;
            this.ButtonDeleteWord.Click += new System.EventHandler(this.ButtonDeleteWord_Click);
            // 
            // ButtonInsertWord
            // 
            this.ButtonInsertWord.Location = new System.Drawing.Point(283, 302);
            this.ButtonInsertWord.Name = "ButtonInsertWord";
            this.ButtonInsertWord.Size = new System.Drawing.Size(76, 23);
            this.ButtonInsertWord.TabIndex = 6;
            this.ButtonInsertWord.Text = "Insert";
            this.ButtonInsertWord.UseVisualStyleBackColor = true;
            this.ButtonInsertWord.Click += new System.EventHandler(this.ButtonAddWord_Click);
            // 
            // TextBoxWords
            // 
            this.TextBoxWords.Location = new System.Drawing.Point(283, 276);
            this.TextBoxWords.Name = "TextBoxWords";
            this.TextBoxWords.Size = new System.Drawing.Size(239, 20);
            this.TextBoxWords.TabIndex = 5;
            this.TextBoxWords.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBoxWords_KeyUp);
            // 
            // TextBoxSeveralWords
            // 
            this.TextBoxSeveralWords.Location = new System.Drawing.Point(529, 13);
            this.TextBoxSeveralWords.MaxLength = 67108864;
            this.TextBoxSeveralWords.Multiline = true;
            this.TextBoxSeveralWords.Name = "TextBoxSeveralWords";
            this.TextBoxSeveralWords.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBoxSeveralWords.Size = new System.Drawing.Size(239, 251);
            this.TextBoxSeveralWords.TabIndex = 8;
            // 
            // ButtonInsertSeveralWords
            // 
            this.ButtonInsertSeveralWords.Location = new System.Drawing.Point(529, 302);
            this.ButtonInsertSeveralWords.Name = "ButtonInsertSeveralWords";
            this.ButtonInsertSeveralWords.Size = new System.Drawing.Size(110, 23);
            this.ButtonInsertSeveralWords.TabIndex = 9;
            this.ButtonInsertSeveralWords.Text = "Add Several";
            this.ButtonInsertSeveralWords.UseVisualStyleBackColor = true;
            this.ButtonInsertSeveralWords.Click += new System.EventHandler(this.ButtonAddSeveralWords_Click);
            // 
            // ButtonUpdateCategory
            // 
            this.ButtonUpdateCategory.Location = new System.Drawing.Point(175, 302);
            this.ButtonUpdateCategory.Name = "ButtonUpdateCategory";
            this.ButtonUpdateCategory.Size = new System.Drawing.Size(76, 23);
            this.ButtonUpdateCategory.TabIndex = 10;
            this.ButtonUpdateCategory.Text = "Update";
            this.ButtonUpdateCategory.UseVisualStyleBackColor = true;
            this.ButtonUpdateCategory.Click += new System.EventHandler(this.ButtonUpdateCategory_Click);
            // 
            // ButtonUpdateWord
            // 
            this.ButtonUpdateWord.Location = new System.Drawing.Point(446, 302);
            this.ButtonUpdateWord.Name = "ButtonUpdateWord";
            this.ButtonUpdateWord.Size = new System.Drawing.Size(76, 23);
            this.ButtonUpdateWord.TabIndex = 11;
            this.ButtonUpdateWord.Text = "Update";
            this.ButtonUpdateWord.UseVisualStyleBackColor = true;
            this.ButtonUpdateWord.Click += new System.EventHandler(this.ButtonUpdateWord_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 333);
            this.Controls.Add(this.ButtonUpdateWord);
            this.Controls.Add(this.ButtonUpdateCategory);
            this.Controls.Add(this.ButtonInsertSeveralWords);
            this.Controls.Add(this.TextBoxSeveralWords);
            this.Controls.Add(this.ButtonDeleteWord);
            this.Controls.Add(this.ButtonInsertWord);
            this.Controls.Add(this.TextBoxWords);
            this.Controls.Add(this.ListBoxWords);
            this.Controls.Add(this.ButtonDeleteCategory);
            this.Controls.Add(this.ButtonInsertCategory);
            this.Controls.Add(this.TextBoxCategories);
            this.Controls.Add(this.ListBoxCategories);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ListBoxCategories;
        private System.Windows.Forms.TextBox TextBoxCategories;
        private System.Windows.Forms.Button ButtonInsertCategory;
        private System.Windows.Forms.Button ButtonDeleteCategory;
        private System.Windows.Forms.ListBox ListBoxWords;
        private System.Windows.Forms.Button ButtonDeleteWord;
        private System.Windows.Forms.Button ButtonInsertWord;
        private System.Windows.Forms.TextBox TextBoxWords;
        private System.Windows.Forms.TextBox TextBoxSeveralWords;
        private System.Windows.Forms.Button ButtonInsertSeveralWords;
        private System.Windows.Forms.Button ButtonUpdateCategory;
        private System.Windows.Forms.Button ButtonUpdateWord;

    }
}

