module Json

open System

open Microsoft.FSharp.Reflection

open Newtonsoft.Json
open Newtonsoft.Json.Serialization

let private defaultSettings = new JsonSerializerSettings(ContractResolver = CamelCasePropertyNamesContractResolver(),
                                                //defaultSettings.TypeNameHandling = TypeNameHandling.All
                                                DefaultValueHandling = DefaultValueHandling.Ignore)
      
/// <summary>Serializing an object to JSON
/// Default value handling is no to emit default values
/// </summary>
/// <param name="obj">The object to serialize.</param>
/// <returns>JSON serialized object as string.</returns>
let serialize obj = JsonConvert.SerializeObject (obj, defaultSettings)

type OptionConverter() =
    inherit JsonConverter()
    
    override x.CanConvert(objectType) = 
        objectType.IsGenericType && objectType.GetGenericTypeDefinition() = typedefof<option<_>>

    override x.WriteJson(writer, value, serializer) =
        let value = 
            match value with 
            | null -> null
            | _ -> 
                let _, fields = FSharpValue.GetUnionFields(value, value.GetType())
                fields.[0]  
        serializer.Serialize(writer, value)

    override x.ReadJson(reader, objectType, existingValue, serializer) =        
        let innerType = objectType.GetGenericArguments().[0]
        let innerType = 
            if innerType.IsValueType then (typedefof<Nullable<_>>).MakeGenericType([|innerType|])
            else innerType        
        let value = serializer.Deserialize(reader, innerType)
        let cases = FSharpType.GetUnionCases(objectType)
        if value = null then 
            FSharpValue.MakeUnion(cases.[0], [||])
        else 
            FSharpValue.MakeUnion(cases.[1], [|value|])

let private defaultSettingsWithOptions =
    new JsonSerializerSettings(ContractResolver = CamelCasePropertyNamesContractResolver(),
                                Converters = [| OptionConverter() |], 
                                DefaultValueHandling = DefaultValueHandling.Ignore)

/// <summary>Serializing an object to JSON
/// Options are supported
/// Default value handling is no to emit default values
/// </summary>
/// <param name="obj">The object to serialize.</param>
/// <returns>JSON serialized object as string.</returns>
let serializeWithOptions obj = JsonConvert.SerializeObject (obj, defaultSettingsWithOptions)



