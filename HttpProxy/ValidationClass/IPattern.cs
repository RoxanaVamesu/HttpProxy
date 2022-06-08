using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public interface IPattern
    {
        IMatch Match(string text);
    }
}
