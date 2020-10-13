using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CS_Money
{
    public class Items
    {
        const string url = "https://csm.auction/api/skins_base";
        const string csmlib = "https://old.cs.money/730/load_bots_inventory";
        public static Dictionary<string, DeserializeItem> GetCSGOItems()
        {
            HttpClient client = new HttpClient();
            Encoding.GetEncoding("ISO-8859-1");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            var data = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Dictionary<string, DeserializeItem>>(data);
        }
        public static CovertItems[] GetCSMItems()
        {
            HttpClient client = new HttpClient();
            Encoding.GetEncoding("ISO-8859-1");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            var data = client.GetAsync(csmlib).Result.Content.ReadAsStringAsync().Result;
            var output = CovertItems.FromJson(data);
            return output;
        }
        public static List<CSMSkinsInfo> GetSalesHistoryItem(int id)
        {
            List<CSMSkinsInfo> meny_data = new List<CSMSkinsInfo>();
            try
            {
                string url = "https://csm.auction/api/sales?nameId=";
                string customeUrl = url + id;
                HttpClient client = new HttpClient();
                Encoding.GetEncoding("ISO-8859-1");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                var data = client.GetAsync(customeUrl).Result.Content.ReadAsStringAsync().Result;
                var jsonResult = JsonConvert.DeserializeObject(data).ToString();
                var result = JsonConvert.DeserializeObject<DeserializeCSMSkinsSales>(jsonResult);
                foreach (var b in result.sales)
                {
                    meny_data.Add(new CSMSkinsInfo() { name = result.name, custom_price = b.custom_price, floatvalue = b.floatvalue, update_time = UnixTimeStampToDateTime(b.update_time) });
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            return meny_data;
        }
        public static SearchItem FoundItem(string name)
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
        public static SearchItem FoundItem(int id)
        {
            SearchItem output = new SearchItem();
            try
            {
                HttpClient client = new HttpClient();
                Encoding.GetEncoding("ISO-8859-1");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                var data = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                var desitems = JsonConvert.DeserializeObject<Dictionary<string, DeserializeItem>>(data);
                foreach (var found in desitems)
                {
                    if (Convert.ToString(id) == found.Key)
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
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            return output;
        }
        //time converter method(used in fi)
        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.ToUniversalTime();
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }
    //csgo items//
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
    //csm items//
    //----sales----------//
    public class DeserializeCSMSkinsSales
    {
        public string name { get; set; }
        public List<Sale> sales { get; set; }
    }
    public class Sale
    {
        public string custom_price { get; set; }
        public string floatvalue { get; set; }
        public int update_time { get; set; }
        public int user_skin_id { get; set; }
    }
    public class CSMSkinsInfo
    {
        public string name { get; set; }
        public string custom_price { get; set; }
        public string floatvalue { get; set; }
        public DateTime update_time { get; set; }
        public int user_skin_id { get; set; }
    }
    //csm -curently on sales
    public class CSMSkinsOnSale
    {
        //used to check in bot item
        [JsonProperty("id")]
        public List<string> Id { get; set; }
        //id to serch history
        [JsonProperty("o")]
        public long O { get; set; }

        [JsonProperty("g")]
        public long G { get; set; }
        //price
        [JsonProperty("p")]
        public double P { get; set; }

        [JsonProperty("vi", NullValueHandling = NullValueHandling.Ignore)]
        public List<long> Vi { get; set; }

        [JsonProperty("ai", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Ai { get; set; }

        [JsonProperty("bi", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Bi { get; set; }

        [JsonProperty("t", NullValueHandling = NullValueHandling.Ignore)]
        public List<long> T { get; set; }
        //float item
        [JsonProperty("f", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> F { get; set; }

        [JsonProperty("ss", NullValueHandling = NullValueHandling.Ignore)]
        public List<Ss> Ss { get; set; }

        [JsonProperty("ui", NullValueHandling = NullValueHandling.Ignore)]
        public List<long> Ui { get; set; }

        [JsonProperty("cp", NullValueHandling = NullValueHandling.Ignore)]
        public double? Cp { get; set; }

        [JsonProperty("pd", NullValueHandling = NullValueHandling.Ignore)]
        public double? Pd { get; set; }
        //used in 3d view
        [JsonProperty("d", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> D { get; set; }

        [JsonProperty("b", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> B { get; set; }

        [JsonProperty("bl", NullValueHandling = NullValueHandling.Ignore)]
        public List<long> Bl { get; set; }

        [JsonProperty("ar", NullValueHandling = NullValueHandling.Ignore)]
        public List<ArElement> Ar { get; set; }

        [JsonProperty("n", NullValueHandling = NullValueHandling.Ignore)]
        public List<N> N { get; set; }

        [JsonProperty("s", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<Empty>> S { get; set; }

        [JsonProperty("fa", NullValueHandling = NullValueHandling.Ignore)]
        public List<long?> Fa { get; set; }

        [JsonProperty("mf", NullValueHandling = NullValueHandling.Ignore)]
        public List<MfElement> Mf { get; set; }

        [JsonProperty("bs", NullValueHandling = NullValueHandling.Ignore)]
        public List<long?> Bs { get; set; }

        [JsonProperty("ps", NullValueHandling = NullValueHandling.Ignore)]
        public List<long?> Ps { get; set; }

        [JsonProperty("pt", NullValueHandling = NullValueHandling.Ignore)]
        public long? Pt { get; set; }

        [JsonProperty("pop", NullValueHandling = NullValueHandling.Ignore)]
        public long? Pop { get; set; }
    }
    public partial class ArClass
    {
        [JsonProperty("reason")]
        public ArEnum Reason { get; set; }

        [JsonProperty("add_price")]
        public double AddPrice { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }
    public partial class Empty
    {
        [JsonProperty("p", NullValueHandling = NullValueHandling.Ignore)]
        public long? P { get; set; }

        [JsonProperty("s", NullValueHandling = NullValueHandling.Ignore)]
        public double? S { get; set; }

        [JsonProperty("w", NullValueHandling = NullValueHandling.Ignore)]
        public long? W { get; set; }

        [JsonProperty("a", NullValueHandling = NullValueHandling.Ignore)]
        public double? A { get; set; }

        [JsonProperty("o", NullValueHandling = NullValueHandling.Ignore)]
        public long? O { get; set; }

        [JsonProperty("n", NullValueHandling = NullValueHandling.Ignore)]
        public string N { get; set; }

        [JsonProperty("i", NullValueHandling = NullValueHandling.Ignore)]
        public string I { get; set; }
    }
    public enum ArEnum { Float, Pattern, Stickers };
    public enum MfEnum { BlueDominant, BlueTip, FakeRed, Ffi, MaxFake, NotFireAndIce, PissIce, RedTip, The10ThMax, The2NdMax, The3RdMax, The4ThMax, The5ThMax, The6ThMax, The7ThMax, The8ThMax, Tricolor, YellowTip };
    public enum Ss { S };
    public partial struct ArElement
    {
        public ArClass ArClass;
        public ArEnum? Enum;

        public static implicit operator ArElement(ArClass ArClass) => new ArElement { ArClass = ArClass };
        public static implicit operator ArElement(ArEnum Enum) => new ArElement { Enum = Enum };
    }
    public partial struct MfElement
    {
        public MfEnum? Enum;
        public long? Integer;

        public static implicit operator MfElement(MfEnum Enum) => new MfElement { Enum = Enum };
        public static implicit operator MfElement(long Integer) => new MfElement { Integer = Integer };
    }
    public partial struct N
    {
        public long? Integer;
        public string String;

        public static implicit operator N(long Integer) => new N { Integer = Integer };
        public static implicit operator N(string String) => new N { String = String };
        public bool IsNull => Integer == null && String == null;
    }
    public partial class CovertItems
    {
        public static CovertItems[] FromJson(string json) => JsonConvert.DeserializeObject<CovertItems[]>(json, Converter.Settings);
    }
    public static class Serialize
    {
        public static string ToJson(this CovertItems[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ArElementConverter.Singleton,
                ArEnumConverter.Singleton,
                MfElementConverter.Singleton,
                MfEnumConverter.Singleton,
                NConverter.Singleton,
                SsConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
    internal class ArElementConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ArElement) || t == typeof(ArElement?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    switch (stringValue)
                    {
                        case "float":
                            return new ArElement { Enum = ArEnum.Float };
                        case "pattern":
                            return new ArElement { Enum = ArEnum.Pattern };
                        case "stickers":
                            return new ArElement { Enum = ArEnum.Stickers };
                    }
                    break;
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<ArClass>(reader);
                    return new ArElement { ArClass = objectValue };
            }
            throw new Exception("Cannot unmarshal type ArElement");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (ArElement)untypedValue;
            if (value.Enum != null)
            {
                switch (value.Enum)
                {
                    case ArEnum.Float:
                        serializer.Serialize(writer, "float");
                        return;
                    case ArEnum.Pattern:
                        serializer.Serialize(writer, "pattern");
                        return;
                    case ArEnum.Stickers:
                        serializer.Serialize(writer, "stickers");
                        return;
                }
            }
            if (value.ArClass != null)
            {
                serializer.Serialize(writer, value.ArClass);
                return;
            }
            throw new Exception("Cannot marshal type ArElement");
        }

        public static readonly ArElementConverter Singleton = new ArElementConverter();
    }
    internal class ArEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ArEnum) || t == typeof(ArEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "float":
                    return ArEnum.Float;
                case "pattern":
                    return ArEnum.Pattern;
                case "stickers":
                    return ArEnum.Stickers;
            }
            throw new Exception("Cannot unmarshal type ArEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ArEnum)untypedValue;
            switch (value)
            {
                case ArEnum.Float:
                    serializer.Serialize(writer, "float");
                    return;
                case ArEnum.Pattern:
                    serializer.Serialize(writer, "pattern");
                    return;
                case ArEnum.Stickers:
                    serializer.Serialize(writer, "stickers");
                    return;
            }
            throw new Exception("Cannot marshal type ArEnum");
        }

        public static readonly ArEnumConverter Singleton = new ArEnumConverter();
    }
    internal class MfElementConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(MfElement) || t == typeof(MfElement?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new MfElement { Integer = integerValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    switch (stringValue)
                    {
                        case "10th Max":
                            return new MfElement { Enum = MfEnum.The10ThMax };
                        case "2nd Max":
                            return new MfElement { Enum = MfEnum.The2NdMax };
                        case "3rd Max":
                            return new MfElement { Enum = MfEnum.The3RdMax };
                        case "4th Max":
                            return new MfElement { Enum = MfEnum.The4ThMax };
                        case "5th Max":
                            return new MfElement { Enum = MfEnum.The5ThMax };
                        case "6th Max":
                            return new MfElement { Enum = MfEnum.The6ThMax };
                        case "7th Max":
                            return new MfElement { Enum = MfEnum.The7ThMax };
                        case "8th Max":
                            return new MfElement { Enum = MfEnum.The8ThMax };
                        case "Blue Dominant":
                            return new MfElement { Enum = MfEnum.BlueDominant };
                        case "Blue tip":
                            return new MfElement { Enum = MfEnum.BlueTip };
                        case "FFI":
                            return new MfElement { Enum = MfEnum.Ffi };
                        case "Fake Red":
                            return new MfElement { Enum = MfEnum.FakeRed };
                        case "Max Fake":
                            return new MfElement { Enum = MfEnum.MaxFake };
                        case "Not Fire and Ice":
                            return new MfElement { Enum = MfEnum.NotFireAndIce };
                        case "Piss Ice":
                            return new MfElement { Enum = MfEnum.PissIce };
                        case "Red Tip":
                            return new MfElement { Enum = MfEnum.RedTip };
                        case "Tricolor":
                            return new MfElement { Enum = MfEnum.Tricolor };
                        case "Yellow tip":
                            return new MfElement { Enum = MfEnum.YellowTip };
                    }
                    break;
            }
            throw new Exception("Cannot unmarshal type MfElement");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (MfElement)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.Enum != null)
            {
                switch (value.Enum)
                {
                    case MfEnum.The10ThMax:
                        serializer.Serialize(writer, "10th Max");
                        return;
                    case MfEnum.The2NdMax:
                        serializer.Serialize(writer, "2nd Max");
                        return;
                    case MfEnum.The3RdMax:
                        serializer.Serialize(writer, "3rd Max");
                        return;
                    case MfEnum.The4ThMax:
                        serializer.Serialize(writer, "4th Max");
                        return;
                    case MfEnum.The5ThMax:
                        serializer.Serialize(writer, "5th Max");
                        return;
                    case MfEnum.The6ThMax:
                        serializer.Serialize(writer, "6th Max");
                        return;
                    case MfEnum.The7ThMax:
                        serializer.Serialize(writer, "7th Max");
                        return;
                    case MfEnum.The8ThMax:
                        serializer.Serialize(writer, "8th Max");
                        return;
                    case MfEnum.BlueDominant:
                        serializer.Serialize(writer, "Blue Dominant");
                        return;
                    case MfEnum.BlueTip:
                        serializer.Serialize(writer, "Blue tip");
                        return;
                    case MfEnum.Ffi:
                        serializer.Serialize(writer, "FFI");
                        return;
                    case MfEnum.FakeRed:
                        serializer.Serialize(writer, "Fake Red");
                        return;
                    case MfEnum.MaxFake:
                        serializer.Serialize(writer, "Max Fake");
                        return;
                    case MfEnum.NotFireAndIce:
                        serializer.Serialize(writer, "Not Fire and Ice");
                        return;
                    case MfEnum.PissIce:
                        serializer.Serialize(writer, "Piss Ice");
                        return;
                    case MfEnum.RedTip:
                        serializer.Serialize(writer, "Red Tip");
                        return;
                    case MfEnum.Tricolor:
                        serializer.Serialize(writer, "Tricolor");
                        return;
                    case MfEnum.YellowTip:
                        serializer.Serialize(writer, "Yellow tip");
                        return;
                }
            }
            throw new Exception("Cannot marshal type MfElement");
        }

        public static readonly MfElementConverter Singleton = new MfElementConverter();
    }
    internal class MfEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(MfEnum) || t == typeof(MfEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "10th Max":
                    return MfEnum.The10ThMax;
                case "2nd Max":
                    return MfEnum.The2NdMax;
                case "3rd Max":
                    return MfEnum.The3RdMax;
                case "4th Max":
                    return MfEnum.The4ThMax;
                case "5th Max":
                    return MfEnum.The5ThMax;
                case "6th Max":
                    return MfEnum.The6ThMax;
                case "7th Max":
                    return MfEnum.The7ThMax;
                case "8th Max":
                    return MfEnum.The8ThMax;
                case "Blue Dominant":
                    return MfEnum.BlueDominant;
                case "Blue tip":
                    return MfEnum.BlueTip;
                case "FFI":
                    return MfEnum.Ffi;
                case "Fake Red":
                    return MfEnum.FakeRed;
                case "Max Fake":
                    return MfEnum.MaxFake;
                case "Not Fire and Ice":
                    return MfEnum.NotFireAndIce;
                case "Piss Ice":
                    return MfEnum.PissIce;
                case "Red Tip":
                    return MfEnum.RedTip;
                case "Tricolor":
                    return MfEnum.Tricolor;
                case "Yellow tip":
                    return MfEnum.YellowTip;
            }
            throw new Exception("Cannot unmarshal type MfEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (MfEnum)untypedValue;
            switch (value)
            {
                case MfEnum.The10ThMax:
                    serializer.Serialize(writer, "10th Max");
                    return;
                case MfEnum.The2NdMax:
                    serializer.Serialize(writer, "2nd Max");
                    return;
                case MfEnum.The3RdMax:
                    serializer.Serialize(writer, "3rd Max");
                    return;
                case MfEnum.The4ThMax:
                    serializer.Serialize(writer, "4th Max");
                    return;
                case MfEnum.The5ThMax:
                    serializer.Serialize(writer, "5th Max");
                    return;
                case MfEnum.The6ThMax:
                    serializer.Serialize(writer, "6th Max");
                    return;
                case MfEnum.The7ThMax:
                    serializer.Serialize(writer, "7th Max");
                    return;
                case MfEnum.The8ThMax:
                    serializer.Serialize(writer, "8th Max");
                    return;
                case MfEnum.BlueDominant:
                    serializer.Serialize(writer, "Blue Dominant");
                    return;
                case MfEnum.BlueTip:
                    serializer.Serialize(writer, "Blue tip");
                    return;
                case MfEnum.Ffi:
                    serializer.Serialize(writer, "FFI");
                    return;
                case MfEnum.FakeRed:
                    serializer.Serialize(writer, "Fake Red");
                    return;
                case MfEnum.MaxFake:
                    serializer.Serialize(writer, "Max Fake");
                    return;
                case MfEnum.NotFireAndIce:
                    serializer.Serialize(writer, "Not Fire and Ice");
                    return;
                case MfEnum.PissIce:
                    serializer.Serialize(writer, "Piss Ice");
                    return;
                case MfEnum.RedTip:
                    serializer.Serialize(writer, "Red Tip");
                    return;
                case MfEnum.Tricolor:
                    serializer.Serialize(writer, "Tricolor");
                    return;
                case MfEnum.YellowTip:
                    serializer.Serialize(writer, "Yellow tip");
                    return;
            }
            throw new Exception("Cannot marshal type MfEnum");
        }

        public static readonly MfEnumConverter Singleton = new MfEnumConverter();
    }
    internal class NConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(N) || t == typeof(N?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return new N { };
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new N { Integer = integerValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new N { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type N");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (N)untypedValue;
            if (value.IsNull)
            {
                serializer.Serialize(writer, null);
                return;
            }
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            throw new Exception("Cannot marshal type N");
        }

        public static readonly NConverter Singleton = new NConverter();
    }
    internal class SsConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Ss) || t == typeof(Ss?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "s")
            {
                return Ss.S;
            }
            throw new Exception("Cannot unmarshal type Ss");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Ss)untypedValue;
            if (value == Ss.S)
            {
                serializer.Serialize(writer, "s");
                return;
            }
            throw new Exception("Cannot marshal type Ss");
        }

        public static readonly SsConverter Singleton = new SsConverter();
    }
    internal class DecodeArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long[]);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            var value = new List<long>();
            while (reader.TokenType != JsonToken.EndArray)
            {
                var converter = ParseStringConverter.Singleton;
                var arrayItem = (long)converter.ReadJson(reader, typeof(long), null, serializer);
                value.Add(arrayItem);
                reader.Read();
            }
            return value.ToArray();
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (long[])untypedValue;
            writer.WriteStartArray();
            foreach (var arrayItem in value)
            {
                var converter = ParseStringConverter.Singleton;
                converter.WriteJson(writer, arrayItem, serializer);
            }
            writer.WriteEndArray();
            return;
        }

        public static readonly DecodeArrayConverter Singleton = new DecodeArrayConverter();
    }
    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
