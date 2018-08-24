using GitDiffReader.Format;
using System;
using System.IO;

namespace GitDiffReader
{
    public class GitDiffReader
    {
        public GitDiff Read(String diff)
        {
            if (String.IsNullOrWhiteSpace(diff))
            {
                return null;
            }

            var result = new GitDiff();

            using (StringReader reader = new StringReader(diff))
            {
                GitDiffParser parser = new GitDiffParser(reader);

                if (parser.TryReadInputSources(result))
                {
                    if (parser.TryReadMetadata(result))
                    {
                        if (parser.TryReadMarkers(result))
                        {
                            return result;
                        }
                    }
                }
            }

            return null;
        }
    }
}
