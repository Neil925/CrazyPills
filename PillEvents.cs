using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using UnityEngine;
using Random = System.Random;

namespace CrazyPills
{
    public abstract class PillEvents
    {
        public static Player Ply;

        private static readonly Random Rand = new Random();

        public static Dictionary<int, Action> PillEffects = new Dictionary<int, Action>
        {
            {
                0, () =>
                {
                    if (Plugin.Instance.Config.ShowHints)
                        Ply.ShowHint("Seems that you'll be meeting an unfortunate fate...", 6f);
                    Timing.CallDelayed(6f, () => Ply.Kill());
                }
            },
            {
                1, () =>
                {
                    RoleType preRole = Ply.Role;
                    Vector3 prePosition = Ply.Position;
                    Ply.DropItems();
                    Ply.Role = RoleType.Scp0492;
                    Timing.CallDelayed(0.4f, () => Ply.Position = prePosition);
                    Timing.CallDelayed(30f, () =>
                    {
                        if (!Ply.IsAlive) return;
                        prePosition = Ply.Position;
                        Ply.Role = preRole;
                        Timing.CallDelayed(0.2f, () =>
                        {
                            Ply.Inventory.Clear();
                            Ply.Position = prePosition;
                        });
                    });
                }
            },
            {
                2, () =>
                {
                    Ply.Health = Ply.MaxHealth;
                    Ply.ArtificialHealth = 200f;
                }
            },
            {
                3, () =>
                {
                    if (Plugin.Instance.Config.ShowHints)
                        Ply.ShowHint("You have been granted a gun and ammo from the pill genie", 8f);
                    if (Ply.Role == RoleType.ClassD || Ply.Role == RoleType.Scientist)
                    {
                        Ply.Inventory.AddNewItem(ItemType.GunUSP);
                        Ply.Ammo[(int)AmmoType.Nato9] += 100;
                        return;
                    }

                    if (Ply.Role != RoleType.NtfLieutenant && Ply.Role != RoleType.NtfScientist)
                    {
                        Ply.Inventory.AddNewItem(ItemType.GunE11SR);
                        Ply.Ammo[(int)AmmoType.Nato556] += 100;
                        return;
                    }

                    Ply.Inventory.AddNewItem(ItemType.GunLogicer);
                    Ply.Ammo[(int)AmmoType.Nato762] += 100;
                }
            },
            {
                4, () =>
                {
                        List<Player> alive = Player.List.Where(x => x.Team != Team.RIP && x != Ply && x.Role != RoleType.Scp079).ToList();
                        if (alive.Count == 0)
                            return;
                        Ply.Position = alive[Rand.Next(alive.Count)].Position;
                }
            },
            {
                5, () => Handlers.SpawnGrenade(Ply.Position, Vector3.zero, Ply, 0.1f)
            },
            {
                6, () =>
                {
                    Ply.Scale = new Vector3 { x = 0.4f, y = 0.4f, z = 0.4f };
                    Timing.CallDelayed(30f, () => Ply.Scale = new Vector3 { x = 1f, y = 1f, z = 1f });
                }
            },
            {
                7, () =>
                {
                    Handlers.SpawnGrenade(Ply.Position, Vector3.zero, Ply, 7f, GrenadeType.Scp018);
                    Handlers.SpawnGrenade(Ply.Position, Vector3.zero, Ply, 7f, GrenadeType.Scp018);
                    Handlers.SpawnGrenade(Ply.Position, Vector3.zero, Ply, 7f, GrenadeType.Scp018);
                    Handlers.SpawnGrenade(Ply.Position, Vector3.zero, Ply, 7f, GrenadeType.Scp018);
                }
            },
            {
                8, () =>
                {
                    if (Plugin.Instance.Config.ShowHints)
                        Ply.ShowHint("You've been granted invincibility for 20 seconds by pill genie", 8f);
                    Plugin.Instance.Invincible.Add(Ply);
                    Timing.CallDelayed(20f, () => Plugin.Instance.Invincible.Remove(Ply));
                }
            },
            {
                9, () =>
                {
                    List<Player> alive = Player.List.Where(x => x.Team != Team.RIP && x != Ply && x.Role != RoleType.Scp079).ToList();
                    if (alive.Count == 0)
                        return;
                    Player randAlive = alive[Rand.Next(alive.Count)];
                    Ply.Position = randAlive.Position;
                }
            },
            {
                10, () =>
                {
                    Ply.DropItems();
                    Ply.Inventory.items.Clear();
                    for (byte i = 0; i < 8; i++)
                        Ply.Inventory.AddNewItem(ItemType.Painkillers);
                }
            },
            { 11, () =>
                {
                    if (!Plugin.Instance.Config.WarheadStatStop || Plugin.Instance.Config.WarheadStartStopChance > 0 && Plugin.Instance.Config.WarheadStartStopChance < 100 && Rand.Next(101) > Plugin.Instance.Config.WarheadStartStopChance)
                    {
                        Warhead.LeverStatus = !Warhead.LeverStatus;
                        if (Plugin.Instance.Config.ShowHints)
                            Ply.ShowHint(string.Format($"The magic pill genie has switched the nuke to {Warhead.LeverStatus.ToString().ToLower()}.", 8f));
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
                        Map.Broadcast(8, "The magic pill genie has stopped the warhead.");
                        Warhead.Stop();
                        return;
                    }

                    Warhead.Start();
                    Map.Broadcast(8, "The magic pill genie has started the warhead.");
                }
            },
            {
                12, () =>
                {
                    List<Player> dead = Player.Get(Team.RIP).ToList();

                    Vector3 prePosition;

                    if (dead.Any())
                    {
                        Player randDead = dead[Rand.Next(dead.Count)];
                        List<Inventory.SyncItemInfo> preInv = new List<Inventory.SyncItemInfo>();
                        preInv.AddRange(Ply.Inventory.items);
                        prePosition = Ply.Position;
                        RoleType preRole = Ply.Role;
                        Ply.Role = RoleType.Spectator;
                        randDead.Role = preRole;
                        Timing.CallDelayed(0.2f, () =>
                        {
                            randDead.Position = prePosition;
                            randDead.ResetInventory(preInv);
                        });
                        if (!Plugin.Instance.Config.ShowHints) return;
                        Ply.ShowHint("the magic pill genie has replaced you with another.");
                        randDead.ShowHint("the magic pill genie has summoned you in place of another.");
                        return;
                    }

                    List<Player> alive = Player.List.Where(x => x.Team != Team.RIP && x != Ply && x.Role != RoleType.Scp079).ToList();
                    if (alive.Count == 0)
                        return;
                    Player randAlive = alive[Rand.Next(alive.Count)];
                    prePosition = Ply.Position;
                    Ply.Position = randAlive.Position;
                    randAlive.Position = prePosition;

                    if (!Plugin.Instance.Config.ShowHints) return;
                    Ply.ShowHint("the magic pill genie has switched you with another.", 8f);
                    randAlive.ShowHint("the magic pill genie has switched you with another.", 8f);
                }
            },
            {
                13, () =>
                {
                    Vector3 prePosition = Ply.Position;
                    switch (Ply.Role)
                    {
                        case RoleType.ClassD:
                            Ply.DropItems();
                            Ply.Role = RoleType.ChaosInsurgency;
                            Timing.CallDelayed(0.4f, () => Ply.Position = prePosition);
                            break;
                        case RoleType.Scientist:
                            Ply.DropItems();
                            Ply.Role = RoleType.NtfScientist;
                            Timing.CallDelayed(0.4f, () => Ply.Position = prePosition); break;
                        case RoleType.FacilityGuard:
                            Ply.DropItems();
                            Ply.Role = RoleType.NtfCadet;
                            Timing.CallDelayed(0.4f, () => Ply.Position = prePosition); break;
                        case RoleType.NtfCadet:
                            Ply.DropItems();
                            Ply.Role = RoleType.NtfLieutenant;
                            Timing.CallDelayed(0.4f, () => Ply.Position = prePosition); break;
                        case RoleType.NtfLieutenant:
                            Ply.DropItems();
                            Ply.Role = RoleType.NtfCommander;
                            Timing.CallDelayed(0.4f, () => Ply.Position = prePosition); break;
                        default:
                            Ply.DropItems();
                            Ply.Inventory.AddNewItem(ItemType.KeycardO5); break;
                    }
                    if (Plugin.Instance.Config.ShowHints)
                        Ply.ShowHint("the magic pill genie has promoted you!", 8f);
                }
            },
            {
                14, () =>
                {
                    List<Player> alive = Player.List.Where(x => x.Team != Team.RIP && x != Ply && x.Role != RoleType.Scp079).ToList();
                    if (alive.Count == 0)
                        return;
                    Player randAlive = alive[Rand.Next(alive.Count)];
                    Vector3 prePosition = Ply.Position;
                    Ply.Position = randAlive.Position;
                    randAlive.Position = prePosition;

                    if (!Plugin.Instance.Config.ShowHints) return;
                    Ply.ShowHint("the magic pill genie has switched you with another.", 8f);
                    randAlive.ShowHint("the magic pill genie has switched you with another.", 8f);
                }
            }
        };
    }
}
