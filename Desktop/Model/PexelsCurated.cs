using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Model
{
    public class PexelsCurated
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("per_page")]
        public int Per_page { get; set; }
        [JsonProperty("photos")]
        public List<PexelsPhoto> Photos { get; set; }
        [JsonProperty("total_results")]
        public int Total_Results { get; set; }
        [JsonProperty("next_page")]
        public string Next_page { get; set; }
    }
}
