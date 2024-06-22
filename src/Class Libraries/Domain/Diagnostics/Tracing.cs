namespace WhenFresh.Utilities.Domain.Diagnostics
{
    using System.Diagnostics;

    internal static class Tracing
    {
        internal static TraceSwitch Is
        {
            get
            {
                return new TraceSwitch("Cavity.Domain", string.Empty);
            }
        }
    }
}