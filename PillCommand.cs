using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;

namespace CrazyPills
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PillCommand : ICommand
    {
        public string Command { get; } = "pill";

        public string[] Aliases { get; } = { "pills" };

        public string Description { get; } = "Instantly uses a pill.";

        public Random Rand = new Random();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var p = Player.Get(((PlayerCommandSender)sender).ReferenceHub);

            if (!p.CheckPermission("cp.pill"))
            {
                response = "You do not have the permission to use this command. Missing command module: cp.pill.";
                return false;
            }

            if (!p.IsAlive)
            {
                response = "This command cannot be used as a spectator.";
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
                        response = $"Accepted range is 1 through {PillEvents.PillEffects.Count}.";
                        return false;
                    }

                    Handlers.PillEffect(num - 1, p);
                }
                else
                {
                    if (arguments.At(0) != "help")
                    {
                        response = $"Incorrect use of command. Either leave arguments empty to have a random pill effect occur or provide the number that corresponds with the event you are attempting. Accepted range is 1 through {PillEvents.PillEffects.Count}.";
                        return false;
                    }

                    response = $"Provide a number based on which pill event you'd like to occur or leave arguments empty for a random event. Accepted range is 1 through {PillEvents.PillEffects.Count}.";
                    return true;
                }
            }

            response = "We do be poppin'";
            return true;
        }
    }
}