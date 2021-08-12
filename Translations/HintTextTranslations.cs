using System.Collections.Generic;
using System.ComponentModel;

namespace CrazyPills.Translations
{
    public class HintTextTranslations
    {
        [Description("Hint displayed upon the death event.")]
        public string DeathEvent = "Seems that you'll be meeting an unfortunate fate...";

        [Description("Hint displayed upon being given a gun and ammo.")]
        public string GunAndAmmo = "You have been granted a gun and ammo from the pill genie.";

        [Description("Text displayed upon being granted invincibility.")]
        public string Invincibility = "You've been granted invincibility for 20 seconds by pill genie.";

        [Description("Hint displayed upon being replaced by a spectator.")]
        public Dictionary<string, string> Replacement = new Dictionary<string, string>
        {
            {
                "Replaced", "The magic pill genie has replaced you with another."
            },
            {
                "Replacer", "The magic pill genie has summoned you in place of another."
            }
        };

        [Description("Hint displayed upon exchanging positions with another player.")]
        public string Switching = "The magic pill genie has switched you with another.";

        [Description("Hint displayed upon the switching of the Warhead Silo's lever state.")]
        public string LeverSwitch = "The magic pill genie has switched the nuke to {LeverStatus}.";

        [Description("Hint displayed upon promotion of a player.")]
        public string Promotion = "The magic pill genie has promoted you!";
    }
}
