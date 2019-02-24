module EnumerableExtensions

open System
open System.Collections
open System.Collections.Generic

/// <summary>Casts a non generic enumerator to a generic version of this enumerator</summary>
///<typeparam name="'U">The type of the returned casted Enumerator</typeparam>
/// <param name="enum">Non generic enumerator to be casted.</param>
/// <returns>The generic version of this non generic enumerator.</returns>
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

/// <summary>Turns an <c>IEnumerator</c> into a <c>seq</c></summary>
/// <param name="enumerator">Generic enumerator to be turned into a seq.</param>
/// <returns>The newly created <c>seq</c>.</returns>
let makeSeq enumerator = {
    new IEnumerable<'U> with
        member x.GetEnumerator() = enumerator
    interface IEnumerable with
        member x.GetEnumerator() = 
            (enumerator :> IEnumerator)
}




