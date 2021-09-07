using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;

using Timing = MEC.Timing;

namespace CrazyPills
{
    internal class EventHandlers
    {
        private readonly System.Random _rand = new System.Random();

        internal void OnItemUsed(UsedItemEventArgs ev)
        {
            if (ev.Item.Type != ItemType.Painkillers) return;

            var num = _rand.Next(PillEvents.PillEffects.Count - 1);
            Timing.CallDelayed(3f, () => Handlers.PillEffect(num, ev.Player));
        }

        internal void OnHurting(HurtingEventArgs ev)
        {
            if (Plugin.Instance.Invincible.Contains(ev.Target))
                ev.IsAllowed = false;
        }

        internal void OnSpawning(SpawningEventArgs ev)
        {
            if (ev.Player.IsHuman && Plugin.Instance.Config.SpawnWithPills)
                Timing.CallDelayed(0.5f, () => ev.Player.AddItem(ItemType.Painkillers));
        }

        internal void OnTeleporting(TeleportingEventArgs ev)
        {
            if (Plugin.Instance.Invincible.Contains(ev.Player))
                ev.IsAllowed = false;
        }
    }
}
