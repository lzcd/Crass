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
        public Parameters Parameters { get; private set; }
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
            // remove '@mixin'
            remainingWords.Dequeue();


            //while (remainingWords.Peek() != "{" &&
            //    remainingWords.Peek() != "(")
            //{
            //    definition.Names.Add(remainingWords.Dequeue());
            //}

            //Parameters parameters;
            //if (Parameters.TryParse(definition, remainingWords, out parameters))
            //{
            //    definition.Parameters = parameters;
            //}

            var remainingNameWords = new Queue<string>();
            while (remainingWords.Peek() != "{")
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
