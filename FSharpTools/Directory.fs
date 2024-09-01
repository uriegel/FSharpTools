namespace FSharpTools
open System
open EnumExtensions

type FileSystemInfo = {
    Path: string
    Items: IO.FileSystemInfo array
}

module Directory = 
    open System
    open System.IO

    open Result
    open Functional

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
        catch create

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

    let internal retrieveHomeDir () = Environment.GetFolderPath Environment.SpecialFolder.UserProfile

    /// <summary>
    /// Gets the user's home directory. /home/<user> for Linux 
    /// </summary>
    /// <returns>The user's home directory</returns>
    let getHomeDir = memoizeSingle retrieveHomeDir

    let getFiles path = 
        let getFiles () = DirectoryInfo(path).GetFiles()
        catch getFiles

    let getDirectories path = 
        let getDirs () = DirectoryInfo(path).GetDirectories()
        catch getDirs

    /// <summary>
    /// Retrieves FileSystemInfos for the specified path, first Infos of directories, then infos of files.
    /// </summary>
    /// <param name="path">Path for retrieving file system infos</param>
    /// <returns>FileSystemInfos and current path</returns>
    let getFileSystemInfo path = 
        let getAsInfo n = n :> FileSystemInfo
        let dirInfo = DirectoryInfo path
        let getFiles path = dirInfo.GetFiles() |> Array.map getAsInfo
        let getDirectories path = 
            DirectoryInfo(path).GetDirectories() 
            |> Array.sortWith (fun x y -> String.Compare(x.Name, y.Name, StringComparison.CurrentCultureIgnoreCase))
            |> Array.map getAsInfo
        let getFileSystemInfos path = 
            let items = Array.concat [|getDirectories path; getFiles path |] 
            {
                Path = dirInfo.FullName
                Items = items
            }   
        catch (fun () -> getFileSystemInfos path)

    /// <summary>
    /// Retrieves FileSystemInfos for the specified path, first Infos of directories, then infos of files.
    /// </summary>
    /// <param name="path">Path for retrieving file system infos</param>
    /// <returns>FileSystemInfos</returns>
    let getFileSystemInfos = 
        getFileSystemInfo >> (fun info -> info |> Result.map (fun i -> i.Items))

    let existsFile file = File.Exists file    

    let existsDirectory path = Directory.Exists path

    /// <summary>
    /// Returns the name of the existing file, otherwise None
    /// </summary>
    let getExistingFile file = if existsFile file then Some file else None 

    let move (sourcePath: string, targetPath: string) = 
        let move () = Directory.Move (sourcePath, targetPath)
        catch move

    /// <summary>
    /// Check if this path is a directory or a file
    /// </summary>
    /// <param name="path">Path od file or directory</param>
    /// <returns>true: is directory, otherwise false</returns>
    let isDirectory (path: string) = 
        try
            if File.Exists path then
                File.GetAttributes (path) |> hasFlag FileAttributes.Directory 
            else
                false
        with
        | _ -> false
