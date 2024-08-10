namespace FSharpTools
open System.Text.Json
open System.Text.Json.Serialization

module TextJson =
    
    let Default = JsonSerializerOptions(
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)

    let serializeWithOptions<'a> (options: JsonSerializerOptions) (a: 'a) = 
        JsonSerializer.Serialize (a, options)    

    let deserializeWithOptions<'a> (options: JsonSerializerOptions) (str: string) = 
        JsonSerializer.Deserialize<'a> (str, options)    

    let serialize<'a> = serializeWithOptions<'a> Default
    let deserialize<'a> = deserializeWithOptions<'a> Default
