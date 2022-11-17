module Tests

open Xunit
open Xunit.Abstractions

open FSharpTools

type ClassDataBase(generator : obj [] seq) = 
    interface seq<obj []> with
        member this.GetEnumerator() = generator.GetEnumerator()
        member this.GetEnumerator() = 
            generator.GetEnumerator() :> System.Collections.IEnumerator

// TODO test indexOf

type subStringBetweenStrsTest () = 
    inherit ClassDataBase([ 
        [| "Start"; "End"; "A substring between the string Start<here is the content of the substring>End. This part is not to be considered."; Some "<here is the content of the substring>" |]
        [| "<Tag>"; "</Tag>"; "<Doc><Item><Tag>A substring between tags</Tag></Item></Doc"; Some "A substring between tags" |]
        [| "<start>"; "<end>"; "In this substring is nothing to be found"; None |]
        [| ""; ""; "In this substring is nothing to be found"; None |]
        [| "nothing "; ""; "In this substring is nothing to be found"; None |]
    ])

type StringTests(output: ITestOutputHelper) =
    [<Theory>]
    [<ClassData(typeof<subStringBetweenStrsTest>)>]
    let subStringBetweenStrs (startStr, endStr, str, result) = 
        Assert.Equal(String.subStringBetweenStrs startStr endStr str, result)
