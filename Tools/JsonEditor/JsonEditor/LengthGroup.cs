using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace JsonEditor
{
    public class LengthGroup
    {
        public LengthGroup()
        {
            Words = new List<String>();
        }
        public LengthGroup(String sWord)
        {
            Words = new List<String>();
            InsertWord(sWord);
            Length = sWord.Length;
        }
        public bool WordExists(String sWord)
        {
            String sExistingWord = FindWord(sWord);
            return (sExistingWord != null);
        }
        public bool InsertWord(String sWord)
        {
            if (WordExists(sWord))
            {
                Form1.LogError("Word " + sWord + " already exists");
                return false;
            }
            Words.Add(sWord);
            return true;
        }
        public bool DeleteWord(String sWord)
        {
            if (!WordExists(sWord))
            {
                Form1.LogError("Word " + sWord + " does not exist");
                return false;
            }
            Words.Remove(sWord);
            return true;
        }
        public bool UpdateWord(String sOldValue, String sNewValue)
        {
            Debug.Assert(sOldValue.Length == sNewValue.Length);

            String sWord = FindWord(sOldValue);
            if (sWord != null)
            { 
                sWord = sNewValue;
                return true;
            }
            return false;
        }
        private String FindWord(String sWord)
        {
            Debug.Assert(sWord.Length > 0);

            if (sWord.Length < Config.m_iMinWordLength || sWord.Length > Config.m_iMaxWordLength)
            {
                Form1.LogError("Word length " + sWord.Length.ToString() + "is not allowed. Look at app config");
                return null;
            }
            for (int i = 0; i < Words.Count; i++)
                if (Words[i] == sWord)
                    return Words[i];
            return null;
        }

        public int Length { get; set; } // m_iLength
        public List<String> Words { get; set; } // m_aWords
    }
}
