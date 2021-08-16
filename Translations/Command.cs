using System.ComponentModel;

namespace CrazyPills.Translations
{
    public class Command
    {
        [Description("Response when the user lacks the 'cp.pill' permission.")]
        public string MissingPermission { get; private set; } = "You do not have the permission to use this command. Missing command module: cp.pill.";

        [Description("Response when the user is a spectator.")]
        public string DeniedSpectator { get; private set; } = "This command cannot be used as a spectator.";

        [Description("Response when the user enters an invalid range for the command.")]
        public string InvalidRange { get; private set; } = "Accepted range is 1 through {Count}.";

        [Description("Response when the user enters invalid arguments for the command.")]
        public string IncorrectUse { get; private set; } = "Incorrect use of command. Either leave arguments empty to have a random pill effect occur or provide the number that corresponds with the event you are attempting. Accepted range is 1 through {Count}.";

        [Description("Response when the user types 'help' as an argument.")]
        public string Help { get; private set; } = "Provide a number based on which pill event you'd like to occur or leave arguments empty for a random event. Accepted range is 1 through {Count}.";

        [Description("Response when the command is succesful.")]
        public string Success { get; private set; } = "We do be poppin'";
    }
}
