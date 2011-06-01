using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crass.Ast;

namespace Crass
{
    public class Host
    {
        public string Execute(string source, Context.TryCallMethodHandler methodCallHandler)
        {
            Script script;
            if (!Parser.TryParse(source, out script))
            {
                return null;
            }

            var context = new Context() { TryCallMethod = methodCallHandler };
            var output = new StringBuilder();
            
           
            return null;
        }

       

    }
}
