using System;
using System.Collections.Generic;

namespace FindWord.Models
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
            AddWord(sWord);
            Length = sWord.Length;
        }
        public bool WordExists(String sWord)
        {
            foreach (String Word in Words)
            {
                if (Word == sWord)
                    return true;
            }
            return false;
        }
        public bool AddWord(String sWord)
        {
            Words.Add(sWord);
            return true;
        }

        public int Length { get; set; } // m_iLength
        public List<String> Words { get; set; } // m_aWords
    }

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
            foreach (LengthGroup oLengthGroup in LengthGroups)
            {
                if (oLengthGroup.WordExists(sWord))
                    return true;
            }
            return false;
        }
        public bool AddWord(String sWord)
        {
            if (WordExists(sWord))
            {
                //Form1.LogError("Word " + sWord + " already exists in " + Name);
                return false;
            }
            for (int i = 0; i < LengthGroups.Count; i++)
            {
                if (sWord.Length == LengthGroups[i].Length)
                {
                    return LengthGroups[i].AddWord(sWord);
                }
            }
            LengthGroup oLengthGroup = new LengthGroup(sWord);
            LengthGroups.Add(oLengthGroup);
            return true;
        }

        public String Name { get; set; } // m_sName
        public List<LengthGroup> LengthGroups { get; set; } // m_aLengthGroups
    }

    public class WordCategories
    {
        public WordCategories()
        {
            TypeGroups = new List<TypeGroup>();
        }
        public bool TypeGroupExists(String sName)
        {
            foreach (TypeGroup oTypeGroup in TypeGroups)
            {
                if (oTypeGroup.Name == sName)
                    return true;
            }
            return false;
        }
        public bool AddTypeGroup(String sName)
        {
            if (TypeGroupExists(sName))
            {
                return false;
            }
            TypeGroup oTypeGroup = new TypeGroup(sName);
            TypeGroups.Add(oTypeGroup);
            return true;
        }
        public bool AddWord(String sTypeName, String sWord)
        {
            if (!TypeGroupExists(sTypeName))
            {
                return false;
            }
            for (int i = 0; i < TypeGroups.Count; i++)
            {
                if (TypeGroups[i].Name == sTypeName)
                {
                    return TypeGroups[i].AddWord(sWord);
                }
            }
            return false;
        }

        public List<String> GetWords(String sTypeName)
        {
            if (!TypeGroupExists(sTypeName))
            {
                return null;
            }
            else
            {
                List<String> aWords = new List<String>();
                foreach (TypeGroup oTypeGroup in TypeGroups)
                {
                    if (oTypeGroup.Name == sTypeName)
                    {
                        foreach (LengthGroup oLengthGroup in oTypeGroup.LengthGroups)
                        {
                            foreach (String sWord in oLengthGroup.Words)
                            {
                                aWords.Add(sWord);
                            }
                        }
                        break;
                    }
                }
                return aWords;
            }
        }

        public List<String> GetWords()
        {
            var words = new List<String>();
            foreach (TypeGroup oTypeGroup in TypeGroups)
            {
                foreach (LengthGroup oLengthGroup in oTypeGroup.LengthGroups)
                {
                    foreach (String sWord in oLengthGroup.Words)
                    {
                        words.Add(sWord);
                    }
                }
                break;
            }
            return words;
        }

        public Dictionary<int, List<string>> GetWordsWithLength()
        {
            var dictWords = new Dictionary<int, List<string>>();

            foreach (TypeGroup oTypeGroup in TypeGroups)
                foreach (LengthGroup oLengthGroup in oTypeGroup.LengthGroups)
                    dictWords.Add(oLengthGroup.Length,oLengthGroup.Words);

            return dictWords;
        }

        public List<TypeGroup> TypeGroups { get; set; } // m_aTypeGroups
    }
}