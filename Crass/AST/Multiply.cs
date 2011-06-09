using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Multiply : Node, IOperator
    {
        public Multiply(Node parent)
            : base(parent)
        {
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }
        }

        public override Node Clone(Node newParent)
        {
            return new Multiply(newParent);
        }

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out Multiply multiply)
        {
            if (remainingWords.Peek().Text != "*")
            {
                multiply = null;
                return false;
            }
            var word = remainingWords.Dequeue();
            multiply = new Multiply(parent);
            return true;
        }
    }
}
