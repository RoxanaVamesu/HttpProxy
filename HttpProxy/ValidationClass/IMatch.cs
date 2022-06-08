using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public interface IMatch
    {
        bool Success { get; set; }
        string RemainingText { get; set; }
    }
}
