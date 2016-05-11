using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

/// <summary>
/// Link.
/// </summary>

namespace Linklaget
{
    /// <summary>
    ///     Link.
    /// </summary>
    public class Link
    {
        /// <summary>
        ///     The DELIMITE for slip protocol.
        /// </summary>
        private const byte DELIMITER = (byte) 'A';

        private const byte END = (byte) 'A';
        private const byte ESC = (byte) 'B';
        private const byte ESC_END = (byte) 'C';
        private const byte ESC_ESC = (byte) 'D';

        /// <summary>
        ///     The buffer for link.
        /// </summary>
        private readonly byte[] buffer;

        /// <summary>
        ///     The serial port.
        /// </summary>
        private readonly SerialPort serialPort;

        /// <summary>
        ///     Initializes a new instance of the <see cref="link" /> class.
        /// </summary>
        public Link(int BUFSIZE)
        {
            // Create a new SerialPort object with default settings.
            serialPort = new SerialPort("/dev/ttyS1", 115200, Parity.None, 8, StopBits.One);

            if (!serialPort.IsOpen)
                serialPort.Open();

            buffer = new byte[BUFSIZE*2];

            serialPort.ReadTimeout = 200;
            serialPort.WriteTimeout = 200;
            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();
        }

        /// <summary>
        ///     Send the specified buf and size.
        /// </summary>
        /// <param name='buf'>
        ///     Buffer.
        /// </param>
        /// <param name='size'>
        ///     Size.
        /// </param>
        public void send(byte[] buf, int size)
        {
            var listofbytes = new List<byte>();
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
            Console.WriteLine("Link Writing to SerialPort" + Encoding.ASCII.GetString(vbuf));
            serialPort.WriteLine(Encoding.ASCII.GetString(vbuf)); // TO DO Your own code

        }

        /// <summary>
        ///     Receive the specified buf and size.
        /// </summary>
        /// <param name='buf'>
        ///     Buffer.
        /// </param>
        /// <param name='size'>
        ///     Size.
        /// </param>
        public int receive(ref byte[] buf)
        {
            byte b;
            do
            {
                try
                {
                    b = (byte) serialPort.ReadByte();
                }
                catch (Exception)
                {
                    b = 0;
                }
            } while (b != 'A');
            var x = 0;
            do
            {
                try
                {
                    b = (byte) serialPort.ReadByte();
                }
                catch (Exception)
                {
                    b = 0;
                }
                buffer[x] = b;
                x++;
            } while (b != 'A');
            var y = 0;
            Console.WriteLine(Encoding.ASCII.GetString(buffer));
            for (var i = 0; i < x; i++)
            {
                if (buffer[i] != END)
                {
                    switch (buffer[i])
                    {
                        case ESC:
                            if (buffer[i+1] == ESC_END)
                            {
                                buf[y] = END;
                            }
                            else if (buffer[i+1] == ESC_ESC)
                            {
                                buf[y] = ESC;
                            }
                            ++i;
                            break;
                        default:
                            buf[y] = buffer[i];
                            break;
                    }
                    y++;
                }
                
            }
            Console.WriteLine("Link Received: " + Encoding.ASCII.GetString(buf));
            return y;
        }
    }
}