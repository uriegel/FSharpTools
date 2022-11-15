namespace FSharpTools

module Async = 

    /// <summary>
    /// Function Compostion for 2 async functions
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning asyc result 'b</param>
    /// <param name="f">function with one input parameter 'b returning asyc result 'c</param>
    /// <returns>f">function with input parameter 'a returning asyc result 'c</returns>
    let (>>) f g x = async {
        let! y = f x
        let! e = g y
        return e
    }