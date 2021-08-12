using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using UnityEngine;
using Random = System.Random;

namespace CrazyPills
{
    public abstract class PillEvents
    {
        private static readonly Random Rand = new Random();

        public static Dictionary<int, Action<Player>> PillEffects = new Dictionary<int, Action<Player>>
        {
            {
                0, p =>
                {
                    if (Plugin.Instance.Config.ShowHints)
                        p.ShowHint("Seems that you'll be meeting an unfortunate fate...", 6f);
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
                    if (Plugin.Instance.Config.ShowHints)
                        p.ShowHint("You have been granted a gun and ammo from the pill genie", 8f);
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
                    if (Plugin.Instance.Config.ShowHints)
                        p.ShowHint("You've been granted invincibility for 20 seconds by pill genie", 8f);
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
                    if (!Plugin.Instance.Config.WarheadStatStop || Plugin.Instance.Config.WarheadStartStopChance > 0 && Plugin.Instance.Config.WarheadStartStopChance < 100 && Rand.Next(101) > Plugin.Instance.Config.WarheadStartStopChance)
                    {
                        Warhead.LeverStatus = !Warhead.LeverStatus;
                        if (Plugin.Instance.Config.ShowHints)
                            p.ShowHint(string.Format($"The magic pill genie has switched the nuke to {Warhead.LeverStatus.ToString().ToLower()}.", 8f));
                        return;
                    }

                    if (!(Plugin.Instance.Config.WarheadStartStopChance > 0 &&
                            Plugin.Instance.Config.WarheadStartStopChance < 100))
                    {
                        Log.Warn("Config value 'WarheadStartStopChance' must be greater than 0 and less than 100.");
                        return;
                    }
                    if (Warhead.IsInProgress)
                    {
                        Map.Broadcast(8, "The magic pill genie has stopped the warhead.", Broadcast.BroadcastFlags.Normal, false);
                        Warhead.Stop();
                        return;
                    }

                    Warhead.Start();
                    Map.Broadcast(8, "The magic pill genie has started the warhead.", Broadcast.BroadcastFlags.Normal, false);
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
                        if (!Plugin.Instance.Config.ShowHints) return;
                        p.ShowHint("the magic pill genie has replaced you with another.");
                        randDead.ShowHint("the magic pill genie has summoned you in place of another.");
                        return;
                    }

                    List<Player> alive = Player.List.Where(x => x.Team != Team.RIP && x != p && x.Role != RoleType.Scp079).ToList();
                    if (alive.Count == 0)
                        return;
                    Player randAlive = alive[Rand.Next(alive.Count)];
                    prePosition = p.Position;
                    p.Position = randAlive.Position;
                    randAlive.Position = prePosition;

                    if (!Plugin.Instance.Config.ShowHints) return;
                    p.ShowHint("the magic pill genie has switched you with another.", 8f);
                    randAlive.ShowHint("the magic pill genie has switched you with another.", 8f);
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
                    if (Plugin.Instance.Config.ShowHints)
                        p.ShowHint("the magic pill genie has promoted you!", 8f);
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

                    if (!Plugin.Instance.Config.ShowHints) return;
                    p.ShowHint("the magic pill genie has switched you with another.", 8f);
                    randAlive.ShowHint("the magic pill genie has switched you with another.", 8f);
                }
            }
        };
    }
}
