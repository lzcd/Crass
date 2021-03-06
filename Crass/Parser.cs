﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crass.Ast;

namespace Crass
{
    class Parser
    {
        public static List<Word> ToWords(string source)
        {
            var text = new StringBuilder();
            var words = new List<Word>();
            var line = 0;
            foreach (var c in source)
            {
                switch (c)
                {
                    case ' ':
                    case '\t':
                    case '\n':
                    case '\r':
                        Add(text, words, line);
                        line++;
                        break;
                    case ':':
                    case '(':
                    case ')':
                    case '{':
                    case '}':
                    case ',':
                    case ';':
                        Add(text, words, line);
                        Add(c, text);
                        Add(text, words, line);
                        break;
                    default:
                        Add(c, text);
                        break;
                }
            }
            Add(text, words, line);
            return words;
        }

        private static void Add(char c, StringBuilder word)
        {
            word.Append(c);
        }

        private static void Add(StringBuilder word, List<Word> words, int line)
        {
            if (word.Length > 0)
            {
                words.Add(new Word() { Text = word.ToString(), Line = line });
                word.Clear();
            }
        }

        internal static bool TryParse(string source, out Script script)
        {
            var words = Parser.ToWords(source);
            var remainingWords = new Queue<Word>(words);
            
            if (Script.TryParse(null, remainingWords, out script))
            {
                return true;
            }

            script = null;
            return true;
        }
    }
}
