using System.ComponentModel;
using Exiled.API.Interfaces;
using CrazyPills.Translations;

namespace CrazyPills
{
    public class Translation : ITranslation
    {
        [Description("Translations for hints triggered by pill events.")]
        public HintTextTranslations Hints = new HintTextTranslations();

        [Description("Translations for broadcasts triggered by pill events.")]
        public BroadcastTextTranslations Broadcasts = new BroadcastTextTranslations();
    }
}
