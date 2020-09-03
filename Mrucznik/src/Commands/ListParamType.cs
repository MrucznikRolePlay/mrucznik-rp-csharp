using System;
using System.Collections.Generic;
using System.Linq;
using SampSharp.GameMode.SAMP.Commands.ParameterTypes;

namespace Mrucznik.Commands
{
    public class ListParamType : ICommandParameterType
    {
        public bool Parse(ref string commandText, out object output, bool isNullable = false)
        {
            List<int> list = new List<int>();
            output = list;
            var words = commandText.TrimStart().Split(' ');
            foreach (var word in words)
            {
                var ok = Int32.TryParse(word, out var number);
                if (ok)
                {
                    list.Add(number);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}