using System.Collections;

namespace Chess.Core;

public sealed class MovesHistory : IEnumerable<Result>
{
    private readonly List<Result> _moveResults = [];

    public event Action<Result>? Added;
    public event Action? Cleared; 
    
    internal void Clear()
    {
        _moveResults.Clear();
        Cleared?.Invoke();
    }
    
    internal void Add(Result result)
    {
        _moveResults.Add(result);
        Added?.Invoke(result);
    }

    public IEnumerator<Result> GetEnumerator()
    {
        return _moveResults.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal static MovesHistory Create()
    {
        return [];
    }
}