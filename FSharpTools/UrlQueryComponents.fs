module UrlQueryComponents
open System
open System.Text.RegularExpressions

let private urlParameterRegex = Regex (@"(?<key>[^&?]*?)=(?<value>[^&?]*)", RegexOptions.Compiled)

type Query = 
    {
        Request: string
        Path: string option
        RawParameters: Map<string,string>
    }
    member this.Query key = this.RawParameters.TryFind key

let create url = 
    let getPathParts url = url |> String.splitChar '/'

    let getPath (parts: string array) =
        let res = parts.[3..parts.Length - 2] |> String.join '/' 
        match res with
        | "" -> None
        | _ -> Some res

    if  url |> String.contains "?" then
        let urlParameterString = Uri.UnescapeDataString url
        let matches = urlParameterRegex.Matches urlParameterString
        let unescapeSpaces (uri: string) = uri.Replace ('+', ' ')
        let urlWithoutQuery = url |> String.substring2 0 (url |> String.lastIndexOfChar '?').Value
        let pathParts = urlWithoutQuery |> getPathParts
        {
            Request = if pathParts.Length > 3 then pathParts.[pathParts.Length - 1] else ""
            Path = getPath pathParts
            RawParameters = matches 
                            |> Seq.cast 
                            |> Seq.map (fun (s: Match) -> (s.Groups.["key"].Value, Uri.UnescapeDataString (unescapeSpaces s.Groups.["value"].Value))) 
                            |> Map.ofSeq
        }
    else 
        let pathParts = url |> getPathParts
        {
            Request = if pathParts.Length > 3 then pathParts.[pathParts.Length - 1] else ""
            Path = getPath pathParts
            RawParameters = Map.empty
        }
