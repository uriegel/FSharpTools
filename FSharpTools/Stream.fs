namespace FSharpTools
module Stream = 

    open System
    open System.IO
    open Result

    /// <summary>
    /// Creates a file stream
    /// </summary>
    /// <param name="path">Path to the file stream to create</param>
    /// <returns>Result of Stream or Exception if it fails</returns>
    let create path = 
        let fileCreate () = File.Create path :> IO.Stream
        catch fileCreate

    /// <summary>
    /// Opens a file stream in read mode
    /// </summary>
    /// <param name="path">Path to the file stream to open</param>
    /// <returns>Result of Stream or Exception if it fails</returns>
    let openRead path = 
        let fileOpen () = File.OpenRead path :> IO.Stream
        catch fileOpen

// TODO StreamReader
    // let withStreamReader (stream: Stream) =
    //     new StreamReader(stream)

    // let readToEnd (sr: StreamReader) =
    //     sr.ReadToEnd ()
