namespace FSharpTools
open System
open System.Text.Json
open System.Text.Json.Serialization

module TextJson =
    open System.IO
    
    let Default = JsonSerializerOptions(
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)

    let serializeWithOptions<'a> (options: JsonSerializerOptions) (a: 'a) = 
        JsonSerializer.Serialize (a, options)    

    let deserializeWithOptions<'a> (options: JsonSerializerOptions) (str: string) = 
        let json = if String.IsNullOrEmpty str = false then str else "{}"
        JsonSerializer.Deserialize<'a> (json, options)    

    let serialize<'a> = serializeWithOptions<'a> Default
    let deserialize<'a> = deserializeWithOptions<'a> Default

    let deserializeStream<'a> (s: Stream) =
        JsonSerializer.Deserialize<'a>(s, Default)

    

