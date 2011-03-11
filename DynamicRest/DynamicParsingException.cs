using System;

namespace DynamicRest {

    public class DynamicParsingException : Exception {

        public DynamicParsingException(string message) 
            : base(message) {}
    }
}