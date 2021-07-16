By default, this mod:
- Adds ability to Hover/Move on water.
- Greatly increases the turn speed, and slightly increase the speed.
- Adds ability to summon a Hoverbike by pressing F near a Hoverpad, costing 0.1 energy per meter away. Summon the nearest Hoverbike if you have multiple. If the bike doesn't have enough energy, you can still summon it and drain all the energy, I may change this in the future.
- Allows you to modify various values: FoV limit, Energy consumption, but the mod uses game default values unless you change it.

To change the values, edit config.json file, these are default mod values:

﻿{
  "HoverOnWater": true,

  "TopSpeed": 11.0,
  "Drag": 0.8,
  "AngularDrag": 1.0,

  "PitchSpring": 5.0,

  "YawSpring": 360.0,
  "MinViewConeAperture": 0.0,
  "MaxViewConeAperture": 10.0,

  "RollSpring": 2.5,
  "RollAngleDeadzone": 45.0,

  "EnergyConsumption": 0.06666,
  "LightEnergyConsumption": 0.0,

  "SummonKey": "f",
  "SummonEnergyPerMeter": 0.1,

  "MaxHealth": 200
}

- HoverOnWater [true/false]: Enable ability to hover on water.
- TopSpeed/Drag/Angular Drag: TopSpeed is the game code name. Definitely not the maximum speed the bike can reach, increasing it doesn't seem to work much for me, it may be Acceleration. Decrease Drag if you want more speed but it's harder to stop/control. Same for angular drag for steering speed. Game Default: 11.0 / 1.0 / 3.0
- PitchSpring: seem useless to me. I keep it at game default 5.0
- RollSpring/RollAngleDeadzone: the speed of looking up/down and the angle lock when you are in the bike. The mod uses the game default values of 2.5 and 45.0. You can set the RollAngleDeadzone to 180 if you want to be able to look fully up while driving (don't do that in real life!)
- YawSpring/MinViewConeAperture/MaxViewConeAperture: YawSpring is the speed which you can steer, so 360 means you can turn around in 1s. Min/Max view cone is the angle you can move freely before the bike would steer, so I decrease it so you can steer the bike easily but you lose the freedom of looking around quickly. Game Default: 15.0 / 0 / 45
- EnergyConsumption / LightEnergyConsumption: energy consumption of the bike itself, and if the light is on. The mod uses game default values of 0.0666  / 0 (Hoverbike light doesn't cost energy while they use a funny formula for energy: 71 / (339 * PI), which I simply put as 0.06666 for my mod).
- SummonKey / SummonEnergyPerMeter: For summon feature. You can change the key and the energy cost. Set SummonKey to "" or null (without "") if you want to disable this feature, or just don't press the key.
- MaxHealth: The maximum durability of the Snowfox. Game default is 200. Note that on screen values always show up as percentage (%) value so you always see 100 max. If you increase it to 1000 for example, your current and newly created Snowfoxes will start at 20% health (I don't know where to set it), just repair it or let them dock on the Hoverpad for a while.

There is an issue that I use a workaround: if you summon a bike that is docked in another Hoverpad, the old Hoverpad will be broken so I am forced to teleport you there, enter the bike to undock it, then exit the bike and teleport you back. There should be flashes of loading terrains, it is normal and just be patient for a few seconds.