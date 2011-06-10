using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Divide : Node, IOperator
    {
        public Divide(Node parent)
            : base(parent)
        {
        }

        public void Operate()
        {
            var expressionParent = (Expression)Parent;
            var operatorIndex = expressionParent.Children.IndexOf(this);
            var a = expressionParent.Children[operatorIndex - 1];
            var b = expressionParent.Children[operatorIndex + 1];

            expressionParent.Children.RemoveRange(operatorIndex - 1, 3);

            if (a is Units && b is Units)
            {
                var aUnits = (Units)a;
                var bUnits = (Units)b;
                var result = new Units(expressionParent) { Amount = (aUnits.Amount / bUnits.Amount), Unit = aUnits.Unit };
                expressionParent.Children.Insert(operatorIndex - 1, result);
            }

        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }
        }

        public override Node Clone(Node newParent)
        {
            return new Divide(newParent);
        }

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out Divide divide)
        {
            if (remainingWords.Peek().Text != "/")
            {
                divide = null;
                return false;
            }
            var word = remainingWords.Dequeue();
            divide = new Divide(parent);
            return true;
        }
    }
}
