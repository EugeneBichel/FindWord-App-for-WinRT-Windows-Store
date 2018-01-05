using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace JsonEditor
{
    public class TypeGroup
    {
        public TypeGroup()
        {
            LengthGroups = new List<LengthGroup>();
            Name = "";
        }
        public TypeGroup(String sTypeName)
        {
            LengthGroups = new List<LengthGroup>();
            Name = sTypeName;
        }
        public bool WordExists(String sWord)
        {
            LengthGroup oLengthGroup = FindLengthGroup(sWord.Length);
            if (oLengthGroup != null)
                return oLengthGroup.WordExists(sWord);
            return false;
        }
        public bool InsertWord(String sNewWord)
        {
            Debug.Assert(sNewWord.Length > 0);

            LengthGroup oLengthGroup = FindLengthGroup(sNewWord.Length);
            if (oLengthGroup != null)
                return oLengthGroup.InsertWord(sNewWord);

            LengthGroup oNewLengthGroup = new LengthGroup(sNewWord);
            LengthGroups.Add(oNewLengthGroup);
            return true;
        }
        public bool DeleteWord(String sWord)
        {
            LengthGroup oLengthGroup = FindLengthGroup(sWord.Length);
            if (oLengthGroup != null)
                return oLengthGroup.DeleteWord(sWord);
            return false;
        }
        public bool UpdateWord(String sOldValue, String sNewValue)
        {
            if (sOldValue.Length == sNewValue.Length)
            {
                LengthGroup oLengthGroup = FindLengthGroup(sOldValue.Length);
                if (oLengthGroup != null)
                    return oLengthGroup.UpdateWord(sOldValue, sNewValue);
                return false;
            }
            else
            {
                if (InsertWord(sNewValue))
                    return DeleteWord(sOldValue);
                return false;
            }
        }
        private LengthGroup FindLengthGroup(Int32 iLength)
        {
            Debug.Assert(iLength > 0);

            for (int i = 0; i < LengthGroups.Count; i++)
                if (LengthGroups[i].Length == iLength)
                    return LengthGroups[i];
            return null;
        }

        public String Name { get; set; } // m_sName
        public List<LengthGroup> LengthGroups { get; set; } // m_aLengthGroups
    }
}
