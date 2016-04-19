using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[assembly: CLSCompliant(true)]
[assembly: AssemblyDefaultAlias("Cavity.Domain.dll")]
[assembly: AssemblyTitle("Cavity.Domain.dll")]

#if (DEBUG)

[assembly: AssemblyDescription("Cavity : Domain Library (Debug)")]

#else

[assembly: AssemblyDescription("Cavity : Domain Library (Release)")]

#endif

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Cavity", Justification = "This is a root namespace.")]