﻿using System;
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
