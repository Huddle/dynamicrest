using System;
using System.Dynamic;
using System.Globalization;

namespace DynamicRest.Xml {

    public class XmlString : DynamicObject {

        readonly string value;
        private const string isoDateFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";

        public XmlString(string value) {
            this.value = value;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result) {
            result = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            result = value;
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result) {
            result = value;
            return true;
        }

        public override bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result) {
            result = value;
            return true;
        }

        public static implicit operator DateTime(XmlString input)
        {
            return DateTime.Parse(input.value);
        }

        public static implicit operator long(XmlString input)
        {
            return long.Parse(input.value);
        }

        public static implicit operator int(XmlString input)
        {
            return int.Parse(input.value);
        }

        public static implicit operator bool(XmlString input)
        {
            if (input.value == "0" || input.value == "-1") return false;
            if (input.value == "1") return true;
            return bool.Parse(input.value);
        }
    }
}