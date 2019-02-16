module Json

open Newtonsoft.Json
open Newtonsoft.Json.Serialization

let private defaultSettings = new JsonSerializerSettings(ContractResolver = CamelCasePropertyNamesContractResolver(),
                                                //defaultSettings.TypeNameHandling = TypeNameHandling.All
                                                DefaultValueHandling = DefaultValueHandling.Ignore)
                                                
let serialize obj = JsonConvert.SerializeObject (obj, defaultSettings)




