namespace Cavity.IO
{
    using System;
    using System.IO;
#if NET40
    using System.Numerics;
#endif
    using Xunit;

    public sealed class FileExtensionMethodsFacts
    {
#if NET40
        [Fact]
        public void op_ToBigInteger_FileInfoNull()
        {
            Assert.Throws<ArgumentNullException>(() => (null as FileInfo).ToBigInteger());
        }

        [Fact]
        public void op_ToBigInteger_FileInfo()
        {
            using (var temp = new TempDirectory())
            {
                var file = temp.Info.ToFile("1234567890.txt").CreateNew(string.Empty);

                BigInteger expected = 1234567890;
                var actual = file.ToBigInteger();

                Assert.Equal(expected, actual);
            }
        }
#endif

        [Fact]
        public void op_ToDate_FileInfoNull()
        {
            Assert.Throws<ArgumentNullException>(() => (null as FileInfo).ToDate());
        }

        [Fact]
        public void op_ToDate_FileInfo()
        {
            using (var temp = new TempDirectory())
            {
                var file = temp.Info.ToFile("1969-03-10.txt").CreateNew(string.Empty);

                var expected = new Date(1969, MonthOfYear.March, 10);
                var actual = file.ToDate();

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void op_ToInt32_FileInfoNull()
        {
            Assert.Throws<ArgumentNullException>(() => (null as FileInfo).ToInt32());
        }

        [Fact]
        public void op_ToInt32_FileInfo()
        {
            using (var temp = new TempDirectory())
            {
                var file = temp.Info.ToFile("1234567890.txt").CreateNew(string.Empty);

                var expected = 1234567890;
                var actual = file.ToInt32();

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void op_ToMonth_FileInfoNull()
        {
            Assert.Throws<ArgumentNullException>(() => (null as FileInfo).ToMonth());
        }

        [Fact]
        public void op_ToMonth_FileInfo()
        {
            using (var temp = new TempDirectory())
            {
                var file = temp.Info.ToFile("1969-03.txt").CreateNew(string.Empty);

                var expected = new Month(1969, MonthOfYear.March);
                var actual = file.ToMonth();

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void op_ToQuarter_FileInfoNull()
        {
            Assert.Throws<ArgumentNullException>(() => (null as FileInfo).ToQuarter());
        }

        [Fact]
        public void op_ToQuarter_FileInfo()
        {
            using (var temp = new TempDirectory())
            {
                var file = temp.Info.ToFile("1969 Q1.txt").CreateNew(string.Empty);

                var expected = new Quarter(1969, QuarterOfYear.Q1);
                var actual = file.ToQuarter();

                Assert.Equal(expected, actual);
            }
        }
    }
}