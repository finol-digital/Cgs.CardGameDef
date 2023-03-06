namespace FinolDigital.Cgs.CardGameDef
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PropertyType
    {
        [EnumMember(Value = "string")] String,
        [EnumMember(Value = "escapedString")] EscapedString,
        [EnumMember(Value = "integer")] Integer,
        [EnumMember(Value = "boolean")] Boolean,
        [EnumMember(Value = "object")] Object,
        [EnumMember(Value = "stringEnum")] StringEnum,
        [EnumMember(Value = "stringList")] StringList,
        [EnumMember(Value = "stringEnumList")] StringEnumList,
        [EnumMember(Value = "objectEnum")] ObjectEnum,
        [EnumMember(Value = "objectList")] ObjectList,
        [EnumMember(Value = "objectEnumList")] ObjectEnumList
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class PropertyDef : ICloneable
    {
        public const string ObjectDelimiter = ".";
        public const string EscapeCharacter = "\\";

        [JsonProperty]
        [JsonRequired]
        [Description("The name of the property: This name can be referenced to lookup a Card's property")]
        public string Name { get; set; }

        [JsonProperty]
        [JsonRequired]
        [Description("The type of the property")]
        [DefaultValue("string")]
        public PropertyType Type { get; set; }

        [JsonProperty]
        [Description("The name of the property as it is displayed to the end user")]
        public string Display { get; set; }

        [JsonProperty]
        [Description("The value to display if the value is null or empty")]
        public string DisplayEmpty { get; set; }

        [JsonProperty]
        [Description("List <displayEmpty> as the first option if this property is an enum?")]
        public bool DisplayEmptyFirst { get; set; }

        [JsonProperty] public List<PropertyDef> Properties { get; set; }

        [JsonProperty]
        [Description(
            "If this property is a stringList or stringEnumList, the value will be delimited by this delimiter")]
        public string Delimiter { get; set; }

        [JsonConstructor]
        public PropertyDef(string name, PropertyType type, string display = "", string displayEmpty = "",
            bool displayEmptyFirst = false, List<PropertyDef>? properties = null, string delimiter = "")
        {
            Name = name ?? string.Empty;
            int objectDelimiterIdx = Name.IndexOf(ObjectDelimiter, StringComparison.Ordinal);
            if (objectDelimiterIdx != -1)
                Name = Name.Substring(0, objectDelimiterIdx);
            Type = objectDelimiterIdx != -1 ? PropertyType.Object : type;
            Display = display ?? string.Empty;
            DisplayEmpty = displayEmpty ?? string.Empty;
            DisplayEmptyFirst = displayEmptyFirst;
            Properties = properties != null ? new List<PropertyDef>(properties) : new List<PropertyDef>();
            if (objectDelimiterIdx != -1)
            {
                if (type == PropertyType.Object || type == PropertyType.ObjectEnum ||
                    type == PropertyType.ObjectEnumList || type == PropertyType.ObjectList)
                    Properties.Clear();
                Properties.Add(new PropertyDef(name == null ? string.Empty : name[(objectDelimiterIdx + 1)..],
                                               Type,
                                               Display,
                                               DisplayEmpty,
                                               DisplayEmptyFirst,
                                               Properties));
            }

            Delimiter = delimiter;
        }

        public object Clone()
        {
            var propertyDef = new PropertyDef(Name, Type, Display, DisplayEmpty, DisplayEmptyFirst, Properties);
            return propertyDef;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class PropertyDefValuePair : ICloneable
    {
        public PropertyDef? Def { get; set; }

        public string Value { get; set; } = string.Empty;

        public object Clone()
        {
            var propertyDefValuePair = new PropertyDefValuePair()
            {
                Def = Def?.Clone() as PropertyDef,
                Value = (string)Value.Clone()
            };
            return propertyDefValuePair;
        }
    }
}
