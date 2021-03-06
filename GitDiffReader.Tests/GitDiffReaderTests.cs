using System;
using System.Linq;
using Xunit;

namespace GitDiffReader.Tests
{
    public class GitDiffReaderTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("d")]
        [InlineData("diff --git a/diff_test.txt")]
        [InlineData("diff --git a/diff_test.txt ")]
        [InlineData("diff --git a/diff_test.txt b/diff_test.txt")]
        [InlineData(@"diff --git a/diff_test.txt b/diff_test.txt
index 6b0c6cf..b37e70a 100644
--- a/diff_test.txt")]
        [InlineData(@"diff --git a/diff_test.txt b/diff_test.txt
index 6b0c6cf..b37e70a 100644")]
        public void Read_Should_Return_Null_For_Invalid_Input(String diff)
        {
            // Arrange
            var reader = new GitDiffReader();

            // Act
            var result = reader.Read(diff);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// https://www.atlassian.com/git/tutorials/saving-changes/git-diff
        /// </summary>
        [Fact]
        public void Read_Should_Parse_Simple_Diff()
        {
            // Arrange
            var diff = @"diff --git a/diff_test.txt b/diff_test.txt
index 6b0c6cf..b37e70a 100644
--- a/diff_test.txt
+++ b/diff_test.txt
@@ -1 +1 @@
-this is a git diff test example
+this is a diff example";
            var reader = new GitDiffReader();

            // Act
            var result = reader.Read(diff);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("diff --git a/diff_test.txt b/diff_test.txt", result.InputSources);
            Assert.Equal("a/diff_test.txt", result.LeftInput);
            Assert.Equal("b/diff_test.txt", result.RightInput);

            Assert.Equal("index 6b0c6cf..b37e70a 100644", result.Metadata);

            Assert.Equal("---", result.LeftMarker.Marker);
            Assert.Equal('-', result.LeftMarker.Symbol);
            Assert.Equal("+++", result.RightMarker.Marker);
            Assert.Equal('+', result.RightMarker.Symbol);

            Assert.Single(result.Chunks);
            Assert.Equal(1, result.Chunks.First().AddedLines);
            Assert.Equal(1, result.Chunks.First().RemovedLines);
        }

        [Fact]
        public void Read_Should_Parse_Real_Diff()
        {
            // Arrange
            var diff = @"diff --git a/GitDiffReader.Tests/GitDiffReaderTests.cs b/GitDiffReader.Tests/GitDiffReaderTests.cs
index c7af73f..9098adb 100644
--- a/GitDiffReader.Tests/GitDiffReaderTests.cs
+++ b/GitDiffReader.Tests/GitDiffReaderTests.cs
@@ -62,7 +62,7 @@ index 6b0c6cf..b37e70a 100644
Assert.Equal(""++ + "", result.RightMarker.Marker);
Assert.Equal('+', result.RightMarker.Symbol);

-Assert.Equal(1, result.Chunks.Count());
+Assert.Single(result.Chunks);
-Assert.Equal(1, result.Chunks.First().AddedLines);
Assert.Equal(1, result.Chunks.First().RemovedLines);
        }
";
            var reader = new GitDiffReader();

            // Act
            var result = reader.Read(diff);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("diff --git a/GitDiffReader.Tests/GitDiffReaderTests.cs b/GitDiffReader.Tests/GitDiffReaderTests.cs", result.InputSources);
            Assert.Equal("a/GitDiffReader.Tests/GitDiffReaderTests.cs", result.LeftInput);
            Assert.Equal("b/GitDiffReader.Tests/GitDiffReaderTests.cs", result.RightInput);

            Assert.Equal("index c7af73f..9098adb 100644", result.Metadata);

            Assert.Equal("---", result.LeftMarker.Marker);
            Assert.Equal('-', result.LeftMarker.Symbol);
            Assert.Equal("+++", result.RightMarker.Marker);
            Assert.Equal('+', result.RightMarker.Symbol);

            Assert.Single(result.Chunks);
            Assert.Equal(1, result.Chunks.First().AddedLines);
            Assert.Equal(2, result.Chunks.First().RemovedLines);
        }

        [Fact]
        public void Read_Should_Parse_Diff_For_New_File()
        {
            // Arrange
            var diff = @"diff --git a/build.ps1 b/build.ps1
new file mode 100644
index 0000000..12afc79
--- /dev/null
+++ b/build.ps1
@@ -0,0 +1,9 @@
+
";
            var reader = new GitDiffReader();

            // Act
            var result = reader.Read(diff);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("diff --git a/build.ps1 b/build.ps1", result.InputSources);
            Assert.Equal("a/build.ps1", result.LeftInput);
            Assert.Equal("b/build.ps1", result.RightInput);

            Assert.Equal(@"new file mode 100644
index 0000000..12afc79", result.Metadata);

            Assert.Equal("---", result.LeftMarker.Marker);
            Assert.Equal('-', result.LeftMarker.Symbol);
            Assert.Equal("+++", result.RightMarker.Marker);
            Assert.Equal('+', result.RightMarker.Symbol);

            Assert.Single(result.Chunks);
            Assert.Equal(1, result.Chunks.First().AddedLines);
            Assert.Equal(0, result.Chunks.First().RemovedLines);
        }
    }
}
