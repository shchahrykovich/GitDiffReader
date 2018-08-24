using System;

namespace GitDiffReader.Format
{
    public class GitDiffMarker
    {
        public GitDiffMarker(string marker)
        {
            Marker = marker;
        }

        public String Marker { get; }

        public Char Symbol { get => Marker[0]; }
    }
}
