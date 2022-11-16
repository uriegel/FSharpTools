module Tests

open Xunit
open FSharpTools

[<Fact>]
let ``My test`` () =
    Assert.True(true)

[<Theory>]
[<InlineData("Start", "End", "A substring between the string Start<here is the content of the substring>End. This part is not to be considered.", "<here is the content of the substring>")>]
[<InlineData("<Tag>", "</Tag>", "<Doc><Item><Tag>A substring between tags</Tag></Item></Doc", "A substring between tags")>]
[<InlineData("<start>", "<end>", "In this substring is nothing to be found", null)>]
let ``subStringBetweenStrs`` (startStr, endStr, str, result: string) =
    Assert.Equal(String.subStringBetweenStrs startStr endStr str, Option.ofObj result)
    