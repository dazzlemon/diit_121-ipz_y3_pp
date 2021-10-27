using System;
using System.Collections.Generic;
using System.Linq;

namespace Refactor {
    public class Refactor {
        public string RenameMethod(string oldName, string newName, string fileContents) {
            var fileContents_ = new string(fileContents);
            var comments = Comments(fileContents_);
            var quotes = Quotes(fileContents_);

            var strings = new List<int>();

            int start = 0;
            while (start < fileContents_.Length) {
                int index = fileContents_.IndexOf(oldName, start);
                if (index == -1)
                    break;
                else {
                    bool inCode = true;
                    foreach (var comment in comments.Concat(quotes)) {
                        if (index > comment.Item1 && index < comment.Item2) {
                            inCode = false;
                            break;
                        }
                    }

                    if (inCode) {
                        if(fileContents_[index - 1] == '.' ||
                           fileContents_[index - 1] == ' '
                            && fileContents_[index+oldName.Length] == '(')
                            strings.Add(index);
                    }
                    start = index + oldName.Length;
                }
                
            }

            for (int i = 0; i < strings.Count; i++){
                fileContents_ = fileContents_.Remove(strings[i], oldName.Length);
                fileContents_ = fileContents_.Insert(strings[i], newName);
                for (int j = i; j < strings.Count; j++)
                {
                    strings[j] = strings[j] - oldName.Length + newName.Length;
                }
            }
            return fileContents_;

           // throw new NotImplementedException("Not implemented yet.");
        }
        
        public string ReplaceMagicNumber(string number, string CName, string fileContents) {
            throw new NotImplementedException("Not implemented yet.");
        }

        public static IEnumerable<String> PairwiseStr(String str) {
            return str.Zip(str.Skip(1), (a, b) => "" + a + b);
        }

        public List<(int, int)> Quotes(string fileContents) {
            var quotes = new List<(int, int)>();
            bool inQuotes = false;
            int index = 0;
            int quotesStart = 0;
            foreach (var ch in fileContents) {
                // start of quotes
                var inQuotes_ = ch == '\"';
                if (!inQuotes && inQuotes_) {
                    quotesStart = index;
                }

                // end of quotes
                bool notInQuotes = inQuotes && ch == '\"';

                inQuotes |= inQuotes_;
                if (notInQuotes) {
                    inQuotes = false;
                    quotes.Add((quotesStart, index));
                }
                index++;
            }
            if (inQuotes) {
                quotes.Add((quotesStart, index));
            }

            return quotes;
        }

        public List<(int, int)> Comments(string fileContents) {
            var comments = new List<(int, int)>();
            bool inSingleComment = false;
            bool inMultiComment  = false;
            int index = 0;
            int commentStart = 0;
            foreach (var pair in PairwiseStr(fileContents)) {
                // start of comment
                var inSingleComment_ = pair == "//";
                var inMultiComment_  = pair == "/*" && !inSingleComment;
                if (!inSingleComment && inSingleComment_ ||
                    !inMultiComment  && inMultiComment_) {
                    commentStart = index;
                } 
                
                // end of comment
                bool notInSingleComment = inSingleComment && pair[0] == '\n';
                bool notInMultiComment  = inMultiComment  && pair    == "*/";
             
                inSingleComment |= inSingleComment_;
                inMultiComment  |= inMultiComment_;
                if ( notInSingleComment || notInMultiComment) {
                    inSingleComment = false;
                    inMultiComment = false;
                    comments.Add((commentStart, index));
                }

                index++;
            }
            if (inSingleComment || inMultiComment) {
                comments.Add((commentStart, index));
            }

            return comments;
        }
    }
}
