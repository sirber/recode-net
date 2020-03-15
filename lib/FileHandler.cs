using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recode.net.lib
{
    class FileHandler
    {
        static public string GetDestinationFile(string SourceFile)
        {
            return $"{SourceFile}.out.mkv";
        }
    }
}
