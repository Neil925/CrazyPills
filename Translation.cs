using System.ComponentModel;
using Exiled.API.Interfaces;
using CrazyPills.Translations;

namespace CrazyPills
{
    public class Translation : ITranslation
    {
        [Description("Translations for hints triggered by pill events.")]
        public Hint Hints { get; private set; } = new Hint();

        [Description("Translations for broadcasts triggered by pill events.")]
        public Translations.Broadcast Broadcasts { get; private set; } = new Translations.Broadcast();

        public Command Command { get; private set; } = new Command();
    }
}
