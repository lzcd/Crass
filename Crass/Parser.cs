using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass
{
    class Parser
    {
        public static List<string> ToWords(string source)
        {
            var word = new StringBuilder();
            var words = new List<string>();

            foreach (var c in source)
            {
                switch (c)
                {
                    case ' ':
                    case '\t':
                    case '\n':
                    case '\r':
                        Add(word, words);
                        break;
                    case ':':
                    case '(':
                    case ')':
                    case ',':
                    case ';':
                        Add(word, words);
                        Add(c, word);
                        Add(word, words);
                        break;
                    default:
                        Add(c, word);
                        break;
                }
            }
            Add(word, words);
            return words;
        }

        private static void Add(char c, StringBuilder word)
        {
            word.Append(c);
        }

        private static void Add(StringBuilder word, List<string> words)
        {
            if (word.Length > 0)
            {
                words.Add(word.ToString());
                word.Clear();
            }
        }
    }
}
