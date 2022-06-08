using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class Match: IMatch
    {
        public Match(bool success, string remainingText)
        {
            Success = success;
            RemainingText = remainingText;
        }

        public bool Success { get ; set; }
        public string RemainingText { get; set; }
    }
}
