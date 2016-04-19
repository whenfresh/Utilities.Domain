namespace Cavity.IO
{
    using System;
    using System.Globalization;
    using System.IO;
#if NET40
    using System.Numerics;
#endif
    using System.Xml;

    public static class FileExtensionMethods
    {
#if NET40
        public static BigInteger ToBigInteger(this FileInfo file)
        {
            if (null == file)
            {
                throw new ArgumentNullException("file");
            }

            return BigInteger.Parse(file.RemoveExtension().Name, CultureInfo.InvariantCulture);
        }
#endif

        public static Date ToDate(this FileInfo file)
        {
            if (null == file)
            {
                throw new ArgumentNullException("file");
            }

            return file.RemoveExtension().Name;
        }

        public static int ToInt32(this FileInfo file)
        {
            if (null == file)
            {
                throw new ArgumentNullException("file");
            }

            return XmlConvert.ToInt32(file.RemoveExtension().Name);
        }

        public static Month ToMonth(this FileInfo file)
        {
            if (null == file)
            {
                throw new ArgumentNullException("file");
            }

            return file.RemoveExtension().Name;
        }

        public static Quarter ToQuarter(this FileInfo file)
        {
            if (null == file)
            {
                throw new ArgumentNullException("file");
            }

            return file.RemoveExtension().Name;
        }
    }
}