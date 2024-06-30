namespace WhenFresh.Utilities.Diagnostics;

using System.Diagnostics;

internal static class Tracing
{
    internal static TraceSwitch Is => new("Cavity.Domain", string.Empty);
}