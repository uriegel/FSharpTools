namespace FSharpTools
module TextJson =
    open System.Text.Json

    let serialize (options: JsonSerializerOptions) obj = 
        JsonSerializer.Serialize (obj, options)    
