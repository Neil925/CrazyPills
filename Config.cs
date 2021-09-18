using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;
using static CrazyPills.Handlers;

namespace CrazyPills
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Whether or not all players are guarenteed to spawn with pills.")]
        public bool SpawnWithPills { get; private set; } = true;

        [Description("A list of possible pill effects that are allowed to occur.")]
        public List<PillEffectType> PillEffects { get; private set; } = new List<PillEffectType>
        {
            PillEffectType.Kill,
            PillEffectType.Zombify,
            PillEffectType.FullHeal,
            PillEffectType.GiveGun,
            PillEffectType.Goto,
            PillEffectType.Combustion,
            PillEffectType.Shrink,
            PillEffectType.Balls,
            PillEffectType.Invincibility,
            PillEffectType.Bring,
            PillEffectType.FullPK,
            PillEffectType.WarheadEvent,
            PillEffectType.SwitchDead,
            PillEffectType.Promote,
            PillEffectType.Switch
        };

        [Description("Whether or not SCPs are included in teleportation events.")]
        public bool AllowSCPTeleportation { get; private set; } = true;

        [Description("If the above value is true, this dictates the percentage chance of the warhead starting/stopping with the event.")]
        public int WarheadStartStopChance { get; private set; } = 10;

        [Description("Whether or not to show a hint during certain Pain Killer consumption events.")]
        public bool ShowHints { get; private set; } = true;
    }
}
