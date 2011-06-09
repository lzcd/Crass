﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Subtract : Node, IOperator
    {
        public Subtract(Node parent)
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
            return new Subtract(newParent);
        }

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out Subtract subtract)
        {
            if (remainingWords.Peek().Text != "-")
            {
                subtract = null;
                return false;
            }
            var word = remainingWords.Dequeue();
            subtract = new Subtract(parent);
            return true;
        }
    }
}
