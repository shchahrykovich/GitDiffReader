namespace GitDiffReader.Format
{
    public class GitDiff
    {
        public string InputSources { get; internal set; }
        public string LeftInput { get; internal set; }
        public string RightInput { get; internal set; }
        public string Metadata { get; internal set; }
        public GitDiffMarker LeftMarker { get; internal set; }
        public GitDiffMarker RightMarker { get; internal set; }
    }
}
