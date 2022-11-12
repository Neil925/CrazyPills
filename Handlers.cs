using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MEC;
using UnityEngine;
using CrazyPills.Translations;
using Exiled.API.Enums;
using Exiled.API.Features.Items;
using InventorySystem.Items;

using Random = System.Random;

namespace CrazyPills
{
    public class Handlers
    {
        private static readonly Random Rand = new Random();
        private static readonly Config Configs = Plugin.Instance.Config;
        private static readonly Hint HintText = Plugin.Instance.Translation.Hints;

        public enum PillEffectType
        {
            Kill,
            Zombify,
            FullHeal,
            GiveGun,
            Goto,
            Combustion,
            Shrink,
            Balls,
            Invincibility,
            Bring,
            FullPk,
            WarheadEvent,
            SwitchDead,
            Promote,
            Switch
        }

        public static List<PillEffectType> ActiveEnums() => Enum.GetValues(typeof(PillEffectType))
            .Cast<PillEffectType>().Where(e => Plugin.Instance.Config.PillEffects.Contains(e)).ToList();

        public static void RunPillEffect(PillEffectType type, Player p)
        {
            switch (type)
            {
                case PillEffectType.Kill:
                    Kill(p);
                    break;
                case PillEffectType.Zombify:
                    Zombify(p);
                    break;
                case PillEffectType.FullHeal:
                    FullHeal(p);
                    break;
                case PillEffectType.GiveGun:
                    GiveGun(p);
                    break;
                case PillEffectType.Goto:
                    Goto(p);
                    break;
                case PillEffectType.Combustion:
                    Combustion(p);
                    break;
                case PillEffectType.Shrink:
                    Shrink(p);
                    break;
                case PillEffectType.Balls:
                    Balls(p);
                    break;
                case PillEffectType.Invincibility:
                    Invincibility(p);
                    break;
                case PillEffectType.Bring:
                    Bring(p);
                    break;
                case PillEffectType.FullPk:
                    FullPk(p);
                    break;
                case PillEffectType.WarheadEvent:
                    WarheadEvent(p);
                    break;
                case PillEffectType.SwitchDead:
                    SwitchDead(p);
                    break;
                case PillEffectType.Promote:
                    Promote(p);
                    break;
                case PillEffectType.Switch:
                    Switch(p);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static void Kill(Player p)
        {
            if (Configs.ShowHints)
                p.ShowHint(HintText.DeathEvent, 8f);
            Timing.CallDelayed(6f, () => p.Kill(DamageType.Unknown));
        }

        private static void Zombify(Player p)
        {
            RoleType preRole = p.Role;
            Vector3 prePosition = p.Position;
            p.DropItems();
            p.Role.Type = RoleType.Scp0492;
            Timing.CallDelayed(0.4f, () => p.Position = prePosition);
            Timing.CallDelayed(30f, () =>
            {
                if (!p.IsAlive) return;
                prePosition = p.Position;
                p.Role.Type = preRole;
                Timing.CallDelayed(0.2f, () =>
                {
                    p.ClearInventory();
                    p.Position = prePosition;
                });
            });
        }

        private static void FullHeal(Player p)
        {
            p.Health = p.MaxHealth;
            p.ArtificialHealth = 200;
        }

        private static void GiveGun(Player p)
        {
            if (Configs.ShowHints)
                p.ShowHint(HintText.GunAndAmmo, 8f);

            Firearm gun;

            if (p.Items.Any(x => x.Type == ItemType.GunE11SR))
                gun = Item.Create(ItemType.GunLogicer) as Firearm;
            else if (p.Items.All(x => x.Type != ItemType.GunE11SR))
                gun = Item.Create(ItemType.GunE11SR) as Firearm;
            else if (p.Items.All(x => x.Type != ItemType.GunAK))
                gun = Item.Create(ItemType.GunAK) as Firearm;
            else if (p.Items.All(x => x.Type != ItemType.GunCrossvec))
                gun = Item.Create(ItemType.GunCrossvec) as Firearm;
            else if (p.Items.All(x => x.Type != ItemType.GunShotgun))
                gun = Item.Create(ItemType.GunShotgun) as Firearm;
            else if (p.Items.All(x => x.Type != ItemType.GunFSP9))
                gun = Item.Create(ItemType.GunFSP9) as Firearm;
            else if (p.Items.All(x => x.Type != ItemType.GunRevolver))
                gun = Item.Create(ItemType.GunRevolver) as Firearm;
            else
                gun = Item.Create(ItemType.GunCOM18) as Firearm;

            gun.Give(p);
            p.AddAmmo(gun.AmmoType, 100);
        }

        private static void Goto(Player p)
        {
            List<Player> alive = Player.List.Where(x =>
                x.Role.Team != Team.RIP && x != p && x.Role != RoleType.Scp079 &&
                (x.Role.Team != Team.SCP || Configs.AllowSCPTeleportation)).ToList();

            if (alive.Count == 0) return;

            p.Position = alive[Rand.Next(alive.Count)].Position;
        }

        private static void Combustion(Player p)
        {
            ExplosiveGrenade grenade = Item.Create(ItemType.GrenadeHE) as ExplosiveGrenade;
            grenade.FuseTime = 0.1f;

            grenade.SpawnActive(p.Position, p);
        }

        private static void Shrink(Player p)
        {
            p.Scale = new Vector3 { x = 0.4f, y = 0.4f, z = 0.4f };
            Timing.CallDelayed(30f, () => p.Scale = new Vector3 { x = 1f, y = 1f, z = 1f });
        }

        private static void Balls(Player p)
        {
            for (int i = 0; i < 5; i++)
                ((ExplosiveGrenade)Item.Create(ItemType.SCP018)).SpawnActive(p.Position, p);
        }

        private static void Invincibility(Player p)
        {
            if (Configs.ShowHints)
                p.ShowHint(HintText.Invincibility, 8f);
            p.IsGodModeEnabled = true;
            Timing.CallDelayed(20f, () => p.IsGodModeEnabled = false);
        }

        private static void Bring(Player p)
        {
            List<Player> alive = Player.List.Where(x =>
                x.Role.Team != Team.RIP && x != p && x.Role != RoleType.Scp079 &&
                (x.Role.Team != Team.SCP || Configs.AllowSCPTeleportation)).ToList();
            if (alive.Count == 0)
                return;
            Player randAlive = alive[Rand.Next(alive.Count)];
            randAlive.Position = p.Position;
        }

        private static void FullPk(Player p)
        {
            p.DropItems();
            p.ClearInventory();
            for (byte i = 0; i < 8; i++)
                p.AddItem(ItemType.Painkillers);
        }

        private static void WarheadEvent(Player p)
        {
            if (Configs.WarheadStartStopChance > 0 && Configs.WarheadStartStopChance < 100 &&
                Rand.Next(101) > Configs.WarheadStartStopChance)
            {
                Warhead.LeverStatus = !Warhead.LeverStatus;
                if (Configs.ShowHints)
                    p.ShowHint(string.Format(
                        HintText.LeverSwitch.Replace("{LeverStatus}", Warhead.LeverStatus.ToString().ToLower()), 8f));
                return;
            }

            if (!(Configs.WarheadStartStopChance > 0 && Configs.WarheadStartStopChance < 100))
            {
                Log.Warn("Config value 'WarheadStartStopChance' must be greater than 0 and less than 100.");
                return;
            }

            if (Warhead.IsInProgress)
            {
                Map.Broadcast(8, Plugin.Instance.Translation.Broadcasts.WarHeadDisabled);
                Warhead.Stop();
                return;
            }

            Warhead.Start();
            Map.Broadcast(8, Plugin.Instance.Translation.Broadcasts.WarHeadEnabled);
        }

        private static void SwitchDead(Player p)
        {
            List<Player> dead = Player.Get(Team.RIP).ToList();

            Vector3 prePosition;

            if (dead.Any())
            {
                Player randDead = dead[Rand.Next(dead.Count)];
                List<ItemType> preInv = new List<ItemType>();
                Dictionary<ItemType, ushort> preAmmo = p.Ammo;

                preInv.AddRange(p.Items.Select(x => x.Type));
                prePosition = p.Position;
                RoleType preRole = p.Role;
                
                p.Role.Type = RoleType.Spectator;
                randDead.Role.Type = preRole;
                Timing.CallDelayed(1f, () =>
                {
                    randDead.Position = prePosition;
                    foreach (var item in preInv)
                        randDead.AddItem(item);
                    foreach (var ammo in preAmmo) 
                        randDead.AddAmmo(ammo.Key, ammo.Value);
                });
                if (!Configs.ShowHints) return;
                p.ShowHint(HintText.Replacement["Replaced"], 8f);
                randDead.ShowHint(HintText.Replacement["Replacer"], 8f);
                return;
            }

            List<Player> alive = Player.List.Where(x =>
                x.Role.Team != Team.RIP && x != p && x.Role != RoleType.Scp079 &&
                (x.Role.Team != Team.SCP || Configs.AllowSCPTeleportation)).ToList();
            if (alive.Count == 0)
                return;
            Player randAlive = alive[Rand.Next(alive.Count)];
            prePosition = p.Position;
            p.Position = randAlive.Position;
            randAlive.Position = prePosition;

            if (!Configs.ShowHints) return;
            p.ShowHint(HintText.Switching, 8f);
            randAlive.ShowHint(HintText.Switching, 8f);
        }

        private static void Promote(Player p)
        {
            Vector3 prePosition = p.Position;
            switch (p.Role.Type)
            {
                case RoleType.ClassD:
                    p.DropItems();
                    p.Role.Type = RoleType.ChaosConscript;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                    break;
                case RoleType.ChaosConscript:
                    p.DropItems();
                    p.Role.Type = RoleType.ChaosRifleman;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                    break;
                case RoleType.ChaosRifleman:
                    p.DropItems();
                    p.Role.Type = RoleType.ChaosRepressor;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                    break;
                case RoleType.ChaosRepressor:
                    p.DropItems();
                    p.Role.Type = RoleType.ChaosMarauder;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                    break;
                case RoleType.Scientist:
                    p.DropItems();
                    p.Role.Type = RoleType.NtfSpecialist;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                    break;
                case RoleType.FacilityGuard:
                    p.DropItems();
                    p.Role.Type = RoleType.NtfPrivate;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                    break;
                case RoleType.NtfPrivate:
                    p.DropItems();
                    p.Role.Type = RoleType.NtfSergeant;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                    break;
                case RoleType.NtfSergeant:
                    p.DropItems();
                    p.Role.Type = RoleType.NtfCaptain;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                    break;
                default:
                    p.DropItems();
                    p.AddItem(ItemType.KeycardO5);
                    break;
            }

            if (Configs.ShowHints)
                p.ShowHint(HintText.Promotion, 8f);
        }

        private static void Switch(Player p)
        {
            List<Player> alive = Player.List.Where(x =>
                x.Role.Team != Team.RIP && x != p && x.Role != RoleType.Scp079 &&
                (x.Role.Team != Team.SCP || Configs.AllowSCPTeleportation)).ToList();
            if (alive.Count == 0)
                return;
            
            Player randAlive = alive[Rand.Next(alive.Count)];
            (p.Position, randAlive.Position) = (randAlive.Position, p.Position);

            if (!Configs.ShowHints) return;
            p.ShowHint(HintText.Switching, 8f);
            randAlive.ShowHint(HintText.Switching, 8f);
        }
    }
}