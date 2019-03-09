module String

/// <summary>
/// Functional string.TrimEnd, parameter is one trim char 
/// If the string is null, an emtpy string is returned
/// </summary>
/// <param name="chr">Char to be trimmed at the end</param>
/// <param name="str">String to be trimmed</param>
/// <returns>The trimmed string</returns>
let trimEnd chr (str: string) =
    if str <> null then 
        str.TrimEnd [| chr |] 
    else 
        ""

/// <summary>
/// Splits a string into parts, separator is one char
/// If the string is null, an emtpy array is returned
/// </summary>
/// <param name="chr">The separator</param>
/// <param name="str">String to be splitted</param>
/// <returns>The trimmed string</returns>
let splitChar chr (str: string) =
    if str <> null then
        str.Split([|chr|])
    else
        [||]



