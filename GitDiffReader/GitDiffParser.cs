using GitDiffReader.Format;
using System;
using System.IO;

namespace GitDiffReader
{
    internal class GitDiffParser
    {
        private readonly StringReader _reader;
        private readonly char[] _delimeter = new [] { ' ' };
        private const int InputSourcesExpectedNumberOfParts = 4;

        public GitDiffParser(StringReader reader)
        {
            _reader = reader;
        }

        internal bool TryReadInputSources(GitDiff diff)
        {
            bool result = false;

            var rawInputSources = _reader.ReadLine();
            if (!String.IsNullOrWhiteSpace(rawInputSources))
            {
                var parts = rawInputSources.Split(_delimeter, StringSplitOptions.RemoveEmptyEntries);
                if(parts.Length == (int)InputSourcesLineParts.TotalNumberOfParts)
                {
                    diff.InputSources = rawInputSources;
                    diff.LeftInput = parts[(int)InputSourcesLineParts.LeftFile];
                    diff.RightInput = parts[(int)InputSourcesLineParts.RightFile];
                }

                result = true;
            }

            return result;
        }

        internal bool TryReadMetadata(GitDiff diff)
        {
            bool result = false;

            var rawMetadata = _reader.ReadLine();
            if (!String.IsNullOrWhiteSpace(rawMetadata))
            {
                diff.Metadata = rawMetadata;
                result = true;
            }

            return result;
        }

        internal bool TryReadMarkers(GitDiff diff)
        {
            bool result = false;

            var leftMarkers = GetMarkerLineParts();
            var rightMarkers = GetMarkerLineParts();
            if (null != leftMarkers && null != rightMarkers)
            {
                diff.LeftMarker = new GitDiffMarker(leftMarkers[(int)MarkerLineParts.MarkerSymbol]);
                diff.RightMarker = new GitDiffMarker(rightMarkers[(int)MarkerLineParts.MarkerSymbol]);

                result = true;
            }

            return result;
        }

        private String[] GetMarkerLineParts()
        {
            String[] result = null;

            var rawMarkers = _reader.ReadLine();
            if (!String.IsNullOrWhiteSpace(rawMarkers))
            {
                var markerParts = rawMarkers.Split(_delimeter, StringSplitOptions.RemoveEmptyEntries);
                if (markerParts.Length == (int)MarkerLineParts.TotalNumberOfParts)
                {
                    return markerParts;
                }
            }

            return result;
        }
    }
}
