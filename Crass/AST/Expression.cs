using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class Expression : Node
    {
        internal static bool TryParse(Queue<string> remainingWords, out Expression expression)
        {
            expression = new Expression();
            while (remainingWords.Peek() != ";")
            {
                var word = remainingWords.Dequeue();
                expression.Children.Add(new Node());
            }
            if (remainingWords.Peek() == ";")
            {
                remainingWords.Dequeue();
            }
            return true;
        }

        public List<Node> Children { get; private set; }

        public Expression()
        {
            Children = new List<Node>();
        }
    }
}
