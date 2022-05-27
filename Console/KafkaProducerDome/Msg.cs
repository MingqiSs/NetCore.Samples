using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaProducerDome
{
    public class Msg : KafkaMessage
    {
        public string name { get; set; }

        public string text { get; set; }
    }
}
