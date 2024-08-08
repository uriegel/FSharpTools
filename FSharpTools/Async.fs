namespace FSharpTools
open System.Threading

module Async = 
    /// <summary>
    /// Runs the async computation and continues in the given SynchronizationContext 
    /// </summary>
    let StartWithContext (ctx: SynchronizationContext) task = 
        ctx.Send((fun _ -> Async.StartImmediate(task)), null)

    /// <summary>
    /// Runs the async computation and continues in the current SynchronizationContext 
    /// </summary>
    let StartWithCurrentContext = StartWithContext SynchronizationContext.Current

//     /// <summary>
//     /// Function Compostion for 2 async functions
//     /// </summary>
//     /// <returns>function with input parameter 'a returning asyc result 'c</returns>
//     let (>>) f g x = async {
//         let! y = f x
//         let! e = g y
//         return e
//     }

    // /// <summary>
    // /// Anamorph function lifting 'a to Async&lt;'a&gt;
    // /// </summary>
    // /// <param name="a">value to lift</param>
    // /// <returns>returns lifted value Async&lt;'a&gt;</returns>
    // let toAsync a = async {
    //     return a
    // }

//     /// <summary>
//     /// Helper function for composing functions (Railway Oriented Programming). 
//     /// Slot for dead end function (Inject side effects in function coposition pipeline)
//     /// </summary>
//     /// <param name="f">Asynchronous function with one input parameter 'a. This is the dead end function</param>
//     /// <param name="x">input parameter 'a</param>
//     /// <returns>'a</returns>
//     let sideEffect f x = async {
//         let! a = x
//         do! f a
//         return a
//     }

