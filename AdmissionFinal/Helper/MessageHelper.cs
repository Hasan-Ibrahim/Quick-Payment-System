using System.Threading;
using SendMessage;
using org.smslib.modem;
using org.smslib;

namespace AdmissionFinal.Helper
{
    public class MessageHelper
    {
        public string ComPort { get; set; }
        public int PortSpeed { get; set; }
        public string ReceiverMobileNo { get; set; }
        public string SmsServiceCentre { get; set; }
        public string MessageToSend { get; set; }
        public MessageHelper(Configuration configuration)
        {
            ComPort = configuration.ComPort;
            PortSpeed = configuration.PortSpeed;
            ReceiverMobileNo = configuration.ReceiverMobileNo;
            SmsServiceCentre = configuration.SmsServiceCentre;
            MessageToSend = configuration.MessageToSend;
        }

        public void SendMessage()
        {

            var srv = Service.getInstance();


            var com3 = new Comm2IP.Comm2IP(new byte[] { 127, 0, 0, 1 }, 12000, ComPort, PortSpeed);

            try
            {
                new Thread(com3.Run).Start();


                var gateway = new IPModemGateway("modem.com3", "127.0.0.1", 12000, "", "");
                gateway.setIpProtocol(ModemGateway.IPProtocols.BINARY);
                gateway.setProtocol(AGateway.Protocols.PDU);
                gateway.setInbound(true);
                gateway.setOutbound(true);
                gateway.setSmscNumber(SmsServiceCentre);


                srv.addGateway(gateway);


                srv.startService();

                var msg = new OutboundMessage(ReceiverMobileNo, MessageToSend);
                srv.sendMessage(msg);



            }
            finally
            {
                com3.Stop();
                srv.stopService();
            }
        }
    }

}
