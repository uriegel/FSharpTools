namespace FSharpTools
module Directory = 

    open System
    open System.IO

    open Result

    /// <summary>
    /// </summary>
    /// <param name="pathes">Array of path parts to combine</param>
    /// <returns>Combined path</returns>
    let combinePathes pathes = Path.Combine pathes

    /// <summary>
    /// Combines two pathes, for Railway oriented programming and partial application
    /// </summary>
    /// <param name="path">First part of the path</param>
    /// <param name="subpath">Second part of the path</param> 
    /// <returns>Combined path</returns>
    let combine2Pathes path subPath = [| path; subPath |] |> combinePathes

    /// <summary>
    /// Combines two pathes, for Railway oriented programming and partial application
    /// </summary>
    /// <param name="subpath">Second part of the path</param>
    /// <param name="path">First part of the path</param> 
    /// <returns>Combined path</returns>
    let attachSubPath subPath path = [| path; subPath |] |> combinePathes

    /// <summary>
    /// Creates a directory
    /// </summary>
    /// <param name="path">Path to directory to create</param>
    /// <returns>Result of DirectoryInfo or Exception if it fails</returns>
    let create path = 
        let create () = Directory.CreateDirectory path
        exceptionToResult create

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

    let getFiles path = 
        let getFiles () = DirectoryInfo(path).GetFiles()
        exceptionToResult getFiles

    let getDirectories path = 
        let getDirs () = DirectoryInfo(path).GetDirectories()
        exceptionToResult getDirs

    /// <summary>
    /// Retrieves FileSystemInfos for the specified path, first Infos of directories, then infos of files.
    /// </summary>
    let getFileSystemInfos path = 
        let getAsInfo n = n :> FileSystemInfo
        let getFiles path = DirectoryInfo(path).GetFiles() |> Array.map getAsInfo
        let getDirectories path = DirectoryInfo(path).GetDirectories() |> Array.map getAsInfo
        let getFileSystemInfos path = Array.concat [|getDirectories path; getFiles path |] 
        exceptionToResult (fun () -> getFileSystemInfos path)

    let existsFile file = File.Exists file    

    let existsDirectory path = Directory.Exists path

    /// <summary>
    /// Returns the name of the existing file, otherwise None
    /// </summary>
    let getExistingFile file = if existsFile file then Some file else None 

    let move (sourcePath: string, targetPath: string) = 
        let move () = Directory.Move (sourcePath, targetPath)
        exceptionToResult move

