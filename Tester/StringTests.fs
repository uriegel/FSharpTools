module Tests

open NUnit.Framework
open FSharpTools

[<TestFixture>]
type StringTest () = 
    [<Test>]
    member this.``My test`` () =
        Assert.True(true)

    [<Test>]
    [<TestCase("Start", "End", "A substring between the string Start<here is the content of the substring>End. This part is not to be considered.", ExpectedResult = "<here is the content of the substring>")>]
    [<TestCase("<Tag>", "</Tag>", "<Doc><Item><Tag>A substring between tags</Tag></Item></Doc", ExpectedResult = "A substring between tags")>]
    [<TestCase("<start>", "<end>", "In this substring is nothing to be found", ExpectedResult = null)>]
    member this.``subStringBetweenStrs`` (startStr, endStr, str) =
        match String.subStringBetweenStrs startStr endStr str with
        | Some value -> value
        | None -> null
    