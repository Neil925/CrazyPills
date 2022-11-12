using System;
using Exiled.API.Enums;
using Exiled.API.Features;

using PlayerE = Exiled.Events.Handlers.Player;
using SCP106E = Exiled.Events.Handlers.Scp106;

namespace CrazyPills
{
    public class Plugin : Plugin<Config, Translation>
    {
        public override string Author => "Neil";

        public override string Name => "Crazy Pills";

        public override string Prefix => "cp";

        public override Version RequiredExiledVersion => new Version(5, 3, 0);
        public override PluginPriority Priority => PluginPriority.Lowest;

        public static Plugin Instance;

        private EventHandlers _handler;

        public override void OnEnabled()
        {
            Instance = this;

            _handler = new EventHandlers();

            PlayerE.UsedItem += _handler.OnUsedItem;
            PlayerE.ChangingRole += _handler.OnChangingRole;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerE.UsedItem -= _handler.OnUsedItem;
            PlayerE.ChangingRole -= _handler.OnChangingRole;

            _handler = null;
            
            Instance = null;

            base.OnDisabled();
        }
    }
}
