using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class Colour : Node
    {
        public string Text { get; set; }

        public override void Emit(Context context, StringBuilder output)
        {
            output.Append(Text);
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
