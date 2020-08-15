namespace FSharpTools
module Dictionary = 

    open System.Collections.Generic

    /// <summary>
    /// Converts a sequence of KeyValuePairs to a Dictionary.
    /// The key uses an IEqualityComparer
    /// </summary>
    /// <param name="equalityComparer">EqualityComparer used for comparing keys</param>
    /// <param name="keyValuePairs">Sequence of KeyValuePairs to convert to dictionary</param>
    /// <returns>The created Dictionary</returns>
    let Dict<'k,'v> (equalityComparer: IEqualityComparer<'k>) (keyValuePairs: seq<'k*'v>) = 
        let result = Dictionary<'k,'v>(equalityComparer)
        keyValuePairs
        |> Seq.iter (fun (k, v) -> result.[k] <- v)
        result :> IDictionary<'k,'v>





