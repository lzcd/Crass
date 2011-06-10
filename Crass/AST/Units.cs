using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Units : Node
    {
        public Units(Node parent)
            : base(parent)
        {
        }


        public decimal Amount { get; set; }
        public string Unit { get; set; }

        internal override void Emit(StringBuilder output)
        {
            output.Append(Amount);
            output.Append(Unit);
        }

        public override Node Clone(Node newParent)
        {
            return new Units(newParent) { Amount = Amount, Unit = Unit };
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }
        }

        static char[] numericCharacters = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', ',' };

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out Units units)
        {
            int firstDigit;
            if (!int.TryParse(remainingWords.Peek().Text.Substring(0,1), out firstDigit))
            {
                units = null;
                return false;
            }
            var text = remainingWords.Dequeue().Text;
            var index = 0;
            while (index < text.Length &&
                text.Substring(index, 1).IndexOfAny(numericCharacters) >= 0)
            {
                index++;
            }
            var amount = Decimal.Parse(text.Substring(0, index));
            var unit = text.Substring(index);
            units = new Units(parent) { Amount = amount, Unit = unit };
            return true;
        }

       
    }
}
