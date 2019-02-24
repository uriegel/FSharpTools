module Json

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




