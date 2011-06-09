﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Add : Node, IOperator
    {
        public Add(Node parent)
            : base(parent)
        {
        }

        public void Operate()
        {
            var expressionParent = (Expression)Parent;
            var operatorIndex = expressionParent.Children.IndexOf(this);
            var a = expressionParent.Children[operatorIndex - 1];
            var b = expressionParent.Children[operatorIndex + 1];

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
            return new Add(newParent);
        }

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out Add add)
        {
            if (remainingWords.Peek().Text != "+")
            {
                add = null;
                return false;
            }
            var word = remainingWords.Dequeue();
            add = new Add(parent);
            return true;
        }

       
    }
}
