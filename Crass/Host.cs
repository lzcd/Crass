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

            var script = new Script();

            if (!Script.TryParse(remainingWords, out script))
            {
                return null;
            }
           

            return null;
        }

       

    }
}
