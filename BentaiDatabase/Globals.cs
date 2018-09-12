using System.IO;

namespace BentaiDataBase
{
    internal class Globals
    {
        static Globals()
        {
            scriptDirectory = Directory.GetCurrentDirectory();
            dataBaseString = $@"Data Source={scriptDirectory}\Images\images.sqlite;version=3";
        }

        internal static readonly string scriptDirectory;

        internal static readonly string dataBaseString;
    }
}
