using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Variable : Node
    {
        public string Name { get; set; }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }
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
