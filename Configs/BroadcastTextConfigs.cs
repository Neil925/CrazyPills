using System.ComponentModel;

namespace CrazyPills.Configs
{
    public class BroadcastTextConfigs
    {
        [Description("Broadcast made when the pills result in the nuke being triggered.")]
        public string WarHeadEnabled = "The magic pill genie has started the warhead.";

        [Description("Broadcast made when the pills stop the nuke from going off.")]
        public string WarHeadDisabled = "The magic pill genie has stopped the warhead.";
    }
}
