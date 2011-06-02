using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class PropertyAssignment : Node
    {
        public PropertyAssignment(Node parent)
            : base(parent)
        {
        }

        public string Name { get; set; }
        public Node Value { get; set; }

        internal override void Emit(StringBuilder output)
        {
            if (Value is Block)
            {
                Value.Emit(output);
            }
            else
            {
                var propertyAssignmentNames = new List<string>();
                var current = Parent as Node;
                while (current != null)
                {
                    var currentPropertyAssignment = current as PropertyAssignment;
                    if (currentPropertyAssignment != null)
                    {
                        propertyAssignmentNames.Insert(0, currentPropertyAssignment.Name);
                    }
                    current = current.Parent;
                }

                output.Append("  ");

                foreach (var prefix in propertyAssignmentNames)
                {
                    output.Append(prefix);
                    output.Append("-");
                }

                output.Append(Name);
                output.Append(": ");
                Value.Emit(output);
                output.AppendLine(";");
            }
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }

            Value.Find(criteria, matching);
        }

        internal static bool TryParse(Node parent, Queue<string> remainingWords, out PropertyAssignment property)
        {
            if (remainingWords.Skip(1).First() != ":")
            {
                property = null;
                return false;
            }

            property = new PropertyAssignment(parent);
            property.Name = remainingWords.Dequeue();
            // remove ':'
            remainingWords.Dequeue();

            Block block;
            if (Block.TryParse(property, remainingWords, out block))
            {
                property.Value = block;
                return true;
            }
            Expression expression;
            if (Expression.TryParse(property, remainingWords, out expression))
            {
                property.Value = expression;
                // remove ';'
                if (remainingWords.Peek() == ";")
                {
                    remainingWords.Dequeue();
                }
                return true;
            }

            property = null;
            return false;
        }


    }
}
