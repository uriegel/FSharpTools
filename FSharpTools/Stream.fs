namespace FSharpTools
module Stream = 

    open System
    open System.IO

    /// <summary>
    /// Creates a file stream
    /// </summary>
    /// <param name="path">Path to the file stream to create</param>
    /// <returns>Result of Stream or Exception if it fails</returns>
    let create path = 
        try 
            Ok (File.Create (path) :> IO.Stream)
        with
        | e -> Error e

    /// <summary>
    /// Opens a file stream in read mode
    /// </summary>
    /// <param name="path">Path to the file stream to open</param>
    /// <returns>Result of Stream or Exception if it fails</returns>
    let openRead path = 
        try 
            Ok (File.OpenRead (path) :> IO.Stream)
        with
        | e -> Error e
