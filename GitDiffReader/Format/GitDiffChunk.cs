using System;

namespace GitDiffReader
{
    public class GitDiffChunk
    {
        public const Char ChunkFirstSymbol = '@';
        public int RemovedLines { get; internal set; }
        public int AddedLines { get; internal set; }
    }
}
