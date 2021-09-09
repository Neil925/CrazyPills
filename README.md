# Crazy Pills

An SCP:SL Exiled plugin that allows for randomized and fun events to occur upon consumption of the in-game item "Pain Killers".

When enabling this plugin, 15 randomized events may occur during the consumption of the "Pain Killers" item in SCP:SL.

### Pain Killer Consumption Events Consist Of
```
 - Kill - Instant death.
 - Zombify - Temporarily being turned into a zombie.
 - FullHeal - Full health and additional health.
 - GiveGun - Being granted a gun and ammo for said gun.
 - Goto - Teleporting to a random player.
 - Combustion - Spontaneous combustion.
 - Shrink - Temporary minification.
 - Balls - 4 Balls being thrown from the point of the player.
 - Invincibility - Player gets granted god mode temporarily.
 - Bring - Having a random player teleported to you.
 - FullPK - Being given a full inventory of pain killers to consume to your heart's content.
 - WarheadEvent - The toggling of either the warhead's nuke silo lever or the nuke state itself, with the probability being based upon a config value.
 - SwitchDead - Being replaced out for a spectator.
 - Promote - Going up one level in the hierarchy in accordance to the current role or being granted an O5 if already at the higher role.
 - Switch - Having the position exchanged with a different player.
```

### Command
A command is also added with this plugin. That being a remote admin console command called `pill` or `pills`.
The command allows for instantaneous occurrence of the effects of consuming a Pain Killer and requires the `cp.pill` permission node in the Exiled `permissions.yml`.
The command accepts one optional argument to choose which pill effect the command sender would like to trigger. If left empty, it will be randomized.

### Significant Configs Values
```
Boolean SpawnWithPills (true by default)
Description: Whether or not all players are guaranteed to spawn with pills.

List<PillEffectType> PillEffects (All by default)
Description: A list of possible pill effects that are allowed to occur.

Integer WarheadStartStopChance (10 by default)
Description: If "WarheadStatStop" is true, this dictates the percentage chance of the warhead starting/stopping with the event.

Boolean ShowHints (true by default)
Description: Whether or not to show a hint during certain Pain Killer consumption events.
```

### Notes:
All strings are configurable through translation files with the exception of the pill command description.