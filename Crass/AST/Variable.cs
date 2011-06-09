using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Variable : Node, IVariableApplicable
    {
        public Variable(Node parent)
            : base(parent)
        {
        }

        public string Name { get; set; }
        public int SourceLine;


        public void Apply(Dictionary<string, Node> valueByName)
        {
            Node value;
            if (!valueByName.TryGetValue(Name, out value))
            {
                throw new Exception("ooooze nooooze");
            }

            var parentExpression = (Expression)Parent;
            var insertIndex = parentExpression.Children.IndexOf(this);
            var valueExpression = (Expression)value;

            parentExpression.Children.RemoveAt(insertIndex);
            foreach (var node in valueExpression.Children)
            {
                parentExpression.Children.Insert(insertIndex, node.Clone(Parent));
                insertIndex++;
            }

        }


        public override Node Clone(Node newParent)
        {
            return new Variable(newParent) { Name = Name, SourceLine = SourceLine };
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }
        }

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out Variable variable)
        {
            if (!remainingWords.Peek().Text.StartsWith("$"))
            {
                variable = null;
                return false;
            }
            var word = remainingWords.Dequeue();
            variable = new Variable(parent) { Name = word.Text, SourceLine = word.Line };
            return true;
        }

    }
}
