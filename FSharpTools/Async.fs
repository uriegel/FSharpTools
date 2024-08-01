namespace FSharpTools

// module Async = 

//     /// <summary>
//     /// Function Compostion for 2 async functions
//     /// </summary>
//     /// <returns>function with input parameter 'a returning asyc result 'c</returns>
//     let (>>) f g x = async {
//         let! y = f x
//         let! e = g y
//         return e
//     }

//     /// <summary>
//     /// Anamorph function lifting 'a to Async&lt;'a&gt;
//     /// </summary>
//     /// <param name="a">value to lift</param>
//     /// <returns>returns lifted value Async&lt;'a&gt;</returns>
//     let toAsync a = async {
//         return a
//     }

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

//     /// <summary>
//     /// Maps async value by calling function f
//     /// </summary>
//     /// <param name="f">function with one input parameter 'a returning 'b</param>
//     /// <param name="x">input parameter Async&lt;'a&gt;</param>
//     /// <returns>Async&lt;'b&gt;</returns>
//     let map f x = async {
//         let! v = x
//         return f v
//     }

//     /// <summary>
//     /// Binds async value by calling function f
//     /// </summary>
//     /// <param name="f">function with one input parameter 'a returning an Async&lt;'b&gt;</param>
//     /// <param name="x">input parameter 'a</param>
//     /// <returns>Async&lt;'b&gt;</returns>
//     let bind f x = async {
//         let! v = x
//         return!f v
//     }

//     /// <summary>
//     /// Map operator for composing functions returning Async values (Railway Oriented Programming).
//     /// </summary>
//     /// <param name="f">function with one input parameter 'a returning an Async&lt;'b&gt;</param>
//     /// <param name="x">input parameter 'a</param>
//     /// <returns>Async&lt;'b&gt;</returns>
//     let (|>>) x f = map f x
    
//     /// <summary>
//     /// Bind operator for composing functions returning Async values (Railway Oriented Programming).
//     /// </summary>
//     /// <param name="binder">function with one input parameter 'a returning an Async&lt;'b&gt;</param>
//     /// <param name="x">input parameter 'a</param>
//     /// <returns>Async&lt;'b&gt;</returns>
//     let (>>=) x binder = bind binder x

//     /// <summary>
//     /// Fish operator (Kleisli Category) for composing functions returning async values (Railway Oriented Programming).
//     /// </summary>
//     /// <param name="f1">function with one input parameter 'a returning an Async&lt;'b&gt;</param>
//     /// <param name="f2">function with one input parameter 'b returning an Async&lt;'c&gt;</param>
//     /// <param name="x">input parameter 'a</param>
//     /// <returns>function with one input parameter 'a returning an Async&lt;'c&gt;</returns>
//     let (>=>) f1 f2 x = f1 x >>= f2
