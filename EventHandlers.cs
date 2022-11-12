using System;
using System.Collections.Generic;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs;

namespace CrazyPills
{
    internal class EventHandlers
    {
        private readonly Random _rand = new Random();

        internal void OnUsedItem(UsedItemEventArgs ev)
        {
            if (ev.Item.Type != ItemType.Painkillers) return;

            List<Handlers.PillEffectType> pillTypes = Handlers.ActiveEnums();
            int num = _rand.Next(pillTypes.Count);

            Handlers.RunPillEffect(pillTypes[num], ev.Player);
        }

        internal void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole != RoleType.None && ev.NewRole != RoleType.Spectator && ev.NewRole != RoleType.Tutorial && ev.NewRole.GetTeam() != Team.SCP && Plugin.Instance.Config.SpawnWithPills)
                ev.Items.Add(ItemType.Painkillers);
        }
    }
}