namespace Cavity.Data
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
#if !NET20
    using System.Linq;
#endif
    using Cavity.Collections;
    using Cavity.Diagnostics;
    using Cavity.IO;
    using Cavity.Models;

    public class CsvLexiconStorage : IStoreLexicon
    {
        private FileInfo _location;

        public CsvLexiconStorage(FileInfo location)
            : this()
        {
            Location = location;
        }

        private CsvLexiconStorage()
        {
            Trace.WriteIf(Tracing.Is.TraceVerbose, string.Empty);
        }

        public FileInfo Location
        {
            get
            {
                return _location;
            }

            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("value");
                }

#if !NET20
                Trace.WriteIf(Tracing.Is.TraceVerbose, "value=\"{0}\"".FormatWith(value.FullName));
#endif
                _location = value;
            }
        }

        private IEnumerable<FileInfo> Hierarchy
        {
            get
            {
                var file = Location;
                file.Refresh();

                while (true)
                {
                    if (file.Exists)
                    {
                        yield return file;
                    }

                    if (null == file.Directory)
                    {
                        yield break;
                    }

                    if (null == file.Directory.Parent)
                    {
                        yield break;
                    }

#if NET20
                    file = new FileInfo(DirectoryInfoExtensionMethods.ToFile(file.Directory.Parent, file.Name).FullName);
#else
                    file = new FileInfo(file.Directory.Parent.ToFile(file.Name).FullName);
#endif
                }
            }
        }

        public virtual LexicalCollection LoadHierarchy(INormalityComparer comparer)
        {
            Trace.WriteIf(Tracing.Is.TraceVerbose, string.Empty);
            var result = new LexicalCollection(comparer);
            foreach (var file in Hierarchy)
            {
                Load(result, file);
            }

            return result;
        }

        public virtual void Delete(Lexicon lexicon)
        {
            Trace.WriteIf(Tracing.Is.TraceVerbose, string.Empty);
            if (null == lexicon)
            {
                throw new ArgumentNullException("lexicon");
            }

            Location.Refresh();
            if (Location.Exists)
            {
                Location.Delete();
            }
        }

        public virtual Lexicon Load(INormalityComparer comparer)
        {
            Trace.WriteIf(Tracing.Is.TraceVerbose, string.Empty);
            var result = new Lexicon(comparer)
                             {
                                 Storage = this
                             };

            Load(result, Location);

            return result;
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "I trust the using statement.")]
        public virtual void Save(Lexicon lexicon)
        {
            Trace.WriteIf(Tracing.Is.TraceVerbose, string.Empty);
            if (null == lexicon)
            {
                throw new ArgumentNullException("lexicon");
            }

            using (var writers = new StreamWriterDictionary("CANONICAL,SYNONYMS")
                                     {
                                         Access = FileAccess.Write,
                                         Mode = FileMode.Create,
                                         Share = FileShare.None
                                     })
            {
#if NET20
                if (0 == IEnumerableExtensionMethods.Count(lexicon))
#else
                if (!lexicon.Any())
#endif
                {
                    writers.Item(Location.FullName).WriteLine(string.Empty);
                    return;
                }

#if NET20
                var items = new SortedList<string, LexicalItem>();
                foreach (var item in lexicon)
                {
                    items.Add(item.CanonicalForm, item);
                }

                foreach (var item in items)
                {
                    var synonyms = new SortedList<string, string>();
                    foreach (var synonym in item.Value.Synonyms)
                    {
                        synonyms.Add(synonym, synonym);
                    }

                    writers
                        .Item(Location.FullName)
                        .WriteLine(StringExtensionMethods.FormatWith(
                            "{0},{1}", 
                            CsvStringExtensionMethods.FormatCommaSeparatedValue(item.Value.CanonicalForm), 
                            CsvStringExtensionMethods.FormatCommaSeparatedValue(IEnumerableExtensionMethods.Concat(synonyms.Values, ';'))));
                }
#else
                foreach (var item in lexicon.OrderBy(x => x.CanonicalForm))
                {
                    writers
                        .Item(Location.FullName)
                        .WriteLine("{0},{1}".FormatWith(
                                                        item.CanonicalForm.FormatCommaSeparatedValue(),
                                                        item.Synonyms.OrderBy(x => x).Concat(';').FormatCommaSeparatedValue()));
                }

#endif
            }
        }

        private static void Load(LexicalCollection lexicon,
                                 FileInfo file)
        {
            foreach (var data in new CsvDataSheet(file))
            {
                var canonical = data["CANONICAL"];
                var item = lexicon[canonical] ?? lexicon.Add(canonical);
#if NET20
                foreach (var synonym in StringExtensionMethods.Split(data["SYNONYMS"], ';', StringSplitOptions.RemoveEmptyEntries))
#else
                foreach (var synonym in data["SYNONYMS"].Split(';', StringSplitOptions.RemoveEmptyEntries))
#endif
                {
                    item.Synonyms.Add(synonym);
                }
            }
        }
    }
}