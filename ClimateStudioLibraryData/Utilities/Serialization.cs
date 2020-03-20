using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSEnergyLib.Utilities
{



    public static class Serialization
    {

        class FlatDoubleArrJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(double[]) || objectType == typeof(List<double>);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteRawValue(JsonConvert.SerializeObject(value, Formatting.None));
            }
        }



        public static T Deserialize<T>(string json)
        {
            return DeserializeJsonNet<T>(json);
        }
        public static string Serialize<T>(T component)
        {
            return SerializeJsonNet<T>(component);
        }



        //private static T DeserializeXml<T>(string xml)
        //{
        //    var serializer = new DataContractSerializer(typeof(T));
        //    return (T)serializer.ReadObject(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml)));
        //}

        //private static string SerializeXml<T>(T component)
        //{
        //    var dcs = new DataContractSerializer(typeof(T));
        //    using (var mem = new MemoryStream())
        //    using (var reader = new StreamReader(mem))
        //    {
        //        dcs.WriteObject(mem, component);
        //        mem.Position = 0;
        //        return reader.ReadToEnd();
        //    }
        //}




        //private static T DeserializeJson<T>(string json)
        //{
        //    var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
        //    return (T)serializer.ReadObject(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)));
        //}

        //private static string SerializeJson<T>(T component)
        //{
        //    var dcs = new DataContractJsonSerializer(typeof(T));
        //    using (var mem = new MemoryStream())
        //    using (var reader = new StreamReader(mem))
        //    {
        //        dcs.WriteObject(mem, component);
        //        mem.Position = 0;
        //        return reader.ReadToEnd();
        //    }
        //}


        private static T DeserializeJsonNet<T>(string json)
        {
            var set = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            };


            return JsonConvert.DeserializeObject<T>(json, set);

        }

        private static string SerializeJsonNet<T>(T component)
        {

            var set = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            };
            set.Converters.Add(new FlatDoubleArrJsonConverter());



            return JsonConvert.SerializeObject(component, set);


        }



    }
}
