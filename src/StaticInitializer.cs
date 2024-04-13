using Verse;

namespace PrepareLanding
{
    [StaticConstructorOnStartup]
    internal static class StaticInitializer
    {
        static StaticInitializer()
        {
            PrepareLanding.Instance?.Initialize();
        }
    }
}
