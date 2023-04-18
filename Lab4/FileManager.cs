using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab4
{
    public class FileManager
    {
       public static string path = DateTime.Now.ToString("dd.MM.yyyy_hh-mm-ss") + ".txt";
        public static void Save(byte[] strBin2)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Append)))
            {
                foreach (var item in strBin2)
                {
                    writer.Write(item);
                }
            }
        }
    }

    
}
