using System;
using System.Runtime.Serialization;

namespace TrainingCsharpGft.Api.Exceptions
{
    public class WrongValueException : Exception
    {
        public WrongValueException() : base("Wrong value") //default error message
        {
        }

        public WrongValueException(string message) : base(String.Format("Wrong value - {0}", message))
        {
        }

        public WrongValueException(string message, Exception innerException) 
            : base(String.Format("Wrong value - {0}", message), innerException)
        {
        }

        protected WrongValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
