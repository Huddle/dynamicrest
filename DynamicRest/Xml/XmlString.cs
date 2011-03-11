using System.Dynamic;

namespace DynamicRest.Xml {

    public class XmlString : DynamicObject {

        readonly string value;

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
    }
}