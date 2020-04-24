module String
open System

let trim (str: string) =
    match str with
    | null -> ""
    | _ -> str.Trim [||]

let trimChars (chrs: char array) (str: string) =
    match str with
    | null -> ""
    | _ -> str.Trim chrs    
    
/// <summary>
/// Functional string.TrimEnd, parameter is one trim char 
/// If the string is null, an emtpy string is returned
/// </summary>
/// <param name="chr">Char to be trimmed at the end</param>
/// <param name="str">String to be trimmed</param>
/// <returns>The trimmed string</returns>
let trimEnd chr (str: string) =
    if not (isNull str) then 
        str.TrimEnd [| chr |] 
    else 
        ""

let endsWith (str: string) =
    if not (isNull str) then 
        str.EndsWith str
    else 
        false

let endsWithComparison (str: string) (comparisonType: StringComparison) =        
    if not (isNull str) then 
        str.EndsWith (str, comparisonType)
    else 
        false
   
/// <summary>
/// Splits a string into parts, separator is one char
/// If the string is null, an emtpy array is returned
/// </summary>
/// <param name="chr">The separator</param>
/// <param name="str">String to be splitted</param>
/// <returns>The trimmed string</returns>
let splitChar (chr: char) (str: string) =
    if not (isNull str) then 
        str.Split ([|chr|], StringSplitOptions.RemoveEmptyEntries)
    else
        [||]

let contains (part: string) (str: string) =
    if not (isNull str) then 
        str.Contains part
    else
        false

let indexOfChar (chr: char) (str: string) =
    if not (isNull str) then 
        let res = str.IndexOf chr
        match res with
        | -1 -> None
        | _ -> Some res
    else
        None

let indexOfCharStart (chr: char) (startIndex: int) (str: string) =
    if not (isNull str) then 
        let res = str.IndexOf (chr, startIndex)
        match res with
        | -1 -> None
        | _ -> Some res
    else
        None

let indexOf (part: string) (str: string) =
    if not (isNull str) then 
        let res = str.IndexOf part
        match res with
        | -1 -> None
        | _ -> Some res
    else
        None

let indexOfStart (part: string) (startIndex: int) (str: string) =
    if not (isNull str) then 
        let res = str.IndexOf (part, startIndex)
        match res with
        | -1 -> None
        | _ -> Some res
    else
        None

let indexOfCompare (part: string) (str: string) (comparisonType: StringComparison) =
    if not (isNull str) then 
        let res = str.IndexOf (part, 0, comparisonType)
        match res with
        | -1 -> None
        | _ -> Some res
    else
        None

let indexOfStartCompare (part: string) (startIndex: int) (str: string) (comparisonType: StringComparison) =
    if not (isNull str) then 
        let res = str.IndexOf (part, startIndex, comparisonType)
        match res with
        | -1 -> None
        | _ -> Some res
    else
        None

let lastIndexOfChar (chr: char) (str: string) =
    if not (isNull str) then 
        let res = str.LastIndexOf chr
        match res with
        | -1 -> None
        | _ -> Some res
    else
        None

let lastIndexOf (part: string) (str: string) =
    if not (isNull str) then 
        let res = str.LastIndexOf part
        match res with
        | -1 -> None
        | _ -> Some res
    else
        None

let substring pos (str: string) =
    match str with
    | null -> ""
    | _ -> str.Substring pos

let substring2 pos length (str: string) =
    match str with
    | null -> ""
    | _ -> str.Substring (pos, length)

let join (chr: char) (strs: string seq) = String.Join (chr, strs)

let startsWith (testStr: string) (str: string) =
    if not (isNull testStr) && not (isNull str) then
        str.StartsWith testStr
    else
        false

let joinStr (sep: string) (strs: string seq) = String.Join (sep, strs)

let replace (a: string) (b: string) (str: string) =
    if not (isNull str) then
        str.Replace (a, b)
    else
        ""

let replaceChar (a: char) (b: char) (str: string) =
    if not (isNull str) then
        str.Replace (a, b)
    else
        ""

let padRight totalWidth (padChr: char) (str: string) =
    if not (isNull str) then
        str.PadRight (totalWidth, padChr)
    else
        ""

let length (str: string) =
    if not (isNull str) then
        str.Length 
    else
        0

let toUpper (str: string) =
    if not (isNull str) then
        str.ToUpper ()
    else
        ""

let toLower (str: string) =
    if not (isNull str) then
        str.ToLower ()
    else
        ""
let toUpperInvariant (str: string) =
    if not (isNull str) then
        str.ToUpperInvariant ()
    else
        ""

let toLowerInvariant (str: string) =
    if not (isNull str) then
        str.ToLowerInvariant ()
    else
        ""

let icompare a b = 
    System.String.Compare (a, b, System.StringComparison.CurrentCultureIgnoreCase)
