using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Threading;

namespace Playground.Mocks
{
    public sealed class TestAdditionalText : AdditionalText
    {
        private readonly SourceText text;

        public TestAdditionalText(string path, SourceText text)
        {
            Path = path;
            this.text = text;
        }

        public TestAdditionalText(string text = "", Encoding encoding = null, string path = "dummy")
            : this(path, new StringText(text, encoding))
        {
        }

        public override string Path { get; }

        public override SourceText GetText(CancellationToken cancellationToken = default) => text;
    }
}
    