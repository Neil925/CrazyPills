using System;
using System.Collections.Generic;
using Exiled.API.Features;

using PlayerE = Exiled.Events.Handlers.Player;
using SCP106E = Exiled.Events.Handlers.Scp106;

namespace CrazyPills
{
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "Neil";

        public override string Name { get; } = "Crazy Pills";

        public override string Prefix { get; } = "cp";

        public override Version Version { get; } = new Version(1, 4, 0);

        public override Version RequiredExiledVersion { get; } = new Version(2, 12, 0);

        public static Plugin Instance;

        public List<Player> Invincible = new List<Player>();

        private EventHandlers _handler;

        public override void OnEnabled()
        {
            Instance = this;

            _handler = new EventHandlers();

            PlayerE.UsingMedicalItem += _handler.OnUsingMedicalItem;
            PlayerE.Hurting += _handler.OnHurting;
            PlayerE.Spawning += _handler.OnSpawning;
            SCP106E.Teleporting += _handler.OnTeleporting;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerE.UsingMedicalItem -= _handler.OnUsingMedicalItem;
            PlayerE.Hurting -= _handler.OnHurting;
            PlayerE.Spawning -= _handler.OnSpawning;
            SCP106E.Teleporting -= _handler.OnTeleporting;

            _handler = null;
            
            Instance = null;

            base.OnDisabled();
        }

    }
}
