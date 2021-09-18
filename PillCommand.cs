using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using CrazyPills.Translations;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;

namespace CrazyPills
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PillCommand : ICommand
    {
        public string Command => "pill";

        public string[] Aliases => new[] { "pills" };

        public string Description => "Instantly uses a pill.";

        public Random Rand = new Random();
        public Command Translations = Plugin.Instance.Translation.Command;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(((PlayerCommandSender)sender).ReferenceHub);

            if (!player.CheckPermission("cp.pill"))
            {
                response = Translations.MissingPermission;
                return false;
            }

            if (!player.IsAlive)
            {
                response = Translations.DeniedSpectator;
                return false;
            }

            int num;

            if (arguments.Count == 0)
            {
                List<Handlers.PillEffectType> pillTypes = Handlers.ActiveEnums();

                num = Rand.Next(pillTypes.Count);
                Handlers.RunPillEffect(pillTypes[num], player);

                response = Translations.Success;
                return true;
            }

            if (Enum.GetValues(typeof(Handlers.PillEffectType)).Cast<Handlers.PillEffectType>().Any(e => string.Equals(e.ToString(), arguments.At(0), StringComparison.CurrentCultureIgnoreCase)))
            {
                Handlers.RunPillEffect(Enum.GetValues(typeof(Handlers.PillEffectType)).Cast<Handlers.PillEffectType>().FirstOrDefault(e => string.Equals(e.ToString(), arguments.At(0), StringComparison.CurrentCultureIgnoreCase)), player);

                response = Translations.Success;
                return true;
            }

            int pillTypesCount = Enum.GetValues(typeof(Handlers.PillEffectType)).Length;

            if (int.TryParse(arguments.At(0), out num) && num <= pillTypesCount && num >= 1)
            {
                Handlers.RunPillEffect((Handlers.PillEffectType)num, player);

                response = Translations.Success;
                return true;
            }

            if (arguments.At(0) == "help")
            {
                response = Translations.Help.Replace("{Count}", pillTypesCount.ToString());
                return false;
            }

            response = Translations.IncorrectUse.Replace("{Count}", pillTypesCount.ToString());
            return false;
        }
    }
}