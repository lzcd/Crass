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

        public Expression Name { get; private set; }
        public Node Value { get; set; }

        public override Node Clone(Node newParent)
        {
            var newDefinition = new MixinDefinition(newParent);
            newDefinition.Name = (Expression)Name.Clone(newDefinition);
            newDefinition.Value = Value.Clone(newDefinition);
            return newDefinition;
        }

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

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out MixinDefinition definition)
        {
            definition = null;
            if (remainingWords.Peek().Text != "@mixin")
            {
                return false;
            }

            definition = new MixinDefinition(parent);
            // remove '@mixin'
            remainingWords.Dequeue();

            var remainingNameWords = new Queue<Word>();
            while (remainingWords.Peek().Text != "{")
            {
                remainingNameWords.Enqueue(remainingWords.Dequeue());
            }

            Expression name;
            if (!Expression.TryParse(definition, remainingNameWords, out name))
            {
                throw new Exception("errp?");
            }
            definition.Name = name;

            Block block;
            if (!Block.TryParse(definition, remainingWords, out block))
            {
                throw new Exception("erh?");
            }
            definition.Value = block;
            return true;
        }


    }
}
