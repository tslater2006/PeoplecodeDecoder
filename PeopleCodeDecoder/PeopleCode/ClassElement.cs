using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class ClassElement : Element
    {
        String ClassName;
        String Implements = "";
        List<Element> Public = new List<Element>();
        List<Element> Private = new List<Element>();
        List<Element> Protected = new List<Element>();
        List<Element> Body = new List<Element>();

        public override void Write(StringBuilder sb)
        {
            DoPadding(sb);
            sb.Append("class ").Append(ClassName);
            if (Implements.Length > 0)
            {
                sb.Append(" ").Append("implements ").Append(Implements).Append("\r\n");
            } else
            {
                sb.Append("\r\n");
            }

            foreach(Element e in Public)
            {
                e.Write(sb);
            }

            if (Protected.Count > 0)
            {
                sb.Append("protected\r\n");
                foreach(Element e in Protected)
                {
                    e.Write(sb);
                }
            }

            if (Private.Count > 0)
            {
                sb.Append("private\r\n");
                foreach (Element e in Private)
                {
                    e.Write(sb);
                }
            }
            DoPadding(sb);
            sb.Append("end-class;");

            foreach(Element e in Body)
            {
                e.Write(sb);
            }

            sb.Append("\r\n");
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            Element stringElement = new PureStringElement();
            /* eat the class byte */
            ms.ReadByte();
            state.InClassDefn = true;
            stringElement.Parse(ms, state);
            ClassName = stringElement.Value;

            byte nextByte = Peek(ms);

            if (nextByte == 114) /* implements */
            {
                /* eat implements byte */
                ms.ReadByte();
                do
                {
                    if (Peek(ms) == 87)
                    {
                        Implements += Element.GetNextElement(ms, state, 0).Value;
                    }
                    Implements += Element.GetNextElement(ms, state, 0).Value;
                } while (Peek(ms) == 87);
            }

            while (nextByte != 91 && nextByte != 97 && nextByte != 115)
            {
                Element nextElement = Element.GetNextElement(ms, state,IndentLevel);
                Public.Add(nextElement);
                nextByte = Peek(ms);
            }
            if (nextByte == 115) /* has protected section */
            {
                /* eat protected byte */
                ms.ReadByte();
                while (nextByte != 91 && nextByte != 97)
                {
                    Element nextElement = Element.GetNextElement(ms, state, IndentLevel);
                    Protected.Add(nextElement);
                    nextByte = Peek(ms);
                }
            }
            if (nextByte == 97) /* has private section */
            {
                /* eat private byte */
                ms.ReadByte();
                while (nextByte != 91)
                {
                    Element nextElement = Element.GetNextElement(ms, state, IndentLevel);
                    Private.Add(nextElement);
                    nextByte = Peek(ms);
                }
            }
            /* eat the end-class */
            ms.ReadByte();

            state.InClassDefn = false;

            /* eat the semicolon */
            ms.ReadByte();

            /* class defn has been parsed, now for the body */

            while (Peek(ms) != 7)
            {
                Element nextElement = Element.GetNextElement(ms, state, IndentLevel - 1);
                Body.Add(nextElement);
            }

            nextByte = Peek(ms);


            /* handle parse options */
            if (state.Options.AlphabetizeMethodDeclarations)
            {
                List<MethodDeclarationElement> methods = new List<MethodDeclarationElement>();
                List<int> indexes = new List<int>();
                /* process Public */
                for (var x = 0; x < Public.Count; x++)
                {
                    if (Public[x] is MethodDeclarationElement)
                    {
                        methods.Add((MethodDeclarationElement)Public[x]);
                        indexes.Add(x);
                    }
                }
                methods = methods.OrderBy(p => p.MethodName).ToList<MethodDeclarationElement>();
                for (var x = 0; x < indexes.Count; x++)
                {
                    Public[indexes[x]] = methods[x];
                }
                methods.Clear();
                indexes.Clear();

                /* process Protected */
                for (var x = 0; x < Protected.Count; x++)
                {
                    if (Protected[x] is MethodDeclarationElement)
                    {
                        methods.Add((MethodDeclarationElement)Protected[x]);
                        indexes.Add(x);
                    }
                }
                methods = methods.OrderBy(p => p.MethodName).ToList<MethodDeclarationElement>();
                for (var x = 0; x < indexes.Count; x++)
                {
                    Protected[indexes[x]] = methods[x];
                }
                methods.Clear();
                indexes.Clear();

                /* process Private */
                for (var x = 0; x < Private.Count; x++)
                {
                    if (Private[x] is MethodDeclarationElement)
                    {
                        methods.Add((MethodDeclarationElement)Private[x]);
                        indexes.Add(x);
                    }
                }
                methods = methods.OrderBy(p => p.MethodName).ToList<MethodDeclarationElement>();
                for (var x = 0; x < indexes.Count; x++)
                {
                    Private[indexes[x]] = methods[x];
                }
                methods.Clear();
                indexes.Clear();
            }

            if (state.Options.MatchMethodDeclarationOrder)
            {
                /* get ordered list of all declares */
                List<MethodDeclarationElement> declares = new List<MethodDeclarationElement>();
                foreach(var e in Public.Concat(Protected).Concat(Private).ToList())
                {
                    if (e is MethodDeclarationElement)
                    {
                        declares.Add((MethodDeclarationElement)e);
                    }
                }

                /* search body for all methods */
                List<MethodElement> methods = new List<MethodElement>();
                List<int> methodIndexes = new List<int>();

                for (var x = 0; x < Body.Count; x++)
                {
                    if (Body[x] is MethodElement)
                    {
                        methods.Add((MethodElement)Body[x]);
                        methodIndexes.Add(x);
                    }
                }

                for (var x = 0; x < declares.Count; x++)
                {
                    MethodElement currMethod = methods.Where(m => m.MethodName == declares[x].MethodName).First();
                    Body[methodIndexes[x]] = currMethod;
                }
            }

            if (state.Options.PairGetSets)
            {
                List<GetterElement> getters = new List<GetterElement>();
                List<SetterElement> setters = new List<SetterElement>();
                List<int> indexes = new List<int>();

                for(var x = 0; x < Body.Count; x++)
                {
                    if (Body[x] is GetterElement)
                    {
                        getters.Add((GetterElement)Body[x]);
                        indexes.Add(x);
                    }
                    if (Body[x] is SetterElement)
                    {
                        setters.Add((SetterElement)Body[x]);
                        indexes.Add(x);
                    }
                }

                List<Tuple<GetterElement, SetterElement>> pairs = new List<Tuple<GetterElement, SetterElement>>();

                foreach (var g in getters)
                {
                    var s = setters.Where(p => p.PropertyName == g.PropertyName).FirstOrDefault();
                    if (s != null) {
                        pairs.Add(new Tuple<GetterElement, SetterElement>(g, s));
                    }
                }

                /* remove paired items from other lists */
                foreach (var t in pairs)
                {
                    getters.Remove(t.Item1);
                    setters.Remove(t.Item2);
                }
                var y = 0;
                for (var x = 0; x < pairs.Count; x+=2)
                {
                    Body[indexes[x]] = pairs[y].Item1;
                    Body[indexes[x + 1]] = pairs[y++].Item2;
                }

                /* add leftover getters */
                for (var x = pairs.Count; x < getters.Count; x++)
                {
                    Body[indexes[x]] = getters[x];
                }

                /* add leftover setters */
                for (var x = pairs.Count + getters.Count; x < setters.Count; x++)
                {
                    Body[indexes[x]] = setters[x];
                }
            }
        }
    }
}
