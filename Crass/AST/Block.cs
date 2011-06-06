using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Block : Node
    {
        public List<Node> Children { get; private set; }

        public Block(Node parent)
            : base(parent)
        {
            Children = new List<Node>();
        }

        public override Node Clone(Node newParent)
        {
            var newBlock = new Block(newParent);
            foreach (var child in Children)
            {
                newBlock.Children.Add(child.Clone(newBlock));
            }

            return newBlock;
        }

        internal override void Emit(StringBuilder output)
        {
            if (!(Parent is PropertyAssignment))
            {
                output.AppendLine(" {");
            }


            foreach (var child in Children)
            {
                if (child is Selector)
                {
                    continue;
                }
                child.Emit(output);
            }

            if (!(Parent is PropertyAssignment))
            {
                output.AppendLine("}");
            }
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

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out Block block)
        {
            if (remainingWords.Peek().Text != "{")
            {
                block = null;
                return false;
            }
            remainingWords.Dequeue();

            block = new Block(parent);
            while (remainingWords.Peek().Text != "}")
            {
                IncludeDirective includeDirective;
                if (IncludeDirective.TryParse(block, remainingWords, out includeDirective))
                {
                    block.Children.Add(includeDirective);
                    continue;
                }

                ExtendDirective excludeDirective;
                if (ExtendDirective.TryParse(block, remainingWords, out excludeDirective))
                {
                    block.Children.Add(excludeDirective);
                    continue;
                }

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
