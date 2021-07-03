# Crazy Pills

An SCP:SL Exiled plugin that allows for randomized and fun events to occur upon consumption of the in-game item "Pain Killers".

When enabling this plugin, 15 randomized events may occur during the consumption of the "Pain Killers" item in SCP:SL.

### Pain Killer Consumption Events Consist Of
```
- Instant death
- Temporary zombification
- Maximum health reheal and significant artificial hp increase
- Being granted a gun
- Teleportation to a random player
- A random player teleporting to the consumer
- Switching places with a random player
- Replacement with a dead player
- Spontaneous combustion
- 4 bouncy balls being thrown
- Smallification
- Temporary invincibility
- A full inventory of Pain Killers
- 90% cahnce of Switching the state of the warhead lever, 10% switchting the state of the warhead itself.
- A role promotion
```

### Command
A command is also added with this plugin. That being a remote admin console command called `pill` or `pills`.
The command allows for instantaneous occurance of the effects of consuming a Pain Killer and requires the `cp.pill` permission node in the Exiled `permissions.yml`.
The command accepts one optional argument to choose which pill effect the command sender would like to trigger. If left empty, it will be randomized.

### Significant Configs Values
```
Boolean SpawnWithPills (true by default)
Description: Whether or not all players are guarenteed to spawn with pills.

Boolean WarheadStatStop (true by default)
Description: Whether or not the pills can have a 1 in 15 in 10 chance of turning on the nuke or off.
```
