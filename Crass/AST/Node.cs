using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    public abstract class Node
    {
        public abstract void Emit(Context context, StringBuilder output);
        
    }
}
