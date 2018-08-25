using GitDiffReader.Format;
using System;
using System.IO;
using System.Linq;

namespace GitDiffReader
{
    internal class GitDiffParser
    {
        private readonly StringReader _reader;
        private readonly char[] _delimeter = new [] { ' ' };

        public GitDiffParser(StringReader reader)
        {
            _reader = reader;
        }

        internal GitDiff Parse()
        {
            Func<GitDiff, bool>[] stages = GetParsingStages();

            var result = new GitDiff();
            var stageResults = stages.TakeWhile(s => s(result)).ToArray();

            bool isValid = stages.Length == stageResults.Length;
            return isValid ? result : null;
        }

        private Func<GitDiff, bool>[] GetParsingStages()
        {
            return new Func<GitDiff, bool>[]
            {
                (GitDiff d) => TryReadInputSources(d),
                (GitDiff d) => TryReadMetadata(d),
                (GitDiff d) => TryReadMarkers(d),
                (GitDiff d) => TryParseChunks(d)
            };
        }

        private bool TryReadInputSources(GitDiff diff)
        {
            bool result = false;

            var rawInputSources = _reader.ReadLine();
            if (!String.IsNullOrWhiteSpace(rawInputSources))
            {
                var parts = rawInputSources.Split(_delimeter, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == (int)InputSourcesLineParts.TotalNumberOfParts)
                {
                    diff.InputSources = rawInputSources;
                    diff.LeftInput = parts[(int)InputSourcesLineParts.LeftFile];
                    diff.RightInput = parts[(int)InputSourcesLineParts.RightFile];
                }

                result = true;
            }

            return result;
        }

        private bool TryParseChunks(GitDiff result)
        {
            var rawChunkHeader = _reader.ReadLine();
            while (null != rawChunkHeader && GitDiffChunk.ChunkFirstSymbol == rawChunkHeader[0])
            {
                var chunk = new GitDiffChunk();
                var line = _reader.ReadLine();
                while (null != line)
                {
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        if (GitDiffChunk.ChunkFirstSymbol == line[0])
                        {
                            break;
                        }

                        switch (line[0])
                        {
                            case '+': chunk.AddedLines++; break;
                            case '-': chunk.RemovedLines++; break;
                        }
                    }
                    line = _reader.ReadLine();
                }
                result.AddChunk(chunk);

                rawChunkHeader = line;
            }

            return true;
        }

        private bool TryReadMetadata(GitDiff diff)
        {
            bool result = false;

            var rawMetadata = _reader.ReadLine();
            if (!String.IsNullOrWhiteSpace(rawMetadata))
            {
                diff.Metadata = rawMetadata;
                if (!rawMetadata.StartsWith("index"))
                {
                    diff.Metadata += Environment.NewLine;
                    diff.Metadata += _reader.ReadLine();
                }
                result = true;
            }

            return result;
        }

        private bool TryReadMarkers(GitDiff diff)
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
