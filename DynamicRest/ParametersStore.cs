using System.Collections.Generic;

namespace DynamicRest {

    public class ParametersStore {

        private Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public object GetParameter(string parameterName) {
            return _parameters.ContainsKey(parameterName) ? _parameters[parameterName] : null;
        }

        public void SetParameter(string key, object value) {
            if (Contains(key))
            {
                _parameters[key] = value;
            }

            _parameters.Add(key, value);
        }

        public bool Contains(string key) {
            return _parameters.ContainsKey(key);
        }
    }
}