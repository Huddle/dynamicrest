// XmlNode.cs
// DynamicRest provides REST service access using C# 4.0 dynamic programming.
// The latest information and code for the project can be found at
// https://github.com/NikhilK/dynamicrest
//
// This project is licensed under the BSD license. See the License.txt file for
// more information.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Dynamic;

namespace DynamicRest.Xml {

    public sealed class XmlNode : DynamicObject {

        private XElement _element;

        public XmlNode(string name)
            : this(new XElement(name)) {
        }

        public XmlNode(XElement element) {
            _element = element;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result) {
            if (_element.HasElements) {
                new XmlNodeList(new[] { _element }).TryGetIndex(binder, indexes, out result);
            }
            else {
                result = _element.Value;
            }
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            string name = binder.Name;

            if (String.CompareOrdinal(name, "Name") == 0) {
                result = _element.Name.LocalName;
                return true;
            }
            else if (String.CompareOrdinal(name, "Parent") == 0) {
                XElement parent = _element.Parent;
                if (parent != null) {
                    result = new XmlNode(parent);
                    return true;
                }
                result = null;
                return false;
            }
            else if (String.CompareOrdinal(name, "Value") == 0) {
                result = _element.Value;
                return true;
            }
            else if (String.CompareOrdinal(name, "Count") == 0) {
                result = _element.Elements().Count();
                return true;
            }
            else if (String.CompareOrdinal(name, "Nodes") == 0) {
                result = new XmlNodeList(_element.Elements());
                return true;
            }
            else if (String.CompareOrdinal(name, "XElement") == 0)
            {
                result = _element;
                return true;
            }
            else if (String.CompareOrdinal(name, "Xml") == 0) {
                StringWriter sw = new StringWriter();
                _element.Save(sw, SaveOptions.None);

                result = sw.ToString();
                return true;
            }
            else {
                XAttribute attribute = _element.Attributes().SingleOrDefault(a => a.Name.LocalName == name);
                if (attribute != null) {
                    result = attribute.Value;
                    return true;
                }
                try {
                    XElement childNode = _element.Elements().SingleOrDefault(a => a.Name.LocalName == name);
                    if (childNode != null)
                    {
                        if (childNode.HasElements == false)
                        {
                            result = new XmlString(childNode.Value);
                            return true;
                        }
                        result = new XmlNode(childNode);
                        return true;
                    }
                }
                catch (InvalidOperationException)
                {
                    
                    result = new XmlNodeList(_element.Elements().Where(a => a.Name.LocalName == name));
                    return true;
                }
            }

            var memberExists = base.TryGetMember(binder, out result);
            if (result == null) {
                throw new XmlException(string.Format("No element or attribute named '{0}' found in the response.", name));
            }
            return memberExists;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) {
            string name = binder.Name;

            if (String.CompareOrdinal(name, "SelectAll") == 0) {
                IEnumerable<XElement> selectedElements = null;

                if (args.Length == 0) {
                    selectedElements = _element.Descendants();
                }
                else if (args.Length == 1) {
                    selectedElements = _element.Descendants().Where(d => d.Name.LocalName == args[0].ToString());
                }
                else {
                    result = false;
                    return false;
                }
                result = new XmlNodeList(selectedElements);
                return true;
            }
            else if (String.CompareOrdinal(name, "SelectChildren") == 0) {
                IEnumerable<XElement> selectedElements = null;

                if (args.Length == 0) {
                    selectedElements = _element.Elements();
                }
                else if (args.Length == 1) {
                    selectedElements = _element.Elements().Where(d => d.Name.LocalName == args[0].ToString());
                }
                else {
                    result = false;
                    return false;
                }
                result = new XmlNodeList(selectedElements);
                return true;
            }
         
            return base.TryInvokeMember(binder, args, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value) {
            string name = binder.Name;

            if (String.CompareOrdinal(name, "Value") == 0) {
                _element.Value = (value != null) ? value.ToString() : String.Empty;
                return true;
            }

            return base.TrySetMember(binder, value);
        }
    }
}
