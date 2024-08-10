namespace FSharpTools
open System.IO

module File = 

    /// <summary>
    /// Reads all text from a text file (Utf8)
    /// </summary>
    /// <param name="path">Full path to the text file</param>
    /// <returns>Containing text</returns>
    let readAllText path = 
        if Directory.existsFile path then
            use sr = new StreamReader (File.OpenRead path)
            Some <| sr.ReadToEnd ()
        else
            None

    /// <summary>
    /// Writes all text from to a file (Utf8)
    /// </summary>
    /// <param name="text">Text to write</param>
    /// <param name="path">Full path to the text file</param>
    let writeAllText (text: string) path = 
        use sw = File.CreateText(path)
        sw.Write text 
