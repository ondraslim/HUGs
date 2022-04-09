using System;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis.Text;

namespace HUGs.Generator.DDD.IntegrationTests.Setup.Mocks
{
    internal sealed class StringText : SourceText
    {
        private readonly string source;
        private readonly Encoding encodingOpt;

        internal StringText(
            string source,
            Encoding encodingOpt,
            ImmutableArray<byte> checksum = default,
            SourceHashAlgorithm checksumAlgorithm = SourceHashAlgorithm.Sha1)
            : base(checksum, checksumAlgorithm)
        {
            this.source = source;
            this.encodingOpt = encodingOpt;
        }

        public override Encoding Encoding => encodingOpt;

        public string Source => source;

        public override int Length => source.Length;

        public override char this[int position] => source[position];

        public override string ToString(TextSpan span)
        {
            if (span.End > Source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(span));
            }

            if (span.Start == 0 && span.Length == Length)
            {
                return Source;
            }

            return Source.Substring(span.Start, span.Length);
        }

        public override void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            Source.CopyTo(sourceIndex, destination, destinationIndex, count);
        }

        public override void Write(TextWriter textWriter, TextSpan span, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (span.Start == 0 && span.End == Length)
            {
                textWriter.Write(Source);
            }
            else
            {
                base.Write(textWriter, span, cancellationToken);
            }
        }
    }
}