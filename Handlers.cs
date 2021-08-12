using Exiled.API.Features;

namespace CrazyPills
{
    public class Handlers : PillEvents
    {
        public static void PillEffect(int num, Player p)
        {
            PillEffects[num](p);
        }
    }
}
