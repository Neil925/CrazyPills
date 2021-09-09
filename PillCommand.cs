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
            var p = Player.Get(((PlayerCommandSender)sender).ReferenceHub);

            if (!p.CheckPermission("cp.pill"))
            {
                response = Translations.MissingPermission;
                return false;
            }

            if (!p.IsAlive)
            {
                response = Translations.DeniedSpectator;
                return false;
            }

            int num;

            if (arguments.Count == 0)
            {
                List<Handlers.PillEffectType>  pillTypes = Handlers.ActiveEnums();

                num = Rand.Next(pillTypes.Count);
                Handlers.RunPillEffect(pillTypes[num], p);
            }
            else
            {
                int pillTypesCount = Enum.GetValues(typeof(Handlers.PillEffectType)).Length;

                if (int.TryParse(arguments.At(0), out num))
                {
                    if (num > pillTypesCount || num < 1)
                    {
                        response = Translations.InvalidRange.Replace("{Count}", pillTypesCount.ToString());
                        return false;
                    }

                    Handlers.RunPillEffect((Handlers.PillEffectType)num, p);
                }
                else
                {
                    if (arguments.At(0) != "help")
                    {
                        response = Translations.IncorrectUse.Replace("{Count}", pillTypesCount.ToString());
                        return false;
                    }

                    response = Translations.Help.Replace("{Count}", pillTypesCount.ToString());
                    return true;
                }
            }

            response = Translations.Success;
            return true;
        }
    }
}