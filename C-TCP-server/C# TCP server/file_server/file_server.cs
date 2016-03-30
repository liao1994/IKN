using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using System.Text;


namespace tcp
{
	class file_server
	{
		/// <summary>
		/// The PORT
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE
		/// </summary>
		const int BUFSIZE = 1000;
        TcpListener serverSocket = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="file_server"/> class.
        /// Opretter en socket.
        /// Venter på en connect fra en klient.
        /// Modtager filnavn
        /// Finder filstørrelsen
        /// Kalder metoden sendFile
        /// Lukker socketen og programmet
        /// </summary>
        private file_server ()
        {		
            // Opretter en socket
            // telling server to lookout of TCPlistner of any IP addr, from port 9000
            serverSocket = new TcpListener(IPAddress.Any,PORT);
          
            TcpClient clientSocket = default(TcpClient);

            //lytter efter TCP request
            serverSocket.Start();

            while ((true))
            {
                try
                {
                    //blokerer indtil der er accepteret en TCP client
                    clientSocket = serverSocket.AcceptTcpClient();
                    Console.WriteLine(" >> Accept connection from client");

                    //tager en streamobj fra client side til at læse og skrive med
					//oversætter det til string 
					NetworkStream networkStream = clientSocket.GetStream();
					String dataFromClient = LIB.readTextTCP(networkStream);

                    Console.WriteLine(" >> Data from client - " + dataFromClient);

					long filesize = LIB.check_File_Exists(dataFromClient);
					Console.WriteLine(filesize + " filename: " + LIB.extractFileName(dataFromClient));

					if (filesize == 0)
					{
						string serverResponse = "404 - file does not exist";
						Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
						LIB.writeTextTCP(networkStream,serverResponse);
						throw new Exception("404 - file does not exist");
					}
						
					LIB.writeTextTCP(networkStream,filesize.ToString());
					Console.WriteLine("size of " + dataFromClient + " is " + filesize + " byte(s)");

                    //laver noget alt efter hvad der er modtaget??
                    sendFile(dataFromClient, filesize, networkStream);

                    clientSocket.Close();
                    serverSocket.Stop();
                    Console.WriteLine(" >> exit");
                    Console.ReadLine();
                  //  }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

		/// <summary>
		/// Sends the file.
		/// </summary>
		/// <param name='fileName'>
		/// The filename.
		/// </param>
		/// <param name='fileSize'>
		/// The filesize.
		/// </param>
		/// <param name='io'>
		/// Network stream for writing to the client.
		/// </param>
		private void sendFile (String fileName, long fileSize, NetworkStream io)
		{
			Console.WriteLine ("Sending " + fileName + " to client");
				 
            //læser og pakker fra fil
            byte[] data = File.ReadAllBytes(fileName);

		    byte[] dataLength = BitConverter.GetBytes(data.Length);
			byte[] package = new byte[BUFSIZE];
            
            int bytesSent = 0;
            int bytesLeft = data.Length;

            while (bytesLeft > 0)
            {
                int nextPacketSize = (bytesLeft > BUFSIZE) ? BUFSIZE : bytesLeft;
				//LIB.writeTextTCP(io,line);
                io.Write(data, bytesSent, nextPacketSize);
                bytesSent += nextPacketSize;
                bytesLeft -= nextPacketSize;
            }
			Console.WriteLine ("Finished Sending..");
        }

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name='args'>
        /// The command-line arguments.
        /// </param>
        public static void Main (string[] args)
		{
			Console.WriteLine ("Server starts...");
		    new file_server();
		}
	}
}
