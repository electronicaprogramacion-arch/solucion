using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Controls
{
    public class ObjectConvertInfo
        {
            public object ConvertObject { set; get; }
            public IList<Type> IgnoreTypes { set; get; }
            public IList<string> IgnoreProperties { set; get; }
            public int MaxDeep { set; get; } = 3;
        }
}
