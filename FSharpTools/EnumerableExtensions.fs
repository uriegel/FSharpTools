module EnumerableExtensions

open System
open System.Collections
open System.Collections.Generic

let castEnumerator<'U> (enumerator: IEnumerator) = {
    new IEnumerator<'U> with
        member x.Current with get() = enumerator.Current :?> 'U
    interface IEnumerator with
        member x.Current with get() = enumerator.Current
        member x.MoveNext() = enumerator.MoveNext()
        member x.Reset() = enumerator.Reset()
    interface IDisposable with
        member x.Dispose() = ()
}      

let makeSeq enumerator = {
    new IEnumerable<'U> with
        member x.GetEnumerator() = enumerator
    interface IEnumerable with
        member x.GetEnumerator() = 
            (enumerator :> IEnumerator)
}




