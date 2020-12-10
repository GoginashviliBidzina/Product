using System;

namespace Product.Messaging.Models
{
    public class DefaultEventModel
    {
        public string Message { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public DefaultEventModel(string message,
                                 DateTime occuredOn)
        {
            Message = message;
            OccuredOn = occuredOn;
        }
    }
}
