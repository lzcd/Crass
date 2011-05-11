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

        public Selector()
        {
            Names = new List<string>();
        }

        public override void Emit(Context context, StringBuilder output)
        {
            foreach (var name in Names)
            {
                output.Append(name);
                output.Append(" ");
            }
            Block.Emit(context, output);
        }

        internal static bool TryParse(Queue<string> remainingWords, out Selector selector)
        {
            selector = new Selector();
            while (remainingWords.Peek() != "{")
            {
                selector.Names.Add(remainingWords.Dequeue()); 
            }
            Block block;
            if (!Block.TryParse(remainingWords, out block))
            {
                selector = null;
                return false;
            }
            selector.Block = block;
            return true;
        }

       
    }
}
