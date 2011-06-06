﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    public class Colour : Node
    {
        public Colour(Node parent)
            : base(parent)
        {
        }

        public string Text { get; set; }

        
        internal override void Emit(StringBuilder output)
        {
            output.Append(Text);
        }

        public override Node Clone(Node newParent)
        {
            return new Colour(newParent) { Text = Text };
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }
        }

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out Colour colour)
        {
            if (!remainingWords.Peek().Text.StartsWith("#"))
            {
                colour = null;
                return false;
            }
            var word = remainingWords.Dequeue();
            colour = new Colour(parent) { Text = word.Text };
            return true;
        }

       
    }
}
