using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class VariableAssignment : Node
    {
        public VariableAssignment(Node parent)
            : base(parent)
        {
        }

        public string Name { get; set; }
        public Expression Expression { get; set; }

        public override Node Clone(Node newParent)
        {
            var newAssignment = new VariableAssignment(newParent);
            newAssignment.Name = Name;
            newAssignment.Expression = (Expression)Expression.Clone(newAssignment);
            return newAssignment;
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }

            Expression.Find(criteria, matching);
        }

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out VariableAssignment assignment)
        {
            assignment = null;
            if (!remainingWords.Peek().Text.StartsWith("$"))
            {
                return false;
            }
            assignment = new VariableAssignment(parent);
            assignment.Name = remainingWords.Dequeue().Text;
            // remove ':'
            remainingWords.Dequeue();
            Expression expression;
            if (!Expression.TryParse(assignment, remainingWords, out expression))
            {
                return false;
            }
            // remove ';'
            remainingWords.Dequeue();
            assignment.Expression = expression;
            return true;
        }

    }
}
