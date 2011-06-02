using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class DirectiveAssignment : Node
    {
        public DirectiveAssignment(Node parent)
            : base(parent)
        {
        }

        public string Name { get; set; }
        public Expression Expression { get; set; }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }

            Expression.Find(criteria, matching);
        }

        internal static bool TryParse(Node parent, Queue<string> remainingWords, out DirectiveAssignment directive)
        {
            directive = null;
            if (!remainingWords.Peek().StartsWith("@"))
            {
                return false;
            }

            directive = new DirectiveAssignment(parent);
            directive.Name = remainingWords.Dequeue().Substring(1);

            Expression expression;
            if (!Expression.TryParse(directive, remainingWords, out expression))
            {
                return false;
            }
            // remove ';'
            remainingWords.Dequeue();
            directive.Expression = expression;

            return true;
        }
    }
}
