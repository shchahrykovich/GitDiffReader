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

            using (StringReader reader = new StringReader(diff))
            {
                GitDiffParser parser = new GitDiffParser(reader);
                return parser.Parse();
            }
        }
    }
}