using System;
using System.Collections.Generic;
using System.Linq;

namespace Refactor {
    public class Refactor {
        public string RenameMethod(string oldName, string newName, string fileContents) {
            // find comments (start, length)
            // find strings  (start, length)
            throw new NotImplementedException("Not implemented yet.");
        }
        
        public string ReplaceMagicNumber(string number, string CName, string fileContents) {
            throw new NotImplementedException("Not implemented yet.");
        }

        public static IEnumerable<String> PairwiseStr(String str) {
            return str.Zip(str.Skip(1), (a, b) => "" + a + b);
        }

        public List<(int, int)> Comments(string fileContents) {
            List<(int, int)> comments = new List<(int, int)>();
            bool inSingleComment = false;
            bool inMultiComment  = false;
            int index = 0;
            int commentStart = 0;
            foreach (var pair in PairwiseStr(fileContents)) {
                // start of comment
                inSingleComment = pair == "//";
                inMultiComment  = pair == "/*" && !inSingleComment;
                if (inSingleComment || inMultiComment) {
                    commentStart = index;
                } 
                
                // end of comment
                bool notInSingleComment = inSingleComment && pair[1] == '\n';
                bool notInMultiComment  = inMultiComment  && pair    == "*/";
                if (notInSingleComment || notInMultiComment) {
                    comments.Append((commentStart, index));
                }

                inSingleComment = !notInSingleComment;
                inMultiComment  = !notInMultiComment;

                index += 2;
            }
            return comments;
        }
    }
}
