using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    abstract class Node
    {
        public abstract void Emit(Context context, StringBuilder output);
        
    }
}
