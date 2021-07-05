using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Grenades;
using MEC;
using Mirror;
using UnityEngine;

namespace CrazyPills
{
    public class EventHandlers
    {
        public static void SpawnGrenadeOnPlayer(Player player, GrenadeType type, float timer)
        {
            var relativeVelocity = new Vector3(UnityEngine.Random.Range(0.0f, 2f), UnityEngine.Random.Range(0.0f, 2f), UnityEngine.Random.Range(0.0f, 2f));
            var component1 = player.ReferenceHub.gameObject.GetComponent<GrenadeManager>();
            GrenadeSettings grenadeSettings = null;
            switch (type)
            {
                case GrenadeType.FragGrenade:
                    grenadeSettings = (component1.availableGrenades).FirstOrDefault(g => g.inventoryID == ItemType.GrenadeFrag);
                    break;
                case GrenadeType.Scp018:
                    grenadeSettings = (component1.availableGrenades).FirstOrDefault(g => g.inventoryID == ItemType.SCP018);
                    break;
            }

            if (grenadeSettings == null) return;

            var component2 = UnityEngine.Object.Instantiate(grenadeSettings.grenadeInstance).GetComponent<Grenade>();
            if (type != GrenadeType.Scp018)
            {
                component2.fuseDuration = timer;
                component2.FullInitData(component1, player.Position, Quaternion.Euler(component2.throwStartAngle), component2.throwLinearVelocityOffset, component2.throwAngularVelocity, player.Team);
            }
            else
                component2.InitData(component1, relativeVelocity, Vector3.zero);
            NetworkServer.Spawn(component2.gameObject);
        }

        public static void SetPlayerScale(GameObject target, float scale)
        {
            try
            {
                NetworkIdentity identity = target.GetComponent<NetworkIdentity>();
                target.transform.localScale = Vector3.one * scale;

                ObjectDestroyMessage destroyMessage = new ObjectDestroyMessage();
                destroyMessage.netId = identity.netId;

                foreach (GameObject player in PlayerManager.players)
                {
                    if (player == target)
                        continue;

                    NetworkConnection playerCon = player.GetComponent<NetworkIdentity>().connectionToClient;
                    playerCon.Send(destroyMessage, 0);

                    object[] parameters = new object[] { identity, playerCon };
                    typeof(NetworkServer).InvokeStaticMethod("SendSpawnMessage", parameters);
                }
            }
            catch (Exception e)
            {
                Log.Info($"Set Scale error: {e}");
            }
        }

        public static void PillEffect(int num, Player p)
        {
            var rand = new System.Random();
            RoleType preRole;
            Vector3 prePosition;
            List<Player> alive;
            var preInv = new List<Inventory.SyncItemInfo>();
            Player randAlive;
            switch (num)
            {
                case 0:
                    if (Plugin.Instance.Config.ShowHints)
                        p.ShowHint("Seems that you'll be meeting an unfortunate fate...", 6f);
                    Timing.CallDelayed(6f, () => p.Kill());
                    break;
                case 1:
                    preRole = p.Role;
                    prePosition = p.Position;
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
                    break;
                case 2:
                    p.Health = p.MaxHealth;
                    p.ArtificialHealth = 200f;
                    break;
                case 3:
                    if (p.Role == RoleType.ClassD || p.Role == RoleType.Scientist)
                    {
                        p.Inventory.AddNewItem(ItemType.GunUSP);
                        p.Ammo[(int)AmmoType.Nato9] += 100;
                    }
                    else if (p.Role != RoleType.NtfLieutenant && p.Role != RoleType.NtfScientist)
                    {
                        p.Inventory.AddNewItem(ItemType.GunE11SR);
                        p.Ammo[(int)AmmoType.Nato556] += 100;
                    }
                    else
                    {

                        p.Inventory.AddNewItem(ItemType.GunLogicer);
                        p.Ammo[(int)AmmoType.Nato762] += 100;
                    }
                    if (Plugin.Instance.Config.ShowHints)
                        p.ShowHint("You have been granted a gun and ammo from the pill genie", 8f);
                    break;
                case 4:
                    alive = Player.List.Where(x => x.Team != Team.RIP && x != p && x.Role != RoleType.Scp079).ToList();
                    if (alive.Count == 0)
                        return;
                    p.Position = alive[rand.Next(alive.Count)].Position;
                    break;
                case 5://
                    SpawnGrenadeOnPlayer(p, GrenadeType.FragGrenade, 0.1f);
                    break;
                case 6:
                    SetPlayerScale(p.GameObject, 0.4f);
                    Timing.CallDelayed(30f, () => SetPlayerScale(p.GameObject, 1f));
                    break;
                case 7:
                    SpawnGrenadeOnPlayer(p, GrenadeType.Scp018, 7f);
                    SpawnGrenadeOnPlayer(p, GrenadeType.Scp018, 7f);
                    SpawnGrenadeOnPlayer(p, GrenadeType.Scp018, 7f);
                    SpawnGrenadeOnPlayer(p, GrenadeType.Scp018, 7f);
                    break;
                case 8:
                    if (Plugin.Instance.Config.ShowHints)
                        p.ShowHint("You've been granted invincibility for 20 seconds by pill genie", 8f);
                    Plugin.Instance.Invincible.Add(p);
                    Timing.CallDelayed(20f, () => Plugin.Instance.Invincible.Remove(p));
                    break;
                case 9:
                    alive = Player.List.Where(x => x.Team != Team.RIP && x != p && x.Role != RoleType.Scp079).ToList();
                    if (alive.Count == 0)
                        return;
                    randAlive = alive[rand.Next(alive.Count)];
                    p.Position = randAlive.Position;
                    break;
                case 10:
                    p.DropItems();
                    p.Inventory.items.Clear();
                    for (byte i = 0; i < 8; i++)
                        p.Inventory.AddNewItem(ItemType.Painkillers);
                    break;
                case 11:
                    if (!Plugin.Instance.Config.WarheadStatStop || Plugin.Instance.Config.WarheadStartStopChance > 0 && Plugin.Instance.Config.WarheadStartStopChance < 100 && rand.Next(101) > Plugin.Instance.Config.WarheadStartStopChance)
                    {
                        Warhead.LeverStatus = !Warhead.LeverStatus;
                        if (Plugin.Instance.Config.ShowHints)
                            p.ShowHint(string.Format($"The magic pill genie has switched the nuke to {Warhead.LeverStatus.ToString().ToLower()}.", 8f));
                    }
                    else
                    {
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
                        }
                        else
                        {
                            Warhead.Start();
                            Map.Broadcast(8, "The magic pill genie has started the warhead.");
                        }
                    }
                    break;
                case 12:
                    var dead = Player.Get(Team.RIP).ToList();
                    if (dead.Any())
                    {
                        var randDead = dead[rand.Next(dead.Count)];
                        preInv.AddRange(p.Inventory.items);
                        prePosition = p.Position;
                        preRole = p.Role;
                        p.Role = RoleType.Spectator;
                        randDead.Role = preRole;
                        Timing.CallDelayed(0.2f, () =>
                        {
                            randDead.Position = prePosition;
                            randDead.ResetInventory(preInv);
                        });
                        if (Plugin.Instance.Config.ShowHints)
                        {
                            p.ShowHint("the magic pill genie has replaced you with another.");
                            randDead.ShowHint("the magic pill genie has summoned you in place of another.");
                        }
                    }
                    else
                    {
                        alive = Player.List.Where(x => x.Team != Team.RIP && x != p && x.Role != RoleType.Scp079).ToList();
                        if (alive.Count == 0)
                            return;
                        randAlive = alive[rand.Next(alive.Count)];
                        prePosition = p.Position;
                        p.Position = randAlive.Position;
                        randAlive.Position = prePosition;
                        if (Plugin.Instance.Config.ShowHints)
                        {
                            p.ShowHint("the magic pill genie has switched you with another.", 8f);
                            randAlive.ShowHint("the magic pill genie has switched you with another.", 8f);
                        }
                    }
                    break;
                case 13:
                    prePosition = p.Position;
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
                    break;
                case 14:
                    alive = Player.List.Where(x => x.Team != Team.RIP && x != p && x.Role != RoleType.Scp079).ToList();
                    if (alive.Count == 0)
                        return;
                    randAlive = alive[rand.Next(alive.Count)];
                    prePosition = p.Position;
                    p.Position = randAlive.Position;
                    randAlive.Position = prePosition;
                    if (Plugin.Instance.Config.ShowHints)
                    {
                        p.ShowHint("the magic pill genie has switched you with another.", 8f);
                        randAlive.ShowHint("the magic pill genie has switched you with another.", 8f);
                    }
                    break;
            }
        }
    }
}
