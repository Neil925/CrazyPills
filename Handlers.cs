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
using Object = UnityEngine.Object;

namespace CrazyPills
{
    public class Handlers : PillEvents
    {
        public static void SpawnGrenade(Vector3 position, Vector3 velocity, Player player, float fuseTime = 3f, GrenadeType grenadeType = GrenadeType.FragGrenade)
        {
            if (player == null)
                player = Server.Host;

            var component = player.GrenadeManager;
            var component2 = Object.Instantiate(component.availableGrenades[(int)grenadeType].grenadeInstance).GetComponent<Grenade>();

            component2.FullInitData(component, position, Quaternion.Euler(component2.throwStartAngle), velocity, component2.throwAngularVelocity, player == Server.Host ? Team.SCP : player.Team);
            component2.NetworkfuseTime = NetworkTime.time + fuseTime;
            NetworkServer.Spawn(component2.gameObject);
        }

        public static void PillEffect(int num, Player p)
        {
            Ply = p;
            PillEffects[num]();
        }
    }
}
