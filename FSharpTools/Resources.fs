namespace FSharpTools
open System.Reflection

module Resources =
    /// <summary>
    /// Gets a stream from resource
    /// </summary>
    /// <param name="path">Path to the resource</param>
    /// <param name="assembly">Assembly containing the desired resource</param>
    /// <returns>A resource stream option</returns>
    let getFromAssembly path (assembly: Assembly) =
        assembly.GetManifestResourceStream path
        |> Option.checkNull

    /// <summary>
    /// Gets a stream from resource
    /// </summary>
    /// <param name="path">Path to the resource</param>
    /// <returns>A resource stream option</returns>
    let get path =
        Assembly
            .GetEntryAssembly()
            .GetManifestResourceStream path
        |> Option.checkNull