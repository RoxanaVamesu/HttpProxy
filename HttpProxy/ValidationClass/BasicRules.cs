using System;
using System.Text;

namespace HttpProxy
{
    public static class BasicRules
    {
        public static IPattern CHAR => new Range(Convert.ToChar(0), Convert.ToChar(127));

        public static IPattern UPALPHA => new Range(Convert.ToChar(65), Convert.ToChar(90));

        public static IPattern LOALPHA => new Range(Convert.ToChar(97), Convert.ToChar(122));

        public static IPattern ALPHA => new Choice(UPALPHA, LOALPHA);

        public static IPattern DIGIT => new Range(Convert.ToChar(48), Convert.ToChar(57));

        public static IPattern CTL =>  new Range(Convert.ToChar(0), Convert.ToChar(31));

        public static IPattern CR => new Character(Convert.ToChar(13));

        public static IPattern LF => new Character(Convert.ToChar(10));

        public static IPattern SP => new Character(Convert.ToChar(32));

        public static IPattern HT => new Character(Convert.ToChar(9));

        public static IPattern QuoteMark => new Character(Convert.ToChar(34));

        public static IPattern CRLF => new Sequence(new IPattern[] { CR, LF });

        public static IPattern LWS => new Sequence(new Optional(CRLF), new Choice(SP, HT));

        public static IPattern TEXT => new Choice( new Range(Convert.ToChar(32), Convert.ToChar(126)), new Range(Convert.ToChar(160), Convert.ToChar(255)), LWS);

        public static IPattern HEX => new Choice(new Range(Convert.ToChar(65), Convert.ToChar(70)), new Range(Convert.ToChar(97), Convert.ToChar(102)), DIGIT);

        public static IPattern Escape => new Sequence( new Character('%'), HEX, HEX);

        public static IPattern Reserved => new Choice(new Character(Convert.ToChar(36)),
            new Character(Convert.ToChar(38)),
            new Range(Convert.ToChar(43), Convert.ToChar(44)),
            new Character(Convert.ToChar(47)),
            new Range(Convert.ToChar(58), Convert.ToChar(59)),
            new Character(Convert.ToChar(61)),
            new Range(Convert.ToChar(63),Convert.ToChar(64)));

        public static IPattern Mark => new Choice(
            new Character(Convert.ToChar(33)),
            new Range(Convert.ToChar(39),Convert.ToChar(42)),
            new Range(Convert.ToChar(45), Convert.ToChar(46)),
            new Character(Convert.ToChar(95)),
            new Character(Convert.ToChar(126)));

        public static IPattern Unreserved => new Choice(ALPHANUM, Mark);

        public static IPattern ALPHANUM => new Choice(ALPHA, DIGIT);

        public static IPattern Uric => new Choice(Reserved, Unreserved, Escape);

        public static IPattern Fragment => new Many(Uric);

        public static IPattern Query => new Many(Uric);

        public static IPattern Pchar => new Choice(Unreserved, Escape, new Character(Convert.ToChar(36)), new Character(Convert.ToChar(38)), new Range(Convert.ToChar(43), Convert.ToChar(44)), new Character(Convert.ToChar(58)), new Character(Convert.ToChar(61)), new Character(Convert.ToChar(64)));

        public static IPattern Param => new Many(Pchar);

        public static IPattern Segment => new Sequence(
            new Many(Pchar),
            new Many(new Sequence(new Character(Convert.ToChar(59)), new Many(Param))));

        public static IPattern PathSegments => new Sequence(Segment, new Many(new Sequence(new Character(Convert.ToChar(47)), Segment)));

        public static IPattern AbsPath => new Sequence(new Character(Convert.ToChar(47)), PathSegments);

        public static IPattern UricNoSlash => new Choice(Unreserved, Escape, new Character(Convert.ToChar(36)), new Character(Convert.ToChar(38)), new Range(Convert.ToChar(43), Convert.ToChar(44)), new Range(Convert.ToChar(58), Convert.ToChar(59)), new Character(Convert.ToChar(61)), new Range(Convert.ToChar(63), Convert.ToChar(64)));

        public static IPattern OpaquePart => new Sequence(UricNoSlash, new Many(Uric));

        public static IPattern Path => new Optional(new Choice(AbsPath, OpaquePart));

        public static IPattern Port => new Many(DIGIT);

        public static IPattern IPv4address => new Sequence(
            new OneOrMore(DIGIT), new Character(Convert.ToChar(46)),
            new OneOrMore(DIGIT), new Character(Convert.ToChar(46)),
            new OneOrMore(DIGIT), new Character(Convert.ToChar(46)),
            new OneOrMore(DIGIT), new Character(Convert.ToChar(46)));

        public static IPattern Toplabel => new Choice(new IPattern[]
        {
            new OneOrMore(ALPHA),
            new Sequence(ALPHA, new Many(new Choice(ALPHANUM, new Character(Convert.ToChar(45)))), ALPHANUM)
        });

        public static IPattern Domainlabel => new Choice(
            new Sequence(ALPHANUM, new Many(new Choice(ALPHANUM, new Character(Convert.ToChar(45)))), ALPHANUM),
            new OneOrMore(ALPHANUM));

        public static IPattern Hostname => new Sequence(new Many(new Sequence(Domainlabel, new Character(Convert.ToChar(46)))), Toplabel, new Optional(new Character(Convert.ToChar(46))));

        public static IPattern Host => new Choice(Hostname, IPv4address);

        public static IPattern HostPort => new Sequence(Host, new Optional(new Sequence(new Character(Convert.ToChar(58)), Port)));

        public static IPattern UserInfo => new Many(new Choice(Unreserved, Escape, new Character(Convert.ToChar(36)), new Character(Convert.ToChar(38)), new Range(Convert.ToChar(43), Convert.ToChar(44)), new Character(Convert.ToChar(58)), new Character(Convert.ToChar(59)), new Character(Convert.ToChar(61))));

        public static IPattern Server => new Optional(new Sequence(
            new Optional(new Sequence(UserInfo, new Character(Convert.ToChar(64)))),
            HostPort));

        public static IPattern RegName => new OneOrMore(new Choice( Unreserved, Escape,
            new Character(Convert.ToChar(36)),
            new Character(Convert.ToChar(44)),
            new Range(Convert.ToChar(58), Convert.ToChar(59)),
            new Character(Convert.ToChar(38)),
            new Character(Convert.ToChar(64)),
            new Character(Convert.ToChar(43)),
            new Character(Convert.ToChar(61))));

        public static IPattern Authority => new Choice( Server, RegName);

        public static IPattern Scheme => new Sequence(ALPHA,
            new Many(new Choice(ALPHA, DIGIT,
                new Character(Convert.ToChar(43)),
                new Range(Convert.ToChar(45),Convert.ToChar(46)))));

        public static IPattern RelSegment => new OneOrMore(new Choice(Unreserved, Escape,
            new Character(Convert.ToChar(64)),
            new Character(Convert.ToChar(59)),
            new Character(Convert.ToChar(38)),
            new Character(Convert.ToChar(43)),
            new Character(Convert.ToChar(61)),
            new Character(Convert.ToChar(36)),
            new Character(Convert.ToChar(44))));

        public static IPattern RelPath => new Sequence(RelSegment, new Optional(AbsPath));

        public static IPattern NetPath => new Sequence(new Character('/'), new Character('/'), Authority, new Optional(AbsPath));

        public static IPattern HierPart => new Sequence(new Choice(NetPath, AbsPath), new Optional(new Sequence(new Character('?'), Query)));

        public static IPattern RelativeURI => new Sequence(new Choice(NetPath, AbsPath, RelPath), new Optional(new Sequence(new Character('?'), Query)));

        public static IPattern AbsoluteURI => new Sequence(Scheme, new Character(':'), new Choice(HierPart, OpaquePart));

        public static IPattern URIReference => new Sequence(new Optional(new Choice(AbsoluteURI, RelativeURI)), new Optional(new Sequence(new Character('#'), Fragment)));
    }
}
