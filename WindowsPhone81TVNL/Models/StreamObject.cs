using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPhone81TVNL.Models
{
    public class StreamObject
    {
        public StreamObject(string id, bool isLive, bool isWebcam)
        {
            Id = id;
            IsLive = isLive;
            IsWebcam = isWebcam;
        }

        public StreamObject(string id, bool isLive)
        {
            Id = id;
            IsLive = isLive;
        }

        public StreamObject(string id)
        {
            Id = id;
        }

        public string Id;
        public bool IsLive = false;
        public bool IsWebcam = false;
    }
}
