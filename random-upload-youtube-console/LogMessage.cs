using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_upload_youtube_console
{
    public static class LogMessage
    {
        public async static void Log(string massage)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("log.txt", true))
            {
                file.WriteLine(massage);
            }
        }
    }
}
