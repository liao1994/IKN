using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;

/// <summary>
/// Link.
/// </summary>
namespace Linklaget
{
	/// <summary>
	/// Link.
	/// </summary>
	public class Link
	{
		/// <summary>
		/// The DELIMITE for slip protocol.
		/// </summary>
		const byte DELIMITER = (byte)'A';

	    const byte END = (byte) 'A';
	    const byte ESC = (byte) 'B';
        const byte ESC_END = (byte) 'C';
        const byte ESC_ESC = (byte) 'D';
		/// <summary>
		/// The buffer for link.
		/// </summary>
		private byte[] buffer;
		/// <summary>
		/// The serial port.
		/// </summary>
		SerialPort serialPort;

		/// <summary>
		/// Initializes a new instance of the <see cref="link"/> class.
		/// </summary>
		public Link (int BUFSIZE)
		{
			// Create a new SerialPort object with default settings.
			serialPort = new SerialPort("/dev/ttyS1",115200,Parity.None,8,StopBits.One);

			if(!serialPort.IsOpen)
				serialPort.Open();

			buffer = new byte[(BUFSIZE*2)];

			serialPort.ReadTimeout = 200;
			serialPort.WriteTimeout = 200;
			serialPort.DiscardInBuffer ();
			serialPort.DiscardOutBuffer ();
		}

		/// <summary>
		/// Send the specified buf and size.
		/// </summary>
		/// <param name='buf'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public void send (byte[] buf, int size)
		{
            
            List<byte> listofbytes = new List<byte>();
            listofbytes.Add(END);
		    foreach (var element in buf)
		    {
		        if (element == END)
		        {
		            listofbytes.Add(ESC);
		            listofbytes.Add(ESC_END);
		        }
		        else if (element == ESC)
		        {
		            listofbytes.Add(ESC);
		            listofbytes.Add(ESC_ESC);
		        }
		        else
		        {
                    listofbytes.Add(element);
		        }
		    }
            listofbytes.Add(END);
            var vbuf = new byte[listofbytes.Count];
		    for (var i = 0; i < listofbytes.Count; i++)
		    {
                vbuf[i] = listofbytes[i];
            }

			serialPort.WriteLine(Convert.ToString(vbuf));// TO DO Your own code

        }

		/// <summary>
		/// Receive the specified buf and size.
		/// </summary>
		/// <param name='buf'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public int receive (ref byte[] buf)
		{
            Array.Copy(buf, 0, buffer, 1, buf.Length);
            // TO DO Your own code
            List<byte> listofbytes = new List<byte>();
		    if (buf[0] == END && buf[buf.Length] == END)
		    {
                for (int i = 1; i < buf.Length-1; i++)
                {
                    if (buf[i] == ESC)
                    {
                        if (buf[i + 1] == ESC_END)
                        {
                            listofbytes.Add(END);
                        }
                        if (buf[i + 1] == ESC_ESC)
                        {
                            listofbytes.Add(ESC);
                        }
                    }
                    
                }
            }
		    if (listofbytes.Count != 0)
		    {
                var myByte = new byte[listofbytes.Count];
                for (int i = 1; i < buf.Length; i++)
                {
                    myByte[i] = listofbytes[i];
                }
                buf = myByte;
		        return listofbytes.Count;
		    }

            return 0; 
		}
	}
}
