using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class MethodCall : Node
    {
        public string Name { get; set; }

        public Parameters Parameters { get; set; }

      
        internal static bool TryParse(Queue<string> remainingWords, out MethodCall methodCall)
        {
            if (remainingWords.Skip(1).First() != "(")
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
