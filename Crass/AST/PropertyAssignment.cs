﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class PropertyAssignment : Node
    {
        internal static bool TryParse(Queue<string> remainingWords, out PropertyAssignment property)
        {
            if (remainingWords.Skip(1).Take(1).First() != ":")
            {
                property = null;
                return false;
            }

            property = new PropertyAssignment();
            property.Name = remainingWords.Dequeue();
            remainingWords.Dequeue();

            Block block;
            if (Block.TryParse(remainingWords, out block))
            {
                property.Value = block;
                return true;
            }
            Expression expression;
            if (Expression.TryParse(remainingWords, out expression))
            {
                property.Value = expression;
                return true;
            }

            property = null;
            return false;
        }

        public string Name { get; set; }
        public Node Value { get; set; }

       
    }
}
