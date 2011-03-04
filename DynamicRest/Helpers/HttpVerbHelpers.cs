using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynamicRest.HTTPInterfaces;

namespace DynamicRest.Helpers
{
    public static class HttpVerbHelpers
    {
        public static HttpVerb ToHttpVerb(this string operationName) {
            return ((HttpVerb)Enum.Parse(typeof(HttpVerb), operationName));
        }
    }
}
