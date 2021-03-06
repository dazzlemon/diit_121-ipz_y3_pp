using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Refactor
{
    public class Refactor
    {
        public string RenameMethod(string oldName, string newName, string fileContents)
        {
            var strings = MethodIndexes(oldName, fileContents);
            fileContents = ReplaceByIndexes(fileContents, strings, oldName, newName);
            return fileContents;
        }

        public string ReplaceMagicNumber(string number, string CName, string fileContents)
        {
            var type = NumericType(number);
            var magicNumberIndexes = MagicNumberIndexes(number, fileContents);
            var prefix = "";
            var constIndexes = Indexes(CName, fileContents, _ => true);
            if (magicNumberIndexes.Any()) {
                if (constIndexes.Any())
                {
                    CName += "_NEW";
                }
                prefix = $"const {type} {CName} = {number};\r\n";
                fileContents = ReplaceByIndexes(fileContents, magicNumberIndexes, number, CName);
            }
            return prefix + fileContents;
        }


        private string ReplaceByIndexes(string str, IEnumerable<int> indexes, string old, string new_)
        {
            var indexes_ = new List<int>(indexes);
            for (int i = 0; i < indexes_.Count; i++)
            {
                str = str.Remove(indexes_[i], old.Length)
                         .Insert(indexes_[i], new_);
                indexes_ = indexes_.Select(x => x - old.Length + new_.Length)
                                   .ToList();
            }
            return str;
        }

        private IEnumerable<int> ItemIndexes(string item, string fileContents) {
            for (
                int start = 0,
                    index = fileContents.IndexOf(item, start);
                start < fileContents.Length && index != -1; 
                start = index + item.Length,
                    index = fileContents.IndexOf(item, start)
            ) {
                yield return index;
            }
        }


        private IEnumerable<int> Indexes(string item, string fileContents, Func<int, bool> checkIndex) {
            var commentsAndQuotes = Comments(fileContents).Concat(Quotes(fileContents));
            return ItemIndexes(item, fileContents)
                .Where(i => IsInCode(i, commentsAndQuotes) && checkIndex(i));
        }

        private IEnumerable<int> MethodIndexes(string name, string fileContents) {
            return Indexes(name, fileContents, (x) => {
                return
                    fileContents[x - 1] is '.' or ' ' &&
                    fileContents[x + name.Length] == '(';
            });
        }

        private IEnumerable<int> MagicNumberIndexes(string number, string fileContents) {
            return Indexes(number, fileContents, (x) => {
                return !("" + fileContents[x - 1] + fileContents[x + number.Length])
                    .Any(Char.IsLetterOrDigit);
            });
        }

        private bool IsInCode(int index, IEnumerable<(int, int)> commentsAndQuotes) {
            return !commentsAndQuotes.Any(r => index > r.Item1 && index < r.Item2);
        }

        public string NumericType(string number) {
            bool isInteger = int.TryParse(number, out _);
            if (isInteger) {
                return "int";
            }
            bool isDouble = Double.TryParse(number, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            if (isDouble) {
                return "double";
            }
            throw new ArgumentException($"{number} is not a number");
        }

        public static IEnumerable<String> PairwiseStr(String str)
        {
            return str.Zip(str.Skip(1), (a, b) => "" + a + b);
        }

        public List<(int, int)> Quotes(string fileContents)
        {
            var quotes = new List<(int, int)>();
            bool inQuotes = false;
            int index = 0;
            int quotesStart = 0;
            foreach (var ch in fileContents)
            {
                // start of quotes
                var inQuotes_ = ch == '\"';
                if (!inQuotes && inQuotes_)
                {
                    quotesStart = index;
                }

                // end of quotes
                bool notInQuotes = inQuotes && ch == '\"';

                inQuotes |= inQuotes_;
                if (notInQuotes)
                {
                    inQuotes = false;
                    quotes.Add((quotesStart, index));
                }
                index++;
            }
            if (inQuotes)
            {
                quotes.Add((quotesStart, index));
            }

            return quotes;
        }

        public List<(int, int)> Comments(string fileContents)
        {
            var comments = new List<(int, int)>();
            bool inSingleComment = false;
            bool inMultiComment = false;
            int index = 0;
            int commentStart = 0;
            foreach (var pair in PairwiseStr(fileContents))
            {
                // start of comment
                var inSingleComment_ = pair == "//";
                var inMultiComment_ = pair == "/*" && !inSingleComment;
                if (!inSingleComment && inSingleComment_ ||
                    !inMultiComment && inMultiComment_)
                {
                    commentStart = index;
                }

                // end of comment
                bool notInSingleComment = inSingleComment && pair[0] == '\n';
                bool notInMultiComment = inMultiComment && pair == "*/";

                inSingleComment |= inSingleComment_;
                inMultiComment |= inMultiComment_;
                if (notInSingleComment || notInMultiComment)
                {
                    inSingleComment = false;
                    inMultiComment = false;
                    comments.Add((commentStart, index));
                }
                index++;
            }
            if (inSingleComment || inMultiComment)
            {
                comments.Add((commentStart, index));
            }
            return comments;
        }
    }
}
