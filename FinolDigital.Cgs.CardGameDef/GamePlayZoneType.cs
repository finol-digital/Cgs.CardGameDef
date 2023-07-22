﻿namespace FinolDigital.Cgs.CardGameDef
{
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum GamePlayZoneType
    {
        [EnumMember(Value = "area")] Area,
        [EnumMember(Value = "horizontal")] Horizontal,
        [EnumMember(Value = "vertical")] Vertical
    }
}