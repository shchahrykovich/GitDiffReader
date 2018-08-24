using System.Collections.Generic;

namespace GitDiffReader.Format
{
    public class GitDiff
    {
        private List<GitDiffChunk> _chunks = new List<GitDiffChunk>();

        public string InputSources { get; internal set; }
        public string LeftInput { get; internal set; }
        public string RightInput { get; internal set; }
        public string Metadata { get; internal set; }
        public GitDiffMarker LeftMarker { get; internal set; }
        public GitDiffMarker RightMarker { get; internal set; }

        public IEnumerable<GitDiffChunk> Chunks => _chunks;

        internal void AddChunk(GitDiffChunk chunk)
        {
            _chunks.Add(chunk);
        }
    }
}
