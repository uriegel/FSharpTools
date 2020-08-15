namespace FSharpTools
module ActivePatterns = 
    open System

    let (|Int|_|) (str: string option) =
        match str with
        | Some str -> 
            match System.Int32.TryParse str with
            | true,int -> Some int
            | _ -> None
        | _ -> None        

    // TODO: UwebFiles
    let (|Long|_|) (str: string option) =
        match str with
        | Some str -> 
            match System.Int64.TryParse str with
            | true,int -> Some int
            | _ -> None
        | _ -> None        
