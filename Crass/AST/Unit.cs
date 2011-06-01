﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Unit : Node
    {
        public string Text { get; set; }

        internal static bool TryParse(Queue<string> remainingWords, out Unit unit)
        {
            int firstDigit;
            if (!int.TryParse(remainingWords.Peek().Substring(0,1), out firstDigit))
            {
                unit = null;
                return false;
            }
            var text = remainingWords.Dequeue();
            unit = new Unit() { Text = text };
            return true;
        }

       
    }
}
