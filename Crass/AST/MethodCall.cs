using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class MethodCall : Node
    {
        public string Name { get; set; }

        public Parameters Parameters { get; set; }

        public override void Emit(Context context, StringBuilder output)
        {
            output.Append(Name);
            Parameters.Emit(context, output);
        }

        internal static bool TryParse(Queue<string> remainingWords, out MethodCall methodCall)
        {
            if (remainingWords.Skip(1).Take(1).First() != "(")
            {
                methodCall = null;
                return false;
            }

            var name = remainingWords.Dequeue();
            Parameters parameters;
            if (!Parameters.TryParse(remainingWords, out parameters))
            {
                methodCall = null;
                return false;
            }

            methodCall = new MethodCall() { Name = name, Parameters = parameters };
            
            return true;
        }

       
    }
}
