using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Selector : Node
    {
        public List<string> Names { get; private set; }
        public Block Block { get; set; }

        public Selector(Node parent)
            : base(parent)
        {
            Names = new List<string>();
        }

        public override Node Clone(Node newParent)
        {
            var newSelector = new Selector(newParent);
            newSelector.Names = new List<string>(Names);
            newSelector.Block = (Block)Block.Clone(newSelector);
            return newSelector;
        }

        internal override void Emit(StringBuilder output)
        {
            var searchNode = this as Node;
            while (searchNode != null)
            {
                if (searchNode is MixinDefinition)
                {
                    return;
                }
                searchNode = searchNode.Parent;
            }

            var selectorNames = new List<string>();
            var current = this as Node;
            while (current != null)
            {
                var currentSelector = current as Selector;
                if (currentSelector != null)
                {
                    var names = new List<string>(selectorNames);
                    for (var index = names.Count - 1; index > 0; index--)
                    {
                        if (names[index] == ",")
                        {
                            selectorNames.InsertRange(index + 1, currentSelector.Names);
                        }
                    }

                    selectorNames.InsertRange(0, currentSelector.Names);
                }
                current = current.Parent;
            }

            bool isFirst = true;
            foreach (var name in selectorNames)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else if (name != ",")
                {
                    output.Append(" ");
                }

                output.Append(name);

                if (name == ",")
                {
                    output.AppendLine();
                    isFirst = true;
                }
            }
            Block.Emit(output);

            output.AppendLine();
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }
            Block.Find(criteria, matching);
        }

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out Selector selector)
        {
            var openingBraceFound = false;
            foreach (var word in remainingWords)
            {
                if (word.Text == "{")
                {
                    openingBraceFound = true;
                    break;
                }
            }

            if (!openingBraceFound)
            {
                selector = null;
                return false;
            }

            selector = new Selector(parent);
            while (remainingWords.Peek().Text != "{")
            {
                selector.Names.Add(remainingWords.Dequeue().Text);
            }
            Block block;
            if (!Block.TryParse(selector, remainingWords, out block))
            {
                selector = null;
                return false;
            }
            selector.Block = block;
            return true;
        }


    }
}
