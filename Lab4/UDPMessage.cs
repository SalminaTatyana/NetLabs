using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab4
{
    [Serializable]
    public class UDPMessage
    {
        public bool IsCheck { get; set; }
        public int Length { get; set; }
        public byte[] Message { get; set; }
}
    public class UDPMessageManager
    {
        // кодирование экземпляра
        public static byte[] CodingAsync(UDPMessage source)
        {
            string jsonString =  JsonSerializer.Serialize(source);
            return ISO.Code(jsonString);
        }
        // декодирование экземпляра
        public static UDPMessage Decoding(byte[] code)
        {
            String readOnlySpan = ISO.Decode(code);
            return JsonSerializer.Deserialize<UDPMessage>(readOnlySpan);
        }
    }
}
