module UrlQueryComponents
open System
open System.Text.RegularExpressions

let private urlParameterRegex = Regex (@"(?<key>[^&?]*?)=(?<value>[^&?]*)", RegexOptions.Compiled)

type Query = 
    {
        Method: string
        RawParameters: Map<string,string>
    }
    member this.Query key = this.RawParameters.TryFind key

let create (url: string) = 
    if url.Contains "?" then
        let urlParameterString = Uri.UnescapeDataString url
        let matches = urlParameterRegex.Matches urlParameterString
        
        let unescapeSpaces (uri: string) = uri.Replace ('+', ' ')
        let methodPath = url.Substring (0, url.IndexOf('?'))
        let pos = methodPath.LastIndexOf '/' + 1 
        {
            Method = methodPath.Substring pos
            RawParameters = matches 
                            |> Seq.cast 
                            |> Seq.map (fun (s: Match) -> (s.Groups.["key"].Value, Uri.UnescapeDataString (unescapeSpaces s.Groups.["value"].Value))) 
                            |> Map.ofSeq
        }
    else 
        let pos = url.LastIndexOf '/' + 1 
        {
            Method = url.Substring (pos)
            RawParameters = Map.empty
        }
