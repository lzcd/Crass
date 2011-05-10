using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class Parameters : Node
    {
        internal static bool TryParse(Queue<string> remainingWords, out Parameters parameters)
        {
            if (remainingWords.Peek() != "(")
            {
                parameters = null;
                return false;
            }
            remainingWords.Dequeue();

            parameters = new Parameters();
            while (remainingWords.Peek() != ")")
            {
                Expression expression;
                if (Expression.TryParse(remainingWords, out expression))
                {
                    parameters.Children.Add(expression);
                    continue;
                }
            }
            remainingWords.Dequeue();

            return true;
        }

        public List<Node> Children { get; private set; }

        public Parameters()
        {
            Children = new List<Node>();
        }
    }
}
