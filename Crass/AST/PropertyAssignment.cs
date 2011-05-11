﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class PropertyAssignment : Node
    {
        public string Name { get; set; }
        public Node Value { get; set; }

        public override void Emit(Context context, StringBuilder output)
        {
            if (Value is Block)
            {
                var childContext = context.CreateChild();
                childContext.PropertyPrefix = Name;
                childContext.EmitBraces = false;
                Value.Emit(childContext, output);
            }
            else
            {
                if (!string.IsNullOrEmpty(context.PropertyPrefix))
                {
                    output.Append(context.PropertyPrefix);
                    output.Append("-");
                }
                output.Append(Name);
                output.Append(": ");
                Value.Emit(context, output);
                output.AppendLine(";");
            }
        }

        internal static bool TryParse(Queue<string> remainingWords, out PropertyAssignment property)
        {
            if (remainingWords.Skip(1).First() != ":")
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
                // remove ';'
                remainingWords.Dequeue();
                return true;
            }

            property = null;
            return false;
        }


    }
}
