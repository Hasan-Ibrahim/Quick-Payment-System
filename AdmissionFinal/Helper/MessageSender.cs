using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SendMessage;

namespace AdmissionFinal.Helper
{
    public class MessageSender
    {
        public MessageSender(string mobNumber,string message)
        {
            var configuration = new Configuration
            {
                ComPort = "COM4",
                PortSpeed = 230400,
                MessageToSend = message,
                ReceiverMobileNo = "+88"+mobNumber,
                SmsServiceCentre = "+8801700000600"
            };


            var messageHelper = new MessageHelper(configuration);
            messageHelper.SendMessage();

        }
    }
}