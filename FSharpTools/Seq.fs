namespace FSharpTools
module Seq = 

    /// <summary>
    /// Counts elements in a sequence. Warning: complete sequence will be accessed!
    /// </summary>
    /// <param name="elem">Element to count</param>
    /// <param name="seq">Sequence containg the elements to be counted</param>
    /// <returns>Number of occurrences of element 'elem'</returns>
    let getElementCount elem seq = 
        let filter a = a = elem
        
        seq
        |> Seq.filter filter
        |> Seq.length

