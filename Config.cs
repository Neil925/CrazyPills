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
        public List<string> PillEffects { get; private set; } = new List<string>
        {
            PillEffectType.Kill.ToString(),
            PillEffectType.Zombify.ToString(),
            PillEffectType.FullHeal.ToString(),
            PillEffectType.GiveGun.ToString(),
            PillEffectType.Goto.ToString(),
            PillEffectType.Combustion.ToString(),
            PillEffectType.Shrink.ToString(),
            PillEffectType.Balls.ToString(),
            PillEffectType.Invincibility.ToString(),
            PillEffectType.Bring.ToString(),
            PillEffectType.FullPK.ToString(),
            PillEffectType.WarheadEvent.ToString(),
            PillEffectType.SwitchDead.ToString(),
            PillEffectType.Promote.ToString(),
            PillEffectType.Switch.ToString()
        };

        [Description("If the above value is true, this dictates the percentage chance of the warhead starting/stopping with the event.")]
        public int WarheadStartStopChance { get; private set; } = 10;

        [Description("Whether or not to show a hint during certain Pain Killer consumption events.")]
        public bool ShowHints { get; private set; } = true;
    }
}
