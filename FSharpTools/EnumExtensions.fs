namespace FSharpTools
module EnumExtensions =

    open System

    /// <summary>Check if a flag in an enum is set</summary>
    /// <param name="enum">The enum to be checked.</param>
    /// <param name="flag">Is this flag set?</param>
    /// <returns>True when flag is set.</returns>
    let hasFlag (enum: Enum) flag = enum.HasFlag flag
