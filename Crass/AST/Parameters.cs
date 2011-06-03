using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    public class Parameters : Node
    {
        public List<Node> Children { get; private set; }

        public Parameters(Node parent)
            : base(parent)
        {
            Children = new List<Node>();
        }

        public override Node Clone(Node newParent)
        {
            var newParameters = new Parameters(newParent);
            foreach (var child in Children)
            {
                newParameters.Children.Add(child.Clone(newParameters));
            }
            return newParameters;
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

        internal static bool TryParse(Node parent, Queue<string> remainingWords, out Parameters parameters)
        {
            if (remainingWords.Count == 0 ||
                remainingWords.Peek() != "(")
            {
                parameters = null;
                return false;
            }
            remainingWords.Dequeue();

            parameters = new Parameters(parent);
            while (remainingWords.Peek() != ")")
            {
                if (remainingWords.Peek() == ",")
                {
                    remainingWords.Dequeue();
                    continue;
                }

                Expression expression;
                if (Expression.TryParse(parent, remainingWords, out expression))
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
