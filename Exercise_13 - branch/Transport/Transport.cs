using System;
using System.Threading.Tasks;
using Linklaget;

/// <summary>
/// Transport.
/// </summary>
namespace Transportlaget
{
	/// <summary>
	/// Transport.
	/// </summary>
	public class Transport
	{
		/// <summary>
		/// The link.
		/// </summary>
		private Link link;
		/// <summary>
		/// The 1' complements checksum.
		/// </summary>
		private Checksum checksum;
		/// <summary>
		/// The buffer.
		/// </summary>
		private byte[] buffer;
		/// <summary>
		/// The seq no.
		/// </summary>
		private byte seqNo;
		/// <summary>
		/// The old_seq no.
		/// </summary>
		private byte old_seqNo;
		/// <summary>
		/// The error count.
		/// </summary>
		private int errorCount;
		/// <summary>
		/// The DEFAULT_SEQNO.
		/// </summary>
		private const int DEFAULT_SEQNO = 2;

		/// <summary>
		/// Initializes a new instance of the <see cref="Transport"/> class.
		/// </summary>
		public Transport (int BUFSIZE)
		{
			link = new Link(BUFSIZE+(int)TransSize.ACKSIZE);
			checksum = new Checksum();
			buffer = new byte[BUFSIZE+(int)TransSize.ACKSIZE];
			seqNo = 0;
			old_seqNo = DEFAULT_SEQNO;
			errorCount = 0;
		}

		/// <summary>
		/// Receives the ack.
		/// </summary>
		/// <returns>
		/// The ack.
		/// </returns>
		private bool receiveAck()
		{
			byte[] buf = new byte[(int)TransSize.ACKSIZE];
			int size = link.receive(ref buf);
			if (size != (int)TransSize.ACKSIZE) return false;
			if(!checksum.checkChecksum(buf, (int)TransSize.ACKSIZE) ||
					buf[(int)TransCHKSUM.SEQNO] != seqNo ||
					buf[(int)TransCHKSUM.TYPE] != (int)TransType.ACK)
				return false;
			
			seqNo = (byte)((buf[(int)TransCHKSUM.SEQNO] + 1) % 2);
			
			return true;
		}

		/// <summary>
		/// Sends the ack.
		/// </summary>
		/// <param name='ackType'>
		/// Ack type.
		/// </param>
		private void sendAck (bool ackType)
		{
			byte[] ackBuf = new byte[(int)TransSize.ACKSIZE];
			ackBuf [(int)TransCHKSUM.SEQNO] = (byte)
					(ackType ? (byte)buffer [(int)TransCHKSUM.SEQNO] : (byte)(buffer [(int)TransCHKSUM.SEQNO] + 1) % 2);
			ackBuf [(int)TransCHKSUM.TYPE] = (byte)(int)TransType.ACK;
			checksum.calcChecksum (ref ackBuf, (int)TransSize.ACKSIZE);

			link.send(ackBuf, (int)TransSize.ACKSIZE);
		}

		/// <summary>
		/// Send the specified buffer and size.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public void send(byte[] buf, int size)
		{
		    var checksumbyteheader = buf;
		    int bytesSent = 0;
		    byte seqnumber = 0;
		    byte type = 0; 
            int bytesLeft = size;

            while (bytesLeft > 0)
            {
                int nextPacketSize = (bytesLeft > 1000) ? 1000 : bytesLeft;
                var buffer= new byte[nextPacketSize];
                for (int i = 0; i < nextPacketSize; i++)
                {
                    buffer[i] = buf[bytesSent + i];

                }
                var buf2 = buffer;
                checksum.calcChecksum(ref buffer,nextPacketSize);
                byte[] sendingThis = new byte[nextPacketSize+4];
                sendingThis[0] = buffer[0];
                sendingThis[1] = buffer[1];
                sendingThis[2] = seqnumber;
                sendingThis[3] = type;
                for (int i = 0; i < nextPacketSize; i++)
                {
                    sendingThis[i + 4] = buf2[i];
                }
                bytesSent += nextPacketSize;
                bytesLeft -= nextPacketSize;
                seqnumber++;
                link.send(sendingThis, nextPacketSize);
                // wait for acknowledge
                while (!receiveAck())
                {
                    link.send(sendingThis, nextPacketSize);
                }
            }
		}

		/// <summary>
		/// Receive the specified buffer.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		public int receive (ref byte[] buf)
		
            sendAck();
		}
	}

}