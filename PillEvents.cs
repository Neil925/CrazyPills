using System;
using System.Collections.Generic;
using System.Linq;
using CrazyPills.Configs;
using Exiled.API.Enums;
using Exiled.API.Features;
using Hints;
using MEC;
using UnityEngine;
using Random = System.Random;

namespace CrazyPills
{
    public abstract class PillEvents
    {
        private static readonly Random Rand = new Random();
        private static readonly Config Configs = Plugin.Instance.Config;
        private static readonly HintTextConfigs HintText = Configs.HintText;

        public static Dictionary<int, Action<Player>> PillEffects = new Dictionary<int, Action<Player>>
        {
            {
                0, p =>
                {
                    if (Configs.ShowHints)
                        p.ShowHint(HintText.DeathEvent, 8f);
                    Timing.CallDelayed(6f, () => p.Kill());
                }
            },
            {
                1, p =>
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
                            p.Inventory.Clear();
                            p.Position = prePosition;
                        });
                    });
                }
            },
            {
                2, p =>
                {
                    p.Health = p.MaxHealth;
                    p.ArtificialHealth = 200f;
                }
            },
            {
                3, p =>
                {
                    if (Configs.ShowHints)
                        p.ShowHint(HintText.GunAndAmmo, 8f);
                    if (p.Role == RoleType.ClassD || p.Role == RoleType.Scientist)
                    {
                        p.Inventory.AddNewItem(ItemType.GunUSP);
                        p.Ammo[(int)AmmoType.Nato9] += 100;
                        return;
                    }

                    if (p.Role != RoleType.NtfLieutenant && p.Role != RoleType.NtfScientist)
                    {
                        p.Inventory.AddNewItem(ItemType.GunE11SR);
                        p.Ammo[(int)AmmoType.Nato556] += 100;
                        return;
                    }

                    p.Inventory.AddNewItem(ItemType.GunLogicer);
                    p.Ammo[(int)AmmoType.Nato762] += 100;
                }
            },
            {
                4, p =>
                {
                        List<Player> alive = Player.List.Where(x => x.Team != Team.RIP && x != p && x.Role != RoleType.Scp079).ToList();
                        if (alive.Count == 0)
                            return;
                        p.Position = alive[Rand.Next(alive.Count)].Position;
                }
            },
            {
                5, p => Map.SpawnGrenade(p.Position, fuseTime: 0.1f, player: p)
            },
            {
                6, p =>
                {
                    p.Scale = new Vector3 { x = 0.4f, y = 0.4f, z = 0.4f };
                    Timing.CallDelayed(30f, () => p.Scale = new Vector3 { x = 1f, y = 1f, z = 1f });
                }
            },
            {
                7, p =>
                {
                    Map.SpawnGrenade(p.Position, GrenadeType.Scp018, 7f, player: p);
                    Map.SpawnGrenade(p.Position, GrenadeType.Scp018, 7f, player: p);
                    Map.SpawnGrenade(p.Position, GrenadeType.Scp018, 7f, player: p);
                    Map.SpawnGrenade(p.Position, GrenadeType.Scp018, 7f, player: p);
                }
            },
            {
                8, p =>
                {
                    if (Configs.ShowHints)
                        p.ShowHint(HintText.Invincibility, 8f);
                    Plugin.Instance.Invincible.Add(p);
                    Timing.CallDelayed(20f, () => Plugin.Instance.Invincible.Remove(p));
                }
            },
            {
                9, p =>
                {
                    List<Player> alive = Player.List.Where(x => x.Team != Team.RIP && x != p && x.Role != RoleType.Scp079).ToList();
                    if (alive.Count == 0)
                        return;
                    Player randAlive = alive[Rand.Next(alive.Count)];
                    p.Position = randAlive.Position;
                }
            },
            {
                10, p =>
                {
                    p.DropItems();
                    p.Inventory.items.Clear();
                    for (byte i = 0; i < 8; i++)
                        p.Inventory.AddNewItem(ItemType.Painkillers);
                }
            },
            { 11, p =>
                {
                    if (!Configs.WarheadStartStop || Configs.WarheadStartStopChance > 0 && Configs.WarheadStartStopChance < 100 && Rand.Next(101) > Configs.WarheadStartStopChance)
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
                        Map.Broadcast(8, Configs.NukeBroadcasts.WarHeadDisabled, Broadcast.BroadcastFlags.Normal, false);
                        Warhead.Stop();
                        return;
                    }

                    Warhead.Start();
                    Map.Broadcast(8, Configs.NukeBroadcasts.WarHeadEnabled, Broadcast.BroadcastFlags.Normal, false);
                }
            },
            {
                12, p =>
                {
                    List<Player> dead = Player.Get(Team.RIP).ToList();

                    Vector3 prePosition;

                    if (dead.Any())
                    {
                        Player randDead = dead[Rand.Next(dead.Count)];
                        List<Inventory.SyncItemInfo> preInv = new List<Inventory.SyncItemInfo>();
                        preInv.AddRange(p.Inventory.items);
                        prePosition = p.Position;
                        RoleType preRole = p.Role;
                        p.Role = RoleType.Spectator;
                        randDead.Role = preRole;
                        Timing.CallDelayed(0.2f, () =>
                        {
                            randDead.Position = prePosition;
                            randDead.ResetInventory(preInv);
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
            },
            {
                13, p =>
                {
                    Vector3 prePosition = p.Position;
                    switch (p.Role)
                    {
                        case RoleType.ClassD:
                            p.DropItems();
                            p.Role = RoleType.ChaosInsurgency;
                            Timing.CallDelayed(0.4f, () => p.Position = prePosition);
                            break;
                        case RoleType.Scientist:
                            p.DropItems();
                            p.Role = RoleType.NtfScientist;
                            Timing.CallDelayed(0.4f, () => p.Position = prePosition); break;
                        case RoleType.FacilityGuard:
                            p.DropItems();
                            p.Role = RoleType.NtfCadet;
                            Timing.CallDelayed(0.4f, () => p.Position = prePosition); break;
                        case RoleType.NtfCadet:
                            p.DropItems();
                            p.Role = RoleType.NtfLieutenant;
                            Timing.CallDelayed(0.4f, () => p.Position = prePosition); break;
                        case RoleType.NtfLieutenant:
                            p.DropItems();
                            p.Role = RoleType.NtfCommander;
                            Timing.CallDelayed(0.4f, () => p.Position = prePosition); break;
                        default:
                            p.DropItems();
                            p.Inventory.AddNewItem(ItemType.KeycardO5); break;
                    }
                    if (Configs.ShowHints)
                        p.ShowHint(HintText.Promotion, 8f);
                }
            },
            {
                14, p =>
                {
                    List<Player> alive = Player.List.Where(x => x.Team != Team.RIP && x != p && x.Role != RoleType.Scp079).ToList();
                    if (alive.Count == 0)
                        return;
                    Player randAlive = alive[Rand.Next(alive.Count)];
                    Vector3 prePosition = p.Position;
                    p.Position = randAlive.Position;
                    randAlive.Position = prePosition;

                    if (!Configs.ShowHints) return;
                    p.ShowHint(HintText.Switching, 8f);
                    randAlive.ShowHint(HintText.Switching, 8f);
                }
            }
        };
    }
}
