using System.Text;

namespace ArchsimLib.Utilities
{
    public static class Logger
    {
        public static StringBuilder log = new StringBuilder();

        public static void WriteLine(string s)
        {
            log.AppendLine(s);
        }

    }
}
