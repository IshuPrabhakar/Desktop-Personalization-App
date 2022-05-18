using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Model
{
    public class PexelsPhoto
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("url")]
        public string URL { get; set; }
        [JsonProperty("photographer")]
        public string Photographer { get; set; }
        [JsonProperty("photographer_url")]
        public string Photographer_Url { get; set; }
        [JsonProperty("photographer_id")]
        public string Photographer_id { get; set; }
        [JsonProperty("avg_Color")]
        public string Avg_Color { get; set; }
        [JsonProperty("src")]
        public Src src { get; set; }
        [JsonProperty("liked")]
        public bool Liked { get; set; }
        [JsonProperty("alt")]
        public string Alt { get; set; }


    }

    public class Src
    {
        [JsonProperty("original")]
        public string Original { get; set; }
        [JsonProperty("large2x")]
        public string Large2x { get; set; }
        [JsonProperty("large")]
        public string Large { get; set; }
        [JsonProperty("medium")]
        public string Medium { get; set; }
        [JsonProperty("small")]
        public string Small { get; set; }
        [JsonProperty("portrait")]
        public string Portrait { get; set; }
        [JsonProperty("landscape")]
        public string Landscape { get; set; }
        [JsonProperty("tiny")]
        public string Tiny { get; set; }
    }
}
