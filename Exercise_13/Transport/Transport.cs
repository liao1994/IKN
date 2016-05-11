using System;
using System.Text;
using Linklaget;

/// <summary>
/// Transport.
/// </summary>

namespace Transportlaget
{
    /// <summary>
    ///     Transport.
    /// </summary>
    public class Transport
    {
        /// <summary>
        ///     The DEFAULT_SEQNO.
        /// </summary>
        private const int DEFAULT_SEQNO = 2;

        /// <summary>
        ///     The buffer.
        /// </summary>
        private byte[] buffer;

        /// <summary>
        ///     The 1' complements checksum.
        /// </summary>
        private readonly Checksum checksum;

        /// <summary>
        ///     The error count.
        /// </summary>
        private int errorCount;

        /// <summary>
        ///     The link.
        /// </summary>
        private readonly Link link;

        /// <summary>
        ///     The old_seq no.
        /// </summary>
        private byte old_seqNo;

        /// <summary>
        ///     The seq no.
        /// </summary>
        private byte seqNo;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Transport" /> class.
        /// </summary>
        public Transport(int BUFSIZE)
        {
            link = new Link(BUFSIZE + (int) TransSize.ACKSIZE);
            checksum = new Checksum();
            buffer = new byte[BUFSIZE + (int) TransSize.ACKSIZE];
            seqNo = 0;
            old_seqNo = DEFAULT_SEQNO;
            errorCount = 0;
        }

        /// <summary>
        ///     Receives the ack.
        /// </summary>
        /// <returns>
        ///     The ack.
        /// </returns>
        private bool receiveAck()
        {
            Console.WriteLine("T calling link.recieve()");
            var buf = new byte[(int) TransSize.ACKSIZE];
            var size = link.receive(ref buf);
            if (size != (int) TransSize.ACKSIZE)
            {
                Console.WriteLine(this.ToString() + "returned false");
                return false;
            }
            if (!checksum.checkChecksum(buf, (int) TransSize.ACKSIZE) ||
                buf[(int) TransCHKSUM.SEQNO] != seqNo ||
                buf[(int) TransCHKSUM.TYPE] != (int) TransType.ACK)
                return false;

            seqNo = (byte) ((buf[(int) TransCHKSUM.SEQNO] + 1)%2);
            Console.WriteLine("Return true");

            return true;
        }

        /// <summary>
        ///     Sends the ack.
        /// </summary>
        /// <param name='ackType'>
        ///     Ack type.
        /// </param>
        private void sendAck(bool ackType)
        {
            Console.WriteLine("Sending ACK after getting Package");
            var ackBuf = new byte[(int) TransSize.ACKSIZE];
            ackBuf[(int) TransCHKSUM.SEQNO] = (byte)
                (ackType ? buffer[(int) TransCHKSUM.SEQNO] : (byte) (buffer[(int) TransCHKSUM.SEQNO] + 1)%2);
            ackBuf[(int) TransCHKSUM.TYPE] = (int) TransType.ACK;
            checksum.calcChecksum(ref ackBuf, (int) TransSize.ACKSIZE);
            Console.WriteLine("link.sendack called from T: "+ ackBuf);
            link.send(ackBuf, (int) TransSize.ACKSIZE);
        }

        /// <summary>
        ///     Send the specified buffer and size.
        /// </summary>
        /// <param name='buffer'>
        ///     Buffer.
        /// </param>
        /// <param name='size'>
        ///     Size.
        /// </param>
        public void send(byte[] buf, int size)
        {
            Console.WriteLine("Transport Layer Sending package...");
            Array.Copy(buf, 0, buffer, 4, size);
            do
            {
                buffer[2] = seqNo;
                buffer[3] = (byte) TransType.DATA;
                Console.WriteLine("Calculating CheckSum...");
                checksum.calcChecksum(ref buffer, size + (int) TransSize.ACKSIZE);

                if (++errorCount == 4)
                {
                    buffer[0]++;
                    //error
                }

                Console.WriteLine("T calling link.send");
                link.send(buffer, buffer.Length);
                Console.WriteLine("T calling link.send ended");
            } while (!receiveAck());
            Console.WriteLine("T got receiveAck");
            old_seqNo = seqNo;

            #region old code

            /*var checksumbyteheader = buf;
		    int bytesSent = 0; 
		    int bytesLeft = size;
		    int seq = 0;
            while (bytesLeft > 0)
            {
                int nextPacketSize = (bytesLeft > 1000) ? 1000 : bytesLeft;
                //io.Write(data, bytesSent, nextPacketSize);
                //checksum.calcChecksum();
                bytesLeft -= nextPacketSize;
                buffer[0] = checksumbyteheader[0];
                buffer[1] = checksumbyteheader[1];
                buffer[2] = (byte)seq++;
                buffer[3] = (byte) 0;
                for (int i = 0; i < nextPacketSize; i++)
                {
                    buffer[i + 4 + bytesSent] = buf[bytesSent];
                    bytesSent++;
                }
                link.send(buffer, buffer.Length);

                //now i need to recieve ack
                var n = link.receive(ref buf);

            }*/

            #endregion
        }

        /// <summary>
        ///     Receive the specified buffer.
        /// </summary>
        /// <param name='buffer'>
        ///     Buffer.
        /// </param>
        public int receive(ref byte[] buf)
        {
            var recvSize = 0;
            var recvOk = false;
            Console.WriteLine("T waiting");
            while (!recvOk)
            {
                Console.WriteLine("T waiting in while");
                recvSize = link.receive(ref buffer);
                Console.WriteLine("T recieved something from link: " + Encoding.ASCII.GetString(buffer));

                if (recvSize <= 0) continue;
                recvOk = checksum.checkChecksum(buffer, recvSize);
                Console.WriteLine("T recieved something");
                sendAck(recvOk);
            }
            Console.WriteLine("T waiting done" + recvSize);
            return recvSize;
        }
    }
}