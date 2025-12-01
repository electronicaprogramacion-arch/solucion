using System.Collections.Generic;

namespace Blazed.Controls
{
    public partial class RulesListTemplate<TTYPE, TITEM, TValue> : RulesListTemplateBase<TTYPE, TITEM, TValue> where TTYPE : new()
    {

        public ICollection<TITEM> Items { get; set; }


    }
}
