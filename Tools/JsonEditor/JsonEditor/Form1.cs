using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.IO;
using System.Diagnostics;

namespace JsonEditor
{
    public partial class Form1 : Form
    {
#if DEBUG
        private const String m_sFilePath = "../../../words.json";
#else
        private const String m_sFilePath = "words.json";
#endif
        WordCategory m_oWordCategories = new WordCategory();

        public Form1()
        {
            InitializeComponent();

            TextBoxCategories.Text = "";
            TextBoxWords.Text = "";
            
            LoadDictionary();
            UpdateCategoriesList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void UpdateCategoriesList()
        {
            ListBoxCategories.Items.Clear();
            foreach (TypeGroup oTypeGroup in m_oWordCategories.TypeGroups)
                ListBoxCategories.Items.Add(oTypeGroup.Name);
        }

        private void UpdateWordsList(String sCategoryName)
        {
            ListBoxWords.Items.Clear();
            List<String> aWords = m_oWordCategories.GetWords(sCategoryName);
            if (aWords != null)
            { 
                foreach (String sWord in aWords)
                ListBoxWords.Items.Add(sWord);
            }
        }

        private void LoadDictionary()
        {
            if (!File.Exists(m_sFilePath))
                return;

            try
            {
                String sJson = File.ReadAllText(m_sFilePath);
                JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                oSerializer.MaxJsonLength = Config.m_iMaxJsonLength;
                m_oWordCategories = oSerializer.Deserialize<WordCategory>(sJson);
                if (m_oWordCategories == null)
                {
                    m_oWordCategories = new WordCategory();
                }
            }
            catch (Exception oException)
            {
                LogError(oException.Message);
            }
        }

        private void SaveDictionary()
        {
            try
            {
                JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                oSerializer.MaxJsonLength = Config.m_iMaxJsonLength;
                String sJson = oSerializer.Serialize(m_oWordCategories);
                File.WriteAllText(m_sFilePath, sJson);
            }
            catch (Exception oException)
            {
                LogError(oException.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveDictionary();
        }

        public static void LogError(String sMessage)
        {
            MessageBox.Show(sMessage);
        }

        public static void LogWarning(String sMessage)
        {
            MessageBox.Show(sMessage);
        }

        private void ButtonAddCategory_Click(object sender, EventArgs e)
        {
            if (TextBoxCategories.Text.Length == 0)
            {
                LogWarning("New category is empty");
                return;
            }
            if (m_oWordCategories.InsertTypeGroup(TextBoxCategories.Text))
            {
                UpdateCategoriesList();
                TextBoxCategories.Clear();
            }
            TextBoxCategories.Focus();
        }

        private void TextBoxCategories_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ButtonAddCategory_Click(sender, e);
            }
        }

        private void ButtonAddWord_Click(object sender, EventArgs e)
        {
            if (TextBoxWords.Text.Length == 0)
            {
                LogWarning("New word is empty");
                return;
            }
            if (ListBoxCategories.SelectedItem == null)
            {
                LogWarning("Please select a category");
                return;
            }
            String sSelectedTypeGroup = ListBoxCategories.SelectedItem.ToString();
            if (m_oWordCategories.InsertWord(sSelectedTypeGroup, TextBoxWords.Text))
            {
                UpdateWordsList(sSelectedTypeGroup);
                TextBoxWords.Clear();
            }
            TextBoxWords.Focus();
        }

        private void TextBoxWords_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ButtonAddWord_Click(sender, e);
            }
        }

        private void ListBoxCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBoxCategories.SelectedItem == null)
                return;

            String sSelectedTypeGroup = ListBoxCategories.SelectedItem.ToString();
            UpdateWordsList(sSelectedTypeGroup);
        }

        private void ButtonAddSeveralWords_Click(object sender, EventArgs e)
        {
            if (TextBoxSeveralWords.Lines.Length == 0)
            {
                LogWarning("Please add some words");
                return;
            }
            if (ListBoxCategories.SelectedItem == null)
            {
                LogWarning("Please select a category");
                return;
            }
            String sSelectedTypeGroup = ListBoxCategories.SelectedItem.ToString();
            foreach (String sWord in TextBoxSeveralWords.Lines)
            {
                if (sWord.Length > 0)
                    m_oWordCategories.InsertWord(sSelectedTypeGroup, sWord);
            }
            UpdateWordsList(sSelectedTypeGroup);
            TextBoxSeveralWords.Clear();
        }

        private void ButtonDeleteCategory_Click(object sender, EventArgs e)
        {
            if (ListBoxCategories.SelectedItem == null)
            {
                LogWarning("Please select a category");
                return;
            }
            String sSelectedTypeGroup = ListBoxCategories.SelectedItem.ToString();
            m_oWordCategories.DeleteCategory(sSelectedTypeGroup);
            UpdateCategoriesList();
            ListBoxWords.Items.Clear();
        }

        private void ButtonDeleteWord_Click(object sender, EventArgs e)
        {
            if (ListBoxCategories.SelectedItem == null)
            {
                LogWarning("Please select a category");
                return;
            }
            if (ListBoxWords.SelectedItem == null)
            {
                LogWarning("Please select a word");
                return;
            }
            String sSelectedTypeGroup = ListBoxCategories.SelectedItem.ToString();
            String sSelectedWord = ListBoxWords.SelectedItem.ToString();
            m_oWordCategories.DeleteWord(sSelectedTypeGroup, sSelectedWord);
            UpdateWordsList(sSelectedTypeGroup);
        }

        private void ButtonUpdateCategory_Click(object sender, EventArgs e)
        {
            if (ListBoxCategories.SelectedItem == null)
            {
                LogWarning("Please select a category");
                return;
            }
            if (TextBoxCategories.Text.Length == 0)
            {
                LogWarning("New category name is empty");
                return;
            }
            String sSelectedTypeGroup = ListBoxCategories.SelectedItem.ToString();
            if (m_oWordCategories.UpdateCategory(sSelectedTypeGroup, TextBoxCategories.Text))
            {
                TextBoxCategories.Clear();
                UpdateCategoriesList();
            }
        }

        private void ButtonUpdateWord_Click(object sender, EventArgs e)
        {
            if (ListBoxCategories.SelectedItem == null)
            {
                LogWarning("Please select a category");
                return;
            }
            if (ListBoxWords.SelectedItem == null)
            {
                LogWarning("Please select a word");
                return;
            }
            if (TextBoxWords.Text.Length == 0)
            {
                LogWarning("New word is empty");
                return;
            }
            String sSelectedTypeGroup = ListBoxCategories.SelectedItem.ToString();
            String sSelectedWord = ListBoxWords.SelectedItem.ToString();
            if (m_oWordCategories.UpdateWord(sSelectedTypeGroup, sSelectedWord, TextBoxWords.Text))
            {
                TextBoxWords.Clear();
                UpdateWordsList(sSelectedTypeGroup);
            }
        }
    }
}
