namespace FSharpTools

module Async = 

    /// <summary>
    /// Function Compostion for 2 async functions
    /// </summary>
    /// <returns>function with input parameter 'a returning asyc result 'c</returns>
    let (>>) f g x = async {
        let! y = f x
        let! e = g y
        return e
    }

    /// <summary>
    /// Anamorph function lifting 'a to Async&lt;'a&gt;
    /// </summary>
    /// <param name="a">value to lift</param>
    /// <returns>returns lifted value Async&lt;'a&gt;</returns>
    let toAsync a = async {
        return a
    }
