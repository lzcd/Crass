using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Variable : Node
    {
        public Variable(Node parent)
            : base(parent)
        {
        }

        public string Name { get; set; }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }
        }

        internal static bool TryParse(Node parent, Queue<string> remainingWords, out Variable variable)
        {
            if (!remainingWords.Peek().StartsWith("$"))
            {
                variable = null;
                return false;
            }
            var name = remainingWords.Dequeue();
            variable = new Variable(parent) { Name = name };
            return true;
        }

       
    }
}
