using System.Text.Json.Serialization;

namespace PowerFile.CommandLine.Config.Serialization;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(PowerFileConfiguration))]
internal partial class PowerFileConfigurationSerializerContext : JsonSerializerContext { }