using System;
using Transportlaget;

namespace Application
{
    internal class file_client
    {
        /// <summary>
        ///     The BUFSIZE.
        /// </summary>
        private const int BUFSIZE = 1000;


        /// <summary>
        ///     Initializes a new instance of the <see cref="file_client" /> class.
        ///     file_client metoden opretter en peer-to-peer forbindelse
        ///     Sender en forspÃ¸rgsel for en bestemt fil om denne findes pÃ¥ serveren
        ///     Modtager filen hvis denne findes eller en besked om at den ikke findes (jvf. protokol beskrivelse)
        ///     Lukker alle streams og den modtagede fil
        ///     Udskriver en fejl-meddelelse hvis ikke antal argumenter er rigtige
        /// </summary>
        /// <param name='args'>
        ///     Filnavn med evtuelle sti.
        /// </param>
        private file_client(string[] args)
        {
            var t = new Transport(BUFSIZE);
            var buf = new byte[BUFSIZE];
            buf[0] = Convert.ToByte('A');
            buf[1] = Convert.ToByte('B');
            buf[2] = Convert.ToByte('C');
            Console.WriteLine("Starting up...");

            t.send(buf, buf.Length);
            var running = true;
            while (running)
            {
                var x = Console.ReadKey();
                switch (x.Key)
                {
                    case ConsoleKey.S:
                        Console.WriteLine("sent...");
                        t.send(buf, buf.Length);
                        break;
                    case ConsoleKey.Q:
                        running = false;
                        break;
                }
            }
        }

        /// <summary>
        ///     Receives the file.
        /// </summary>
        /// <param name='fileName'>
        ///     File name.
        /// </param>
        /// <param name='transport'>
        ///     Transportlaget
        /// </param>
        private void receiveFile(string fileName, Transport transport)
        {
            // TO DO Your own code
        }

        /// <summary>
        ///     The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name='args'>
        ///     First argument: Filname
        /// </param>
        public static void Main(string[] args)
        {
            new file_client(args);
        }
    }
}