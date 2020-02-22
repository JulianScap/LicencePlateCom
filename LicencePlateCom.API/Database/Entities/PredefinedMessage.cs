using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LicencePlateCom.API.Database.Entities
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PredefinedMessage
    {
        None = 0,

        BrokenTailLight,
        FlatTire,
        LowTirePressure,
        DirtyPlate,
        WeirdNoise
    }
}