using System;
using System.Collections.Generic;
using Exiled.Events.EventArgs;

using Timing = MEC.Timing;

namespace CrazyPills
{
    internal class EventHandlers
    {
        private readonly Random _rand = new Random();

        internal void OnItemUsed(UsedItemEventArgs ev)
        {
            if (ev.Item.Type != ItemType.Painkillers) return;

            List<Handlers.PillEffectType> pillTypes = Handlers.ActiveEnums();
            int num = _rand.Next(pillTypes.Count);

            Handlers.RunPillEffect(pillTypes[num], ev.Player);
        }

        internal void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player.IsHuman && Plugin.Instance.Config.SpawnWithPills)
                ev.Items.Add(ItemType.Painkillers);
        }
    }
}
