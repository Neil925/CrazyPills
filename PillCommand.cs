using System;
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

        public string[] Aliases { get; } = { "pills" };

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
                num = Rand.Next(PillEvents.PillEffects.Count - 1);
                Handlers.PillEffect(num, p);
            }
            else
            {
                if (int.TryParse(arguments.At(0), out num))
                {
                    if (num > PillEvents.PillEffects.Count || num < 1)
                    {
                        response = Translations.InvalidRange.Replace("{Count}", PillEvents.PillEffects.Count.ToString());
                        return false;
                    }

                    Handlers.PillEffect(num - 1, p);
                }
                else
                {
                    if (arguments.At(0) != "help")
                    {
                        response = Translations.IncorrectUse.Replace("{Count}", PillEvents.PillEffects.Count.ToString());
                        return false;
                    }

                    response = Translations.Help.Replace("{Count}", PillEvents.PillEffects.Count.ToString());
                    return true;
                }
            }

            response = Translations.Success;
            return true;
        }
    }
}