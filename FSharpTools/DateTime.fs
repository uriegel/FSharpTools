module DateTime 

open System
open System.Globalization

/// <summary>
/// Active Pattern: parses a dateTimeString using a pattern
/// </summary>
/// <param name="format">Format pattern used by parsing</param>
/// <param name="str">A string to be pared</param>
/// <returns>The parsed DateTime value or None</returns>
let (|Value|_|) (format: string) (dateString: string) =
    match DateTime.TryParseExact(dateString |> String.trimEnd (char 0), format, CultureInfo.InvariantCulture, DateTimeStyles.None) with
    | true, value -> Some value
    | _ -> None


