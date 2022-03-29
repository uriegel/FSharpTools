namespace FSharpTools
module Functional = 
    /// <summary>
    /// Memoization. A function result is memoized if called for the first time. 
    /// Subsequent calls always returns return the memoized value, not calling the function any more.
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning 'b</param>
    /// <returns>Memoized function with the same signature</returns>
    let memoize func =
        let memoization = System.Collections.Generic.Dictionary<_, _>()
        fun key ->
            match memoization.TryGetValue key with
            | true, value -> value
            | _           -> 
                             let value = func key  
                             memoization.Add(key, value)
                             value

    /// <summary>
    /// Memoization. A function result is memoized if called for the first time. 
    /// Subsequent calls always returns return the memoized value, not calling the function any more.
    /// </summary>
    /// <param name="f">function with no (unit) input parameter 'a returning 'b</param>
    /// <returns>Memoized function with the same signature</returns>
    let memoizeSingle funToMemoize =
        let memoized = funToMemoize ()
        fun () -> memoized

    