namespace FSharpTools
open System

module Disposable =
    let auto<'a, 'd when 'd :> IDisposable> (func: 'd->'a) (disposable: 'd) =
        let res = func disposable
        disposable.Dispose()
        res
