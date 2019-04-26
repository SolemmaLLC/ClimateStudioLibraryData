using ArchsimLib.Utilities;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace ArchsimLib.LibraryObjects
{

    public class Units : Attribute
    {
        public Units(string _unit) {
            Unit = _unit;
        }
        public string Unit { get; set; }
    }


    [DataContract]
    public abstract class LibraryComponent
   {
        [DataMember, DefaultValue("No name")]
        public string Name { get; set; } = "No name";

        [DataMember, DefaultValue("No Category")]
        public string Category { get; set; } = "No Category";

        [DataMember, DefaultValue("No comments")]
        public string Comment { get; set; } = "No comments";

        [DataMember, DefaultValue("No data source")]
        public string DataSource { get; set; } = "No data source";

        [OnDeserializing]
        public void OnDeserializing(StreamingContext context)
        {
            foreach (PropertyInfo f in this.GetType().GetProperties())
            {
                foreach (Attribute attr in f.GetCustomAttributes(true))
                {
                    if (attr is DefaultValueAttribute)
                    {
                        DefaultValueAttribute dv = (DefaultValueAttribute)attr;
                        f.SetValue(this, dv.Value, null);
                    }
                }
            }
        }

        public string Serialize() { return Serialization.Serialize(this); }

        public static T Deserialize<T>(string context)
        {
            return Serialization.Deserialize<T>(context);
        }

        public override string ToString()
        {
            return  Name;
        }

    }
}
