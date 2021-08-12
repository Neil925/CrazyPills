using System.ComponentModel;
using CrazyPills.Configs;
using Exiled.API.Interfaces;
using Hints;

namespace CrazyPills
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Whether or not all players are guarenteed to spawn with pills.")]
        public bool SpawnWithPills { get; private set; } = true;

        [Description("Whether or not Pain Killer consumption has a small chance of turning the nuke on or off dependent on current state.")]
        public bool WarheadStartStop { get; private set; } = true;

        [Description("If the above is true, text displayed during Warhead broadcast events.")]
        public BroadcastTextConfigs NukeBroadcasts { get; private set; } = new BroadcastTextConfigs();

        [Description("If the above value is true, this dictates the percentage chance of the warhead starting/stopping with the event.")]
        public int WarheadStartStopChance { get; private set; } = 10;

        [Description("Whether or not to show a hint during certain Pain Killer consumption events.")]
        public bool ShowHints { get; private set; } = true;

        [Description("If the above is true, these are the hints displayed during certain events.")]
        public HintTextConfigs HintText { get; private set; } = new HintTextConfigs();
    }
}
