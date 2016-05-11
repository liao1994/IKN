using System;
using System.Text;

namespace Transportlaget
{
    public class Checksum
    {
        private long checksum(byte[] buf)
        {
            int i = 0, length = buf.Length;
            long sum = 0;
            while (length > 0)
            {
                sum += (buf[i++] & 0xff) << 8;
                if (--length == 0) break;
                sum += buf[i++] & 0xff;
                --length;
            }

            return ~((sum & 0xFFFF) + (sum >> 16)) & 0xFFFF;
        }

        public bool checkChecksum(byte[] buf, int size)
        {
            Console.WriteLine("ehm CheckSum should be 123 and 189");
            var buffer = new byte[size-2];
            Array.Copy(buf, (int) TransSize.CHKSUMSIZE, buffer, 2, size-2);
            Console.WriteLine("something: " + Encoding.ASCII.GetString(buf));
            Console.WriteLine((buf[(int)TransCHKSUM.CHKSUMHIGH] << 8 | buf[(int)TransCHKSUM.CHKSUMLOW]));
            Console.WriteLine(checksum(buffer));
            return checksum(buffer) == (buf[(int) TransCHKSUM.CHKSUMHIGH] << 8 | buf[(int) TransCHKSUM.CHKSUMLOW]);
        }

        public void calcChecksum(ref byte[] buf, int size)
        {
            Console.WriteLine("calcCheckSum para info: "+ Encoding.ASCII.GetString(buf) + " " + size);
            var buffer = new byte[size - 2];

            Array.Copy(buf, (int)TransSize.CHKSUMSIZE, buffer, 0, size-2);
            var sum = checksum(buffer);
            Console.WriteLine(Encoding.ASCII.GetString(buffer) + " and checksum is " + ((sum >> 8) & 255) + " "+ (sum & 255));
            Console.WriteLine();

            buf[(int) TransCHKSUM.CHKSUMHIGH] = (byte) ((sum >> 8) & 255);
            buf[(int) TransCHKSUM.CHKSUMLOW] = (byte) (sum & 255);


        }
    }
}