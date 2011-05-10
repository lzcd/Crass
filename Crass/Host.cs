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
            Script script;
            if (!Parser.TryParse(source, out script))
            {
                return null;
            }

            var context = new Context();
            var output = new StringBuilder();
            
            script.Emit(context, output);

            return output.ToString();
        }

       

    }
}
