using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MEC;
using UnityEngine;
using CrazyPills.Translations;
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
            FullPK,
            WarheadEvent,
            SwitchDead,
            Promote,
            Switch
        }

        public static List<PillEffectType> ActiveEnums() => Enum.GetValues(typeof(PillEffectType)).Cast<PillEffectType>().Where(e => Plugin.Instance.Config.PillEffects.Contains(e.ToString())).ToList();
        
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
                case PillEffectType.FullPK:
                    FullPK(p);
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
            Timing.CallDelayed(6f, () => p.Kill());
        }

        private static void Zombify(Player p)
        {
            RoleType preRole = p.Role;
            Vector3 prePosition = p.Position;
            p.DropItems();
            p.Role = RoleType.Scp0492;
            Timing.CallDelayed(0.4f, () => p.Position = prePosition);
            Timing.CallDelayed(30f, () =>
            {
                if (!p.IsAlive) return;
                prePosition = p.Position;
                p.Role = preRole;
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
            if (p.Role == RoleType.ClassD || p.Role == RoleType.Scientist)
            {
                p.AddItem(ItemType.GunCOM18);
                p.Ammo[ItemType.Ammo9x19] += 100;
                return;
            }

            if (p.Role != RoleType.NtfCaptain && p.Role != RoleType.NtfSergeant && p.Role != RoleType.NtfSpecialist)
            {
                p.AddItem(ItemType.GunE11SR);
                p.Ammo[ItemType.Ammo762x39] += 100;
                return;
            }

            p.AddItem(ItemType.GunLogicer);
            p.Ammo[ItemType.Ammo762x39] += 100;
        }

        private static void Goto(Player p)
        {
            List<Player> alive = Player.List.Where(x => x.Team != Team.RIP && x != p && x.Role != RoleType.Scp079).ToList();

            if (alive.Count == 0) return;

            p.Position = alive[Rand.Next(alive.Count)].Position;
        }

        private static void Combustion(Player p)
        {
            ExplosiveGrenade grenade = new ExplosiveGrenade(ItemType.SCP018, p)
            {
                FuseTime = 0.1f
            };
            grenade.SpawnActive(p.Position, p);
        }

        private static void Shrink(Player p)
        {
            p.Scale = new Vector3 { x = 0.4f, y = 0.4f, z = 0.4f };
            Timing.CallDelayed(30f, () => p.Scale = new Vector3 { x = 1f, y = 1f, z = 1f });
        }

        private static void Balls(Player p)
        {
            new ExplosiveGrenade(ItemType.SCP018).SpawnActive(p.Position, p);
            new ExplosiveGrenade(ItemType.SCP018).SpawnActive(p.Position, p);
            new ExplosiveGrenade(ItemType.SCP018).SpawnActive(p.Position, p);
            new ExplosiveGrenade(ItemType.SCP018).SpawnActive(p.Position, p);
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
            List<Player> alive = Player.List.Where(x => x.Team != Team.RIP && x != p && x.Role != RoleType.Scp079).ToList();
            if (alive.Count == 0)
                return;
            Player randAlive = alive[Rand.Next(alive.Count)];
            randAlive.Position = p.Position;
        }

        private static void FullPK(Player p)
        {
            p.DropItems();
            p.ClearInventory();
            for (byte i = 0; i < 8; i++)
                p.AddItem(ItemType.Painkillers);
        }

        private static void WarheadEvent(Player p)
        {
            if (Configs.WarheadStartStopChance > 0 && Configs.WarheadStartStopChance < 100 && Rand.Next(101) > Configs.WarheadStartStopChance)
            {
                Warhead.LeverStatus = !Warhead.LeverStatus;
                if (Configs.ShowHints)
                    p.ShowHint(string.Format(HintText.LeverSwitch.Replace("{LeverStatus}", Warhead.LeverStatus.ToString().ToLower()), 8f));
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
                List<ItemBase> preInv = new List<ItemBase>();

                preInv.AddRange(p.Inventory.UserInventory.Items.Values);
                prePosition = p.Position;
                RoleType preRole = p.Role;
                p.Role = RoleType.Spectator;
                randDead.Role = preRole;
                Timing.CallDelayed(0.2f, () =>
                {
                    randDead.Position = prePosition;
                    foreach (var item in preInv)
                        randDead.AddItem(item);
                });
                if (!Configs.ShowHints) return;
                p.ShowHint(HintText.Replacement["Replaced"], 8f);
                randDead.ShowHint(HintText.Replacement["Replacer"], 8f);
                return;
            }

            List<Player> alive = Player.List.Where(x => x.Team != Team.RIP && x != p && x.Role != RoleType.Scp079).ToList();
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
            switch (p.Role)
            {
                case RoleType.ClassD:
                    p.DropItems();
                    p.Role = RoleType.ChaosConscript;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                    break;
                case RoleType.ChaosConscript:
                    p.DropItems();
                    p.Role = RoleType.ChaosRifleman;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                    break;
                case RoleType.ChaosRifleman:
                    p.DropItems();
                    p.Role = RoleType.ChaosRepressor;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                    break;
                case RoleType.ChaosRepressor:
                    p.DropItems();
                    p.Role = RoleType.ChaosMarauder;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                    break;
                case RoleType.Scientist:
                    p.DropItems();
                    p.Role = RoleType.NtfSpecialist;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition); break;
                case RoleType.FacilityGuard:
                    p.DropItems();
                    p.Role = RoleType.NtfPrivate;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition); break;
                case RoleType.NtfPrivate:
                    p.DropItems();
                    p.Role = RoleType.NtfSergeant;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition); break;
                case RoleType.NtfSergeant:
                    p.DropItems();
                    p.Role = RoleType.NtfCaptain;
                    Timing.CallDelayed(0.4f, () => p.Position = prePosition); break;
                default:
                    p.DropItems();
                    p.AddItem(ItemType.KeycardO5); break;
            }
            if (Configs.ShowHints)
                p.ShowHint(HintText.Promotion, 8f);
        }

        private static void Switch(Player p)
        {
            List<Player> alive = Player.List.Where(x => x.Team != Team.RIP && x != p && x.Role != RoleType.Scp079).ToList();
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
