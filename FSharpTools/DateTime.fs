namespace FSharpTools

module DateTime =

    open System
    open System.Globalization

    open FSharpTools.Deprecated

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

    /// <summary>
    /// Creates a DateTime from an UNIX timestamp
    /// </summary>
    /// <param name="timestampInMillis">The UNIX timestamp in milliseconds</param>
    /// <returns>The created DateTime</returns>
    let fromUnixTime (timestampInMillis: int64) =
        let start = DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        start.AddMilliseconds(float timestampInMillis).ToLocalTime()

