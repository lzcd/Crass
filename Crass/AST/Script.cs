﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class Script : Node
    {
        public List<Node> Children { get; set; }

        public Script()
        {
            Children = new List<Node>();
        }

        public override void Emit(Context context, StringBuilder output)
        {
            foreach (var child in Children)
            {
                child.Emit(context, output);
            }
        }

        internal static bool TryParse(Queue<string> remainingWords, out Script script)
        {
            script = new Script();

            do
            {
                VariableAssignment variableAssignment;
                if (VariableAssignment.TryParse(remainingWords, out variableAssignment))
                {
                    script.Children.Add(variableAssignment);
                    continue;
                }

                Selector selector;
                if (Selector.TryParse(remainingWords, out selector))
                {
                    script.Children.Add(selector);
                    continue;
                }
            }
            while (remainingWords.Count > 0);

            return true;
        }



      
    }
}
