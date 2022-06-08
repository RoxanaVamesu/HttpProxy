using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class Value : IPattern
    {
        private readonly IPattern pattern;

        public Value()
        {
            var pattern = new IPattern[]
            {
                new Token(), new String(), new Separator()
            };
            var fieldContent = new Choice(new IPattern[] {new Text(BasicRules.TEXT), new Choice(pattern) });
            this.pattern =new Optional( new Choice(new IPattern[] { fieldContent, BasicRules.LWS}));
        }

        public IMatch Match(string text)
        {
            var remainingText = text;
            var validation = Enumerable.Range(0, text.Length).All(_ =>
            {
                var v = pattern.Match(remainingText);
                remainingText = v.RemainingText;
                return v.Success||remainingText.Equals("");
            });

            return new Match(validation, remainingText);
        }
    }
}
