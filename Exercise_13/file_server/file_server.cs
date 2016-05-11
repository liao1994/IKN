using System;
using Transportlaget;

namespace Application
{
    internal class file_server
    {
        /// <summary>
        ///     The BUFSIZE
        /// </summary>
        private const int BUFSIZE = 1000;

        /// <summary>
        ///     Initializes a new instance of the <see cref="file_server" /> class.
        /// </summary>
        private file_server()
        {
            // TO DO Your own code
            //Test Application in virtual machine #2 (acts only as receiver for testing Link Layer)
            var transport = new Transport(BUFSIZE);
            do
            {
                var rxBytes = new byte[BUFSIZE];

                var n = transport.receive(ref rxBytes);
                for (var x = 0; x < n; x++)
                {
                    Console.WriteLine(rxBytes[x]);
                }
                Console.ReadKey();
            } while (true);
        }

        /// <summary>
        ///     Sends the file.
        /// </summary>
        /// <param name='fileName'>
        ///     File name.
        /// </param>
        /// <param name='fileSize'>
        ///     File size.
        /// </param>
        /// <param name='tl'>
        ///     Tl.
        /// </param>
        private void sendFile(string fileName, long fileSize, Transport transport)
        {
            // TO DO Your own code
        }

        /// <summary>
        ///     The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name='args'>
        ///     The command-line arguments.
        /// </param>
        public static void Main(string[] args)
        {
            new file_server();
        }
    }
}