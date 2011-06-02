using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class MixinDefinition : Node
    {
        public MixinDefinition(Node parent)
            : base(parent)
        {
        }

        public string Name { get; set; }
        public Node Value { get; set; }
        

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }

            if (Value != null)
            {
                Value.Find(criteria, matching);
            }

        }

        internal static bool TryParse(Node parent, Queue<string> remainingWords, out MixinDefinition definition)
        {
            definition = null;
            if (remainingWords.Peek() != "@mixin")
            {
                return false;
            }

            definition = new MixinDefinition(parent);
            definition.Name = remainingWords.Dequeue().Substring(1);

            Selector selector;
            if (!Selector.TryParse(definition, remainingWords, out selector))
            {
                throw new Exception("eeep");
            }
            definition.Value = selector;

            return true;
        }

        
    }
}
