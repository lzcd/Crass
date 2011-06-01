using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Block : Node
    {
        public List<Node> Children { get; private set; }

        public Block(Node parent) : base(parent)
        {
            Children = new List<Node>();
        }

        internal override void Emit(StringBuilder output)
        {
            output.AppendLine(" {");
            foreach (var child in Children)
            {
                if (child is Selector)
                {
                    continue;
                }
                child.Emit(output);
            }
            output.AppendLine("}");
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

        internal static bool TryParse(Node parent, Queue<string> remainingWords, out Block block)
        {
            if (remainingWords.Peek() != "{")
            {
                block = null;
                return false;
            }
            remainingWords.Dequeue();

            block = new Block(parent);
            while (remainingWords.Peek() != "}")
            {
                PropertyAssignment property;
                if (PropertyAssignment.TryParse(block, remainingWords, out property))
                {
                    block.Children.Add(property);
                    continue;
                }

                Selector selector;
                if (Selector.TryParse(block, remainingWords, out selector))
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
