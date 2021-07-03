using System.ComponentModel;
using Exiled.API.Interfaces;

namespace CrazyPills
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Whether or not all players are guarenteed to spawn with pills.")]
        public bool SpawnWithPills { get; private set; } = true;

        [Description("Whether or not the pills can have a 1 in 15 in 10 chance of turning on the nuke or off.")]
        public bool WarheadStatStop { get; private set; } = true;
    }
}
