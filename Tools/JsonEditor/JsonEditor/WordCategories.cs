using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace JsonEditor
{
    public class WordCategory
    {
        public WordCategory()
        {
            TypeGroups = new List<TypeGroup>();
        }
        public bool TypeGroupExists(String sTypeName)
        {
            return (FindTypeGroup(sTypeName) != null);
        }
        public bool InsertTypeGroup(String sTypeName)
        {
            if (TypeGroupExists(sTypeName))
            {
                Form1.LogError("TypeName " + sTypeName + " already exists");
                return false;
            }
            TypeGroup oTypeGroup = new TypeGroup(sTypeName);
            TypeGroups.Add(oTypeGroup);
            return true;
        }
        public bool InsertWord(String sTypeName, String sNewWord)
        {
            TypeGroup oTypeGroup = FindTypeGroup(sTypeName);
            if (oTypeGroup != null)
                return oTypeGroup.InsertWord(sNewWord);
            return false;
        }
        public bool DeleteWord(String sTypeName, String sWord)
        {
            TypeGroup oTypeGroup = FindTypeGroup(sTypeName);
            if (oTypeGroup != null)
                return oTypeGroup.DeleteWord(sWord);
            return false;
        }
        public bool DeleteCategory(String sTypeName)
        {
            TypeGroup oTypeGroup = FindTypeGroup(sTypeName);
            if (oTypeGroup != null)
            {
                TypeGroups.Remove(oTypeGroup);
                return true;
            }
            return false;
        }
        public bool UpdateWord(String sTypeName, String sOldValue, String sNewValue)
        { 
            TypeGroup oTypeGroup = FindTypeGroup(sTypeName);
            if (oTypeGroup != null)
                return oTypeGroup.UpdateWord(sOldValue, sNewValue);
            return false;
        }
        public bool UpdateCategory(String sOldValue, String sNewValue)
        {
            Debug.Assert(sNewValue.Length > 0);

            TypeGroup oTypeGroup = FindTypeGroup(sOldValue);
            if (oTypeGroup != null)
            {
                oTypeGroup.Name = sNewValue;
                return true;
            }
            return false;
        }
        public List<String> GetWords(String sTypeName)
        {
            List<String> aWords = new List<String>();
            TypeGroup oTypeGroup = FindTypeGroup(sTypeName);
            if (oTypeGroup != null)
            {
                foreach (LengthGroup oLengthGroup in oTypeGroup.LengthGroups)
                    foreach (String sWord in oLengthGroup.Words)
                        aWords.Add(sWord);
            }
            return aWords;
        }
        private TypeGroup FindTypeGroup(String sTypeName)
        {
            Debug.Assert(sTypeName.Length != 0);

            for (int i = 0; i < TypeGroups.Count; i++)
                if (TypeGroups[i].Name == sTypeName)
                    return TypeGroups[i];
            return null;
        }

        public List<TypeGroup> TypeGroups { get; set; } // m_aTypeGroups
    }
}
