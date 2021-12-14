using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherGroupRefactor
{
    public class Refactor
    {
        public string RenameVariable(string text, string oldname, string newname)
        {
            try
            {
                if (text != null && oldname != null && newname != null)
                {
                    string pattern = @"\b" + oldname + @"\b";
                    bool isText = false;

                    string[] words = text.Split(' ');
                    int wordsCount = words.Length;
                    if (wordsCount > 0)
                    {
                        for (int i = 0; i < wordsCount; i++)
                        {
                            int lastBumpInd = Convert.ToString(words.GetValue(i)).IndexOf('\"');
                            if (lastBumpInd != -1)
                            {
                                isText ^= true;
                                if (Convert.ToString(words.GetValue(i)).IndexOf('\"', ++lastBumpInd) != -1)
                                {
                                    isText ^= true;
                                }
                                continue;
                            }
                            if (!isText && (Convert.ToString(words.GetValue(i)) == oldname || Convert.ToString(words.GetValue(i)) == (oldname + ';')))
                            {
                                if (Convert.ToString(words.GetValue(i)) == (oldname + ';'))
                                {
                                    words.SetValue((newname + ';'), i);
                                }
                                else
                                {
                                    words.SetValue(newname, i);
                                }
                            }
                        }
                        text = "";
                        for (int i = 0; i < wordsCount; i++)
                        {
                            text += Convert.ToString(words.GetValue(i)) + ' ';
                        }
                        text = text.Remove(text.Length - 1, 1);
                        Console.WriteLine(text);
                        //text.Trim();
                    }
                    return text;
                }
                else
                {
                    throw new ArgumentNullException("Some arguments are null / some argument is null");
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"{e.GetType().Name}: argument is null");
                throw e;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().Name}: something went wrong");
                throw e;
            }
        }

        public string ExtractMethod(string text, string method, string name)
        {
            try
            {
                if (text != null && method != null && name != null)
                {
                    string function = name + "()";
                    if (!text.Contains(function))
                    {
                        if (text.Contains(method))
                        {
                            while (text.Contains(method))
                            {
                                text = text.Replace(method, name + "();");
                            }
                            text = text.Insert(0, ConstructMethod(name, method));
                            return text;
                        }
                        else
                        {
                            return text;
                        }
                    }
                    else
                        return text;
                }
                else
                {
                    throw new ArgumentNullException("Some arguments are null / some argument is null");
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"{e.GetType().Name}: argument is null");
                throw e;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().Name}: something went wrong");
                throw e;
            }
        }
        private string ConstructMethod(string methodName, string methodText)
        {
            string allMethodText = "";

            allMethodText = "void " + methodName + "()\r\n" +
                "{" + methodText + "}\r\n";
            return allMethodText;
        }
    }
}
