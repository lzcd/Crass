using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class Variable : Node
    {
        public string Name { get; set; }

        public override void Emit(Context context, StringBuilder output)
        {
            throw new NotImplementedException();
        }

        internal static bool TryParse(Queue<string> remainingWords, out Variable variable)
        {
            if (!remainingWords.Peek().StartsWith("$"))
            {
                variable = null;
                return false;
            }
            var name = remainingWords.Dequeue();
            variable = new Variable() { Name = name };
            return true;
        }

       
    }
}
