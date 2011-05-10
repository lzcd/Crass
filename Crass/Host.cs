using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crass.AST;

namespace Crass
{
    public class Host
    {
        public string Execute(string source)
        {
            var words = Parser.ToWords(source);

            var remainingWords = new Queue<string>(words);
            var root = new Node();

            VariableAssignment variableAssignment;
            VariableAssignment.TryParse(remainingWords, out variableAssignment);

            return null;
        }

       

    }
}
