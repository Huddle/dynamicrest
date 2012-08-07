using System;
using System.Dynamic;

namespace DynamicRest.Xml {

    public class XmlString : DynamicObject {

        readonly string _value;

        public XmlString(string value) {
            _value = value;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result) {
            result = _value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            if (String.CompareOrdinal(binder.Name, "Count") == 0)
            {
                result = 0;
                return true;
            }

            result = _value;
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result) {
            result = _value;
            return true;
        }

        public override bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result) {
            result = _value;
            return true;
        }

        public static implicit operator DateTime(XmlString input)
        {
            return DateTime.Parse(input._value);
        }

        public static implicit operator long(XmlString input)
        {
            return long.Parse(input._value);
        }

        public static implicit operator int(XmlString input)
        {
            return int.Parse(input._value);
        }

        public static implicit operator bool(XmlString input)
        {
            if (input._value == "0" || input._value == "-1") return false;
            if (input._value == "1") return true;
            return bool.Parse(input._value);
        }

        public static implicit operator string(XmlString input)
        {
            return input._value;
        }
    }
}