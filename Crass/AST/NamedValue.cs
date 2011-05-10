using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class NamedValue : Node
    {
        public string Text { get; set; }

        public override void Emit(Context context, StringBuilder output)
        {
            output.Append(Text);
        }
    }
}
