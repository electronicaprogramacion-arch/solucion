
namespace FormatValidator
{
    using System.Collections.Generic;
    using System.IO;

    public interface ISourceReader
    {
        IEnumerable<string> ReadLines(string rowSeperator);
    }
}
