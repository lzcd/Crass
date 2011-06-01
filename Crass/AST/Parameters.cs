using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    public class Parameters : Node
    {
        public List<Node> Children { get; private set; }

        public Parameters()
        {
            Children = new List<Node>();
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }

            foreach (var child in Children)
            {
                child.Find(criteria, matching);
            }
        }

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
                if (remainingWords.Peek() == ",")
                {
                    remainingWords.Dequeue();
                    continue;
                }

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

       
    }
}
