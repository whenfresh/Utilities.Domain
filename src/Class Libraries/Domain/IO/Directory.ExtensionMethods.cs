namespace Cavity.IO
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
#if NET40
    using System.Numerics;
#endif
    using System.Xml;
    using Cavity.Collections;

    public static class DirectoryExtensionMethods
    {
#if NET40
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "I want strong typing here.")]
        public static BigInteger ToBigInteger(this DirectoryInfo directory)
        {
            if (null == directory)
            {
                throw new ArgumentNullException("directory");
            }

            return BigInteger.Parse(directory.Name, CultureInfo.InvariantCulture);
        }
#endif

        public static FileInfo ToCsvFile(this DirectoryInfo directory,
                                         FileInfo file)
        {
            if (null == directory)
            {
                throw new ArgumentNullException("directory");
            }

            if (null == file)
            {
                throw new ArgumentNullException("file");
            }

            return ToCsvFile(directory, file.RemoveExtension().Name);
        }

        public static FileInfo ToCsvFile(this DirectoryInfo directory,
                                         string value)
        {
            if (null == directory)
            {
                throw new ArgumentNullException("directory");
            }

            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            value = value.Trim();
            if (value.IsEmpty())
            {
                throw new ArgumentOutOfRangeException("value");
            }

            return directory.ToFile("{0}.csv".FormatWith(value));
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "I want strong typing here.")]
        public static Date ToDate(this DirectoryInfo directory)
        {
            if (null == directory)
            {
                throw new ArgumentNullException("directory");
            }

            return directory.Name;
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "I want strong typing here.")]
        public static int ToInt32(this DirectoryInfo directory)
        {
            if (null == directory)
            {
                throw new ArgumentNullException("directory");
            }

            return XmlConvert.ToInt32(directory.Name);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "I want strong typing here.")]
        public static Month ToMonth(this DirectoryInfo directory)
        {
            if (null == directory)
            {
                throw new ArgumentNullException("directory");
            }

            return directory.Name;
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "I want strong typing here.")]
        public static Quarter ToQuarter(this DirectoryInfo directory)
        {
            if (null == directory)
            {
                throw new ArgumentNullException("directory");
            }

            return directory.Name;
        }
    }
}