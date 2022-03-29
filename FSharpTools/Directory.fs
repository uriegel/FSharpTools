namespace FSharpTools
module Directory = 

    open System
    open System.IO

    open Functional

    /// <summary>
    /// </summary>
    /// <param name="pathes">Array of path parts to combine</param>
    /// <returns>Combined path</returns>
    let combinePathes pathes = Path.Combine pathes

    /// <summary>
    /// Creates a directory
    /// </summary>
    /// <param name="path">Path to directory to create</param>
    /// <returns>Result of DirectoryInfo or Exception if it fails</returns>
    let create path = 
        try
            Ok (Directory.CreateDirectory path)
        with
        | e -> Error(e)

    /// <summary>
    /// Retrieves a directory usable for saving configuration.
    /// You can specify a 'scheme' path part and an 'application path part
    /// </summary>
    /// <param name="scheme">Path part representing the scheme</param>
    /// <param name="application">Path part representing the application</param>
    /// <returns>Path of the configuration directory</returns>
    let retrieveConfigDirectory scheme application = 
        [| 
            Environment.GetFolderPath Environment.SpecialFolder.ApplicationData
            scheme
            application
        |] |> combinePathes 

