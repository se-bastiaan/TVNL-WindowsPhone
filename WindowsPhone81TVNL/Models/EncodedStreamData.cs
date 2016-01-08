using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WindowsPhone81TVNL.Helpers;

namespace WindowsPhone81TVNL.Models
{
    public class EncodedStreamData
    {
        public EncodedStreamData()
        {}

        public EncodedStreamData(EncodedStreamData data)
        {
            CipherText = data.CipherText;
            InitVector = data.CipherText;
            Subtitles = data.Subtitles;
            PlainText = EncryptionHelper.DecodeAndDecrypt(data.CipherText, data.InitVector);
            Debug.WriteLineIf(PlainText != null, PlainText);
        }

        [JsonProperty("data")]
        public string CipherText { get; set; }
        [JsonProperty("iv")]
        public string InitVector { get; set; }
        [JsonProperty("tt888")]
        public bool Subtitles { get; set; }
        public string PlainText { get; set; }
    }
}
