using System.IO;

namespace Library
{
    public class LIB
    {
        private LIB()
        {
        }

        /// <summary>
        ///     Extracts the name of the file.
        /// </summary>
        /// <returns>
        ///     The filename only.
        /// </returns>
        /// <param name='fileName'>
        ///     Filename with path.
        /// </param>
        public static string extractFileName(string fileName)
        {
            return fileName.LastIndexOf('/') == 0 ? fileName : fileName.Substring(fileName.LastIndexOf('/') + 1);
        }

        /// <summary>
        ///     Check_s the file_ exists.
        /// </summary>
        /// <returns>
        ///     The filesize.
        /// </returns>
        /// <param name='fileName'>
        ///     The filename.
        /// </param>
        public static long check_File_Exists(string fileName)
        {
            if (File.Exists(fileName))
                return new FileInfo(fileName).Length;

            return 0;
        }
    }
}