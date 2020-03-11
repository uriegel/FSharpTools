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

/// <summary>
/// Splits a string into parts, separator is one char
/// If the string is null, an emtpy array is returned
/// </summary>
/// <param name="chr">The separator</param>
/// <param name="str">String to be splitted</param>
/// <returns>The trimmed string</returns>
let splitChar chr (str: string) =
    if not (isNull str) then 
        str.Split ([|chr|])
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

