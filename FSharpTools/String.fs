namespace FSharpTools

module String = 
    open System
    open FSharpTools
    open Option

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

    let endsWith (strTest: string) (str: string) =
        if not (isNull str) && not (isNull strTest) then 
            str.EndsWith strTest
        else 
            false

    let endsWithComparison (strTest: string) (comparisonType: StringComparison) (str: string) =        
        if not (isNull str) && not (isNull strTest) then 
            str.EndsWith (strTest, comparisonType)
        else 
            false
       
    let startsWith (testStr: string) (str: string) =
        if not (isNull testStr) && not (isNull str) then
            str.StartsWith testStr
        else
            false

    let startsWithComparison (testStr: string) (comparisonType: StringComparison) (str: string) =
        if not (isNull testStr) && not (isNull str) then
            str.StartsWith (testStr, comparisonType)
        else
            false

    /// <summary>
    /// Splits a string into parts, separator is one char
    /// If the string is null, an emtpy array is returned
    /// </summary>
    /// <param name="sep">The separator</param>
    /// <param name="str">String to be splitted</param>
    /// <returns>The trimmed string</returns>
    let split (sep: string) (str: string) =
        match isNull str, isNull sep with
        | false, false -> str.Split ([|sep|], StringSplitOptions.RemoveEmptyEntries)
        | false, true -> [|str|]
        |_ -> [||]

    /// <summary>
    /// Splits a string into parts, separator is one char
    /// If the string is null, an emtpy array is returned
    /// </summary>
    /// <param name="seps">The separators</param>
    /// <param name="str">String to be splitted</param>
    /// <returns>The trimmed string</returns>
    let splitMulti (seps: string[]) (str: string) =
        match isNull str, isNull seps with
        | false, false -> str.Split (seps, StringSplitOptions.RemoveEmptyEntries)
        | false, true -> [|str|]
        |_ -> [||]

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
        if not (isNull str) && not (String.IsNullOrEmpty part) then 
            let res = str.IndexOf part
            match res with
            | -1 -> None
            | _ -> Some res
        else
            None

    let indexOfStart (part: string) (startIndex: int) (str: string) =
        if not (isNull str) && not (String.IsNullOrEmpty part) then 
            let res = str.IndexOf (part, startIndex)
            match res with
            | -1 -> None
            | _ -> Some res
        else
            None

    let indexOfCompare (part: string) (comparisonType: StringComparison) (str: string) =
        if not (isNull str) && not (String.IsNullOrEmpty part) then 
            let res = str.IndexOf (part, 0, comparisonType)
            match res with
            | -1 -> None
            | _ -> Some res
        else
            None

    let indexOfStartCompare (part: string) (startIndex: int) (comparisonType: StringComparison) (str: string) =
        if not (isNull str) && not (String.IsNullOrEmpty part) then 
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
        if not (isNull str) && not (isNull part) then 
            let res = str.LastIndexOf part
            match res with
            | -1 -> None
            | _ -> Some res
        else
            None

    let substring pos (str: string) =
        try 
            match str with
            | null -> ""
            | "" -> ""
            | _ -> 
                let pos = max 0 pos
                let pos = min pos (str.Length - 1)
                str.Substring pos
        with _ -> ""

    let substring2 pos length (str: string) =
        try 
            match str with
            | null -> ""
            | "" -> ""
            | _ -> 
                let pos = max 0 pos
                let pos = min pos (str.Length - 1)
                let length = max 0 length
                let length = min length (str.Length - pos - 1)
                str.Substring (pos, length)
        with _ -> ""

    let join (chr: char) (strs: string seq) = System.String.Join (String([|chr|]), strs)

    let joinStr (sep: string) (strs: string seq) = String.Join (sep, strs)

    let replace (a: string) (b: string) (str: string) =
        match a, b, str with
        | _, _, null -> ""
        | null, _, _ -> str
        | _, null, _ -> str
        | _          -> str.Replace (a, b)

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

    let parseInt (str: string) = 
        match Int32.TryParse str with
        | true, num -> Some num
        | _         -> None

    let parseInt64 (str: string) = 
        match Int64.TryParse str with
        | true, num -> Some num
        | _         -> None

    let parseBool (str: string) = 
        match str |> toLowerInvariant with
        | "true"  -> Some true
        | "false" -> Some false
        | _       -> None

    /// <summary>
    /// Counts characters in a sequence of character or string. 
    /// </summary>
    /// <param name="char">Character to count</param>
    /// <param name="seq">Sequence (or string) containg the characters to be counted</param>
    /// <returns>Number of occurrences of character 'char'</returns>
    let getCharCount (char: Char) = Seq.getElementCount char

    /// <summary>
    /// Gets an environment variable if set, otherwise None
    /// </summary>
    /// <param name="key">Which environment variable shoud be retrieved?</param>
    /// <returns>'Some environment variable' or None if not set</returns>
    let retrieveEnvironmentVariable key =
        exceptionToOption (fun () -> System.Environment.GetEnvironmentVariable key)  

    /// <summary>
    /// Returns a substring beween 2 strings 'startStr' and 'endStr'
    /// </summary>
    /// <param name="startStr">After this str the substring starts</param>
    /// <param name="endStr">The substring ends before 'endStr'</param>
    /// <param name="str">The string to be extracted</param>
    /// <returns>'Some substring' between startStr and endStr or None</returns>
    let subStringBetweenStrs startStr endStr str = 
        let startIndex = str |> indexOf startStr
        let endIndex   = str |> indexOfStart endStr (startIndex |> Option.defaultValue 0)
        match startIndex, endIndex with
        | Some s, Some e -> Some (str |> substring2 (s + (startStr |> String.length)) (e - s - (startStr |> String.length)))
        | _                         -> None

    let isEmpty str = 
        String.IsNullOrEmpty str
