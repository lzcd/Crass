using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Block : Node
    {
        public List<Node> Children { get; private set; }

        public Block()
        {
            Children = new List<Node>();
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }

            foreach (var child in Children)
            {
                child.Find(criteria, matching);
            }
        }

        internal static bool TryParse(Queue<string> remainingWords, out Block block)
        {
            if (remainingWords.Peek() != "{")
            {
                block = null;
                return false;
            }
            remainingWords.Dequeue();

            block = new Block();
            while (remainingWords.Peek() != "}")
            {
                PropertyAssignment property;
                if (PropertyAssignment.TryParse(remainingWords, out property))
                {
                    block.Children.Add(property);
                    continue;
                }

                Selector selector;
                if (Selector.TryParse(remainingWords, out selector))
                {
                    block.Children.Add(selector);
                    continue;
                }
            }
            remainingWords.Dequeue();

            return true;
        }


    }
}
