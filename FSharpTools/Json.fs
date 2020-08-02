module Json

open System

open Microsoft.FSharp.Reflection

open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open System.IO
open Newtonsoft.Json.Linq

let private defaultSettings = JsonSerializerSettings(ContractResolver = CamelCasePropertyNamesContractResolver(),
                                                //defaultSettings.TypeNameHandling = TypeNameHandling.All
                                                DefaultValueHandling = DefaultValueHandling.Ignore)
      
/// <summary>Serializing an object to JSON
/// Default value handling is no to emit default values
/// </summary>
/// <param name="obj">The object to serialize.</param>
/// <returns>JSON serialized object as string.</returns>
let serialize obj = JsonConvert.SerializeObject (obj, defaultSettings)
let deserialize<'a> str = JsonConvert.DeserializeObject<'a> (str, defaultSettings)

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
        if isNull value then 
            FSharpValue.MakeUnion(cases.[0], [||])
        else 
            FSharpValue.MakeUnion(cases.[1], [|value|])

let private defaultSettingsWithOptions =
    JsonSerializerSettings(ContractResolver = CamelCasePropertyNamesContractResolver(),
                                Converters = [| OptionConverter() |], 
                                DefaultValueHandling = DefaultValueHandling.Ignore)

/// <summary>Serializing an object to JSON
/// Options are supported
/// Default value handling is no to emit default values
/// </summary>
/// <param name="obj">The object to serialize.</param>
/// <returns>JSON serialized object as string.</returns>
let serializeWithOptions obj = JsonConvert.SerializeObject (obj, defaultSettingsWithOptions)
let deserializeWithOptions<'a> str = JsonConvert.DeserializeObject<'a> (str, defaultSettingsWithOptions)

let private serializeStreamImpl withOptions (stream: Stream) a =
    let writer = new StreamWriter (stream)
    let jsonWriter = new JsonTextWriter (writer)
    let ser = JsonSerializer ()
    ser.ContractResolver <- CamelCasePropertyNamesContractResolver ()
    ser.DefaultValueHandling <- DefaultValueHandling.Ignore
    if withOptions then 
        ser.Converters.Add (OptionConverter ())
    ser.Serialize (jsonWriter, a)
    jsonWriter.Flush()

let private deserializeStreamImpl<'T> withOptions (stream: Stream) =
    let reader = new StreamReader (stream)
    let jsonReader = new JsonTextReader (reader)
    let ser = JsonSerializer ()
    ser.DefaultValueHandling <- DefaultValueHandling.Ignore
    ser.ContractResolver <- CamelCasePropertyNamesContractResolver ()
    if withOptions then 
        ser.Converters.Add (OptionConverter ())
    ser.Deserialize<'T> (jsonReader)

/// <summary>Serializing an object to JSON stream
/// Default value handling is no to emit default values
/// </summary>
/// <param name="stream">The stream to contain the result.</param>
/// <param name="obj">The object to serialize.</param>
let serializeStream<'T> = serializeStreamImpl false
/// <summary>Serializing an object to JSON stream
/// Options are supported
/// Default value handling is no to emit default values
/// </summary>
/// <param name="stream">The stream to contain the result.</param>
/// <param name="obj">The object to serialize.</param>
let serializeStreamWithOptions<'T> = serializeStreamImpl true
/// <summary>Deserializing an object from JSON stream
/// Default value handling is no to emit default values
/// </summary>
/// <param name="stream">The stream to contain the result.</param>
/// <returns>Deserialized object.</returns>
let deserializeStream<'T> = deserializeStreamImpl<'T> false
/// <summary>Deserializing an object from JSON stream
/// Options are supported
/// Default value handling is no to emit default values
/// </summary>
/// <param name="stream">The stream to contain the result.</param>
/// <returns>Deserialized object.</returns>
let deserializeStreamWithOptions<'T> = deserializeStreamImpl<'T> true

let serializeToBuffer a =     
    use ms = new MemoryStream ()
    serializeStream ms a
    ms.Capacity <- int ms.Length
    ms.GetBuffer ()

let serializeWithOptionsToBuffer a =     
    use ms = new System.IO.MemoryStream ()
    serializeStreamWithOptions ms a
    ms.Capacity <- int ms.Length
    ms.GetBuffer ()

let get<'a> (json: JObject) key =
    let token = json.[key]      
    if isNull token then
        None
    else
        Some (token.Value<'a> ())

let getDef<'a> (json: JObject) key (defaultValue: 'a) =
    match get json key with
    | Some v -> v
    | None -> defaultValue
