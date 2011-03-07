using System;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest.Helpers
{
    public static class HttpVerbHelpers
    {
        public static HttpVerb ToHttpVerb(this string operationName)
        {
            HttpVerb result;
            if (Enum.TryParse(operationName, true, out result))
            {
                return result;
            }

            throw new InvalidOperationException(string.Format("The operation {0} is not a valid Http verb", operationName));
        }
    }
}
