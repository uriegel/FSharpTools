module FileVersion

open System
open System.Collections.Generic
open System.Diagnostics

/// <summary>
/// Checks if the FileVersionInfo is valid
/// </summary>
/// <param name="fvi">FileVersionInfo to be checked</param>
/// <returns>true if the version is valid</returns>
let hasInfo (fvi: FileVersionInfo) = 
    fvi <> null && not (fvi.FileMajorPart = 0 && fvi.FileMinorPart = 0 && fvi.FileBuildPart = 0 && fvi.FilePrivatePart = 0)

/// <summary>
/// Gets the version of a FileVersionInfo as string 
/// </summary>
/// <param name="fvi">FileVersionInfo to be considered</param>
/// <returns>true if the version as string</returns>
let getVersion (fvi: FileVersionInfo) = 
    if hasInfo fvi then 
        Some (string fvi.FileMajorPart + "." + string fvi.FileMinorPart + "." + string fvi.FileBuildPart + "." + string fvi.FilePrivatePart)
    else
        None

/// <summary>
/// Parses a version string
/// </summary>
/// <param name="versionString">The string to be parsed</param>
/// <returns>A tuple containing the 4 version parts</returns>
let parse versionString = 
    let parts = versionString |> String.splitChar '.'
    let getPart index = 
        if index < parts.Length then
            int parts.[index]
        else
            0
    if parts.Length = 0 then
        0, 0, 0, 0
    else
        getPart 0, getPart 1, getPart 2, getPart 3 


/// <summary>
/// Compares to tuples containing version parts
/// </summary>
/// <returns>0 if versions are equal</returns>
let compare (x0, x1, x2, x3) (y0, y1, y2, y3) =
    if x0 <> y0 then x0 - y0 
    elif x1 <> y1 then x1 - y1 
    elif x2 <> y2 then x2 - y2 
    else x3 - y3

type VersionComparer() = 
    interface IComparer<int*int*int*int> with
        member this.Compare (x, y) = compare x y
