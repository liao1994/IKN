using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_client
	{
		/// <summary>
		/// The PORT.
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_client"/> class.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments. First ip-adress of the server. Second the filename
		/// </param>
		private file_client (string[] args)
		{
            _clientSocket.Connect(args[0], PORT);
            Console.WriteLine("Client Socket Program - Server Connected ...");

            NetworkStream serverStream = _clientSocket.GetStream();

            // Translate the passed message into ASCII and store it as a Byte array.
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(args[1]);

            //skriver til den connected tcp server
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            //response part

            byte[] inStream = new byte[BUFSIZE];
            serverStream.Read(inStream, 0, (int)_clientSocket.ReceiveBufferSize);
            //læser en string reponse fra server
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);

            receiveFile(returndata, serverStream);
            // TO DO Your own code
        }

		/// <summary>
		/// Receives the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='io'>
		/// Network stream for reading from the server
		/// </param>
		private void receiveFile (String fileName, NetworkStream io)
		{
            // TO DO Your own code
            //do something with the string, then close the stream..
            io.Close();
            _clientSocket.Close();
        }

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client(args);
		}
	}
}
