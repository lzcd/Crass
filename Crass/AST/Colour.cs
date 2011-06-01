using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    public class Colour : Node
    {
        public string Text { get; set; }


        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }
        }

        internal static bool TryParse(Queue<string> remainingWords, out Colour colour)
        {
            if (!remainingWords.Peek().StartsWith("#"))
            {
                colour = null;
                return false;
            }
            var text = remainingWords.Dequeue();
            colour = new Colour() { Text = text };
            return true;
        }

       
    }
}
