using System;
using System.Reflection;

[assembly: CLSCompliant(false)]
[assembly: AssemblyDefaultAlias("Cavity.Domain.Facts.dll")]
[assembly: AssemblyTitle("Cavity.Domain.Facts.dll")]

#if (DEBUG)

[assembly: AssemblyDescription("Cavity : Domain Facts Library (Debug)")]

#else

[assembly: AssemblyDescription("Cavity : Domain Facts Library (Release)")]

#endif