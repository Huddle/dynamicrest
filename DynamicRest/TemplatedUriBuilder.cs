using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DynamicRest
{
    internal class TemplatedUriBuilder
    {
        private static readonly Regex TokenFormatRewriteRegex =
            new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
                      RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private IRestUriTransformer _uriTransformer;
        private HashSet<string> _addedParameters;

        public string UriTemplate { get; set; }

        public ParametersStore ParametersStore { private get; set; }

        public Uri CreateRequestUri(string operationName, JsonObject parameters)
        {
            var uriBuilder = new StringBuilder();

            BuildBaseUri(operationName, uriBuilder);

            AddQueryString(uriBuilder, parameters);

            Uri uri = Transform(uriBuilder);

            return uri;
        }

        public void SetUriTransformer(IRestUriTransformer uriTransformer)
        {
            _uriTransformer = uriTransformer;
        }
        
        private void BuildBaseUri(string operationName, StringBuilder uriBuilder)
        {
            var values = new List<object>();
            _addedParameters = null;

            string rewrittenUriFormat = TokenFormatRewriteRegex.Replace(UriTemplate, delegate(Match m)
            {
                Group startGroup = m.Groups["start"];
                Group propertyGroup = m.Groups["property"];
                Group formatGroup = m.Groups["format"];
                Group endGroup = m.Groups["end"];

                if ((operationName.Length != 0) && String.CompareOrdinal(propertyGroup.Value, "operation") == 0)
                {
                    values.Add(operationName);
                }
                else if (this.ParametersStore != null && this.ParametersStore.Contains(propertyGroup.Value))
                {
                    values.Add(this.ParametersStore.GetParameter(propertyGroup.Value));

                    if (_addedParameters == null)
                    {
                        _addedParameters = new HashSet<string>(StringComparer.Ordinal);
                    }

                    _addedParameters.Add(propertyGroup.Value);
                }

                return new string('{', startGroup.Captures.Count) + (values.Count - 1) + formatGroup.Value + new string('}', endGroup.Captures.Count);
            });

            if (values.Count != 0)
            {
                uriBuilder.AppendFormat(CultureInfo.InvariantCulture, rewrittenUriFormat, values.ToArray());
            }

            else if (UriTemplate != rewrittenUriFormat)
            {
                throw new ArgumentException(string.Format("You are missing one or more expected template parameters in the uri: {0}", UriTemplate));
            }
            else
            {
                uriBuilder.Append(rewrittenUriFormat);
            }
        }

        private void AddQueryString(StringBuilder uriBuilder, JsonObject parameters){
            if (parameters != null){

                if (UriTemplate.IndexOf('?') < 0)
                {
                    uriBuilder.Append("?");
                }

                foreach (var param in parameters){
                    string name = param.Key;
                    object value = param.Value;

                    if (ConsumedInBaseUri(name)){
                        continue;
                    }

                    if (value is Delegate){
                        // Ignore callbacks in the async scenario.
                        continue;
                    }

                    if (value is JsonObject){
                        // Nested object... use name.subName=value format.
                        foreach (KeyValuePair<string, object> nestedParam in (IDictionary<string, object>)value){
                            uriBuilder.AppendFormat("&{0}.{1}={2}",
                                                    name, nestedParam.Key,
                                                    FormatUriParameter(nestedParam.Value));
                        }

                        continue;
                    }

                    uriBuilder.AppendFormat("&{0}={1}", name, FormatUriParameter(value));
                }
            }
        }

        private bool ConsumedInBaseUri(string name)
        {
            return (_addedParameters != null) && _addedParameters.Contains(name);
        }

        private Uri Transform(StringBuilder uriBuilder){
            var uri = new Uri(uriBuilder.ToString(), UriKind.Absolute);
            if (_uriTransformer != null){
                uri = _uriTransformer.TransformUri(uri);
            }
            return uri;
        }

        internal string FormatUriParameter(object value){
            if (value is IEnumerable<string>) {
                return String.Join("+", (IEnumerable<string>)value);
            }
            return HttpUtility.UrlEncode(String.Format(CultureInfo.InvariantCulture, "{0}", value));
        }

     }
}
 
