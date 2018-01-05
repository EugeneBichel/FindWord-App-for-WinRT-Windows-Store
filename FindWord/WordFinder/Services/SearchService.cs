using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FindWord.Data;

namespace FindWord.Services
{
    public class SearchService
    {
        private const int LengthOfMaxWord = 100;
        private int _wordLength;

        public string GetSearchPattern(IEnumerable<string> letters)
        {
            var sbPattern = new StringBuilder();

            _wordLength = 0;

            sbPattern.Append("^");

            bool isMaxWordAdded = false;

            foreach (var ch in letters)
            {
                if (ch == Constants.QuestionSymbol)
                {
                    sbPattern.Append(@"\w");
                    _wordLength++;
                }
                else if (string.Equals(ch, Constants.StarSymbol, StringComparison.OrdinalIgnoreCase))
                {
                    sbPattern.Append(@"\w*");
                    if (!isMaxWordAdded)
                    {
                        _wordLength += LengthOfMaxWord;
                        isMaxWordAdded = true;
                    }
                }
                else if (string.Equals(ch, Constants.PlusSymbol, StringComparison.OrdinalIgnoreCase))
                {
                    sbPattern.Append(@"\w+");
                    _wordLength++;
                    if (!isMaxWordAdded)
                    {
                        _wordLength += LengthOfMaxWord;
                        isMaxWordAdded = true;
                    }
                }
                else
                {
                    sbPattern.Append(ch);
                    _wordLength++;
                }
            }

            sbPattern.Append(@"$");

            var pattern = sbPattern.ToString();

            return pattern;
        }

        public async Task<Dictionary<int, List<string>>> FindWordsByPatternAsync(string pattern, int topN)
        {
            Task<Task<Dictionary<int, List<string>>>> findedWordsWithLengthTask = Task.Factory.StartNew<Task<Dictionary<int, List<string>>>>(async () =>
            {
                var findedWordsWithLength = new Dictionary<int, List<string>>();
                var counter = 0;

                if (_wordLength >= LengthOfMaxWord)
                {
                    var minLength = _wordLength - LengthOfMaxWord;

                    var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                    for (var i = 0; i < Repository.WordsWithLength.Count; i++)
                    {
                        if (Repository.WordsWithLength.Keys.ElementAt(i) >= minLength)
                        {
                            var words = Repository.WordsWithLength.Values.ElementAt(i).ToList();
                            var findedWords = new List<string>();

                            for (var j = 0; j < words.Count; j++)
                            {
                                var isMatched = regex.IsMatch(words[j]);

                                if (topN == -1 && isMatched)
                                    findedWords.Add(words[j]);
                                else if (isMatched)
                                {
                                    findedWords.Add(words[j]);
                                    if (++counter >= topN)
                                        break;
                                }
                            }

                            findedWordsWithLength.Add(Repository.WordsWithLength.Keys.ElementAt(i), findedWords);
                        }

                        counter = 0;
                    }
                }
                else
                {
                    for (var i = 0; i < Repository.WordsWithLength.Count; i++)
                    {
                        if (Repository.WordsWithLength.Keys.ElementAt(i) == _wordLength)
                        {
                            var words = Repository.WordsWithLength.Values.ElementAt(i).ToList();
                            var findedWords = new List<string>();

                            for (var j = 0; j < words.Count; j++)
                            {
                                var isMatched = Regex.IsMatch(words[j], pattern, RegexOptions.IgnoreCase);

                                if (topN == -1 && isMatched)
                                    findedWords.Add(words[j]);
                                else if (isMatched)
                                {
                                    findedWords.Add(words[j]);
                                    if (++counter >= topN)
                                        break;
                                }
                            }

                            findedWordsWithLength.Add(Repository.WordsWithLength.Keys.ElementAt(i), findedWords);

                            break;
                        }
                    }
                }

                return findedWordsWithLength;

            }, TaskCreationOptions.None);

            try
            {
                return await await findedWordsWithLengthTask;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}