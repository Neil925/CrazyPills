using System;
using System.Collections.Generic;
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

        public override Version Version => new Version(2, 4, 1);

        public override Version RequiredExiledVersion => new Version(3, 0, 0);

        public static Plugin Instance;

        public List<Player> Invincible = new List<Player>();

        private EventHandlers _handler;

        public override void OnEnabled()
        {
            Instance = this;

            _handler = new EventHandlers();

            PlayerE.ItemUsed += _handler.OnItemUsed;
            PlayerE.Hurting += _handler.OnHurting;
            PlayerE.Spawning += _handler.OnSpawning;
            SCP106E.Teleporting += _handler.OnTeleporting;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerE.ItemUsed -= _handler.OnItemUsed;
            PlayerE.Hurting -= _handler.OnHurting;
            PlayerE.Spawning -= _handler.OnSpawning;
            SCP106E.Teleporting -= _handler.OnTeleporting;

            _handler = null;
            
            Instance = null;

            base.OnDisabled();
        }

    }
}
