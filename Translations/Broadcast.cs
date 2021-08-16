using System.ComponentModel;

namespace CrazyPills.Translations
{
    public class Broadcast
    {
        [Description("Broadcast made when the pills result in the nuke being triggered.")]
        public string WarHeadEnabled { get; private set; } = "The magic pill genie has started the warhead.";

        [Description("Broadcast made when the pills stop the nuke from going off.")]
        public string WarHeadDisabled { get; private set; } = "The magic pill genie has stopped the warhead.";
    }
}
