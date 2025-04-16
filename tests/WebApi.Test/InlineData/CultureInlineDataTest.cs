using System.Collections;

namespace WebApi.Test.InlineData;

public class CultureInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return ["pt-BR"];
        yield return ["pt-PT"];
        yield return ["en"];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}