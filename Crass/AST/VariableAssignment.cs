using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class VariableAssignment : Node, IVariableApplicable
    {
        public VariableAssignment(Node parent)
            : base(parent)
        {
        }

        public string Name { get; set; }
        public Expression Expression { get; set; }
        public int SourceLine;


        public void Apply(Dictionary<string, Node> valueByName)
        {
            valueByName[Name] = Expression;
        }

        public override Node Clone(Node newParent)
        {
            var newAssignment = new VariableAssignment(newParent) { Name = Name, SourceLine = SourceLine };
            newAssignment.Expression = (Expression)Expression.Clone(newAssignment.Expression);
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
            var word = remainingWords.Dequeue();
            assignment = new VariableAssignment(parent) { Name = word.Text, SourceLine = word.Line };
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
