﻿using System;
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
            var fileContents_ = new string(fileContents);
            for (int i = 0; i < strings.Count; i++)
            {
                fileContents_ = fileContents_
                    .Remove(strings[i], oldName.Length)
                    .Insert(strings[i], newName);
                strings = strings
                    .Select(x => x - oldName.Length + newName.Length)
                    .ToList();
            }
            return fileContents_;
        }

        private List<int> MethodIndexes(string name, string fileContents) {
            var commentsAndQuotes = Comments(fileContents).Concat(Quotes(fileContents));
            var strings = new List<int>();
            int start = 0;
            while (start < fileContents.Length)
            {
                int index = fileContents.IndexOf(name, start);
                if (index == -1)
                    break;
                else
                {
                    bool inCode = true;
                    foreach (var comment in commentsAndQuotes)
                    {
                        if (index > comment.Item1 && index < comment.Item2)
                        {
                            inCode = false;
                            break;
                        }
                    }

                    if (inCode)
                    {
                        if (fileContents[index - 1] == '.' ||
                            fileContents[index - 1] == ' ' &&
                            fileContents[index + name.Length] == '('
                        )
                            strings.Add(index);
                    }
                    start = index + name.Length;
                }
            }
            return strings;
        }

        private List<int> MagicNumberIndexes(string number, string fileContents) {
            var commentsAndQuotes = Comments(fileContents).Concat(Quotes(fileContents));
            var strings = new List<int>();
            int start = 0;
            while (start < fileContents.Length)
            {
                int index = fileContents.IndexOf(number, start);
                if (index == -1)
                    break;
                else
                {
                    bool inCode = true;
                    foreach (var comment in commentsAndQuotes)
                    {
                        if (index > comment.Item1 && index < comment.Item2)
                        {
                            inCode = false;
                            break;
                        }
                    }

                    if (inCode)
                    {
                        if (!Char.IsLetterOrDigit(fileContents[index - 1]) &&
                            !Char.IsLetterOrDigit(fileContents[index + number.Length])
                        )
                            strings.Add(index);
                    }
                    start = index + number.Length;
                }
            }
            return strings;
        }

        public string ReplaceMagicNumber(string number, string CName, string fileContents)
        {
            var strings = MagicNumberIndexes(number, fileContents);
            var fileContents_ = new string(fileContents);
            for (int i = 0; i < strings.Count; i++)
            {
                fileContents_ = fileContents_.Remove(strings[i], number.Length)
                                             .Insert(strings[i], CName);
                strings = strings
                    .Select(x => x - number.Length + CName.Length)
                    .ToList();
            }
            
            var type = NumericType(number);
            if (!strings.Any())
            {
                return fileContents_;
            }
            if (type is null)
            {
                throw new ArgumentException($"{number} is not a number");
            }
            return $"const {type} {CName} = {number};\r\n" + fileContents_;
        }

        public string NumericType(string number) {
            bool isDouble = Double.TryParse(number, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            bool isInteger = int.TryParse(number, out _);
            return isDouble ? "double"
                            : isInteger ? "integer"
                                        : null;
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
