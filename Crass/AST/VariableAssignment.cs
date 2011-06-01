using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class VariableAssignment : Node
    {
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

        internal static bool TryParse(Queue<string> remainingWords, out VariableAssignment assignment)
        {
            assignment = null;
            if (!remainingWords.Peek().StartsWith("$"))
            {
                return false;
            }

            var name = remainingWords.Dequeue();
            // remove ':'
            remainingWords.Dequeue();
            Expression expression;
            if (!Expression.TryParse(remainingWords, out expression))
            {
                return false;
            }
            // remove ';'
            remainingWords.Dequeue();
            assignment = new VariableAssignment() { Name = name, Expression = expression };
            return true;
        }

    }
}
