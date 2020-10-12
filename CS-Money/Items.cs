using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CS_Money
{
    public class Items
    {
        const string url = "https://csm.auction/api/skins_base";
        public Dictionary<string, DeserializeItem> GetCSGOItems()
        {
            HttpClient client = new HttpClient();
            Encoding.GetEncoding("ISO-8859-1");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            var data = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Dictionary<string, DeserializeItem>>(data);
        }
        public SearchItem FoundItem(string name)
        {
            SearchItem output = new SearchItem();
            HttpClient client = new HttpClient();
            Encoding.GetEncoding("ISO-8859-1");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            var data = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            var desitems = JsonConvert.DeserializeObject<Dictionary<string, DeserializeItem>>(data);
            foreach (var found in desitems)
            {
                if (name == found.Value.M)
                {
                    var pros = 15;
                    double num = Convert.ToDouble(found.Value.A);
                    double procount = num / 100 * pros;
                    double result = num + procount;
                    double finalres = Math.Round(result, 2);

                    output.CustomeId = Convert.ToInt32(found.Key);
                    output.Name = found.Value.M;
                    output.Price = finalres;
                    output.Quality = found.Value.E;
                }
                else
                {
                    return null;
                }
            }
            return output;
        }
    }
    public class SearchItem
    {
        public int CustomeId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Quality { get; set; }

    }
    public class DeserializeItem
    {
        [JsonProperty("u")]
        public string U { get; set; }

        [JsonProperty("m")]
        public string M { get; set; }

        [JsonProperty("a")]
        public double A { get; set; }

        [JsonProperty("c", NullValueHandling = NullValueHandling.Ignore)]
        public long? C { get; set; }

        [JsonProperty("g")]
        public G G { get; set; }

        [JsonProperty("z", NullValueHandling = NullValueHandling.Ignore)]
        public long? Z { get; set; }

        [JsonProperty("e", NullValueHandling = NullValueHandling.Ignore)]
        public string E { get; set; }

        [JsonProperty("w", NullValueHandling = NullValueHandling.Ignore)]
        public long? W { get; set; }

        [JsonProperty("v", NullValueHandling = NullValueHandling.Ignore)]
        public long? V { get; set; }

        [JsonProperty("h")]
        public long H { get; set; }

        [JsonProperty("j")]
        public string J { get; set; }
    }
    public partial struct G
    {
        public long? Integer;
        public string String;

        public static implicit operator G(long Integer) => new G { Integer = Integer };
        public static implicit operator G(string String) => new G { String = String };
    }
}
