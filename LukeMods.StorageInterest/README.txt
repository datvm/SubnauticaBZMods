Alterra needs YOU! With their trade on the boom, they need more resources. To encourage resource gathering, they enacted a new policy: paying Interest for storing items in Lockers.

For each locker, you will receive a duplication of a stored item every 300 seconds (5 minutes). It gets even better! The duration is halves for each additional copy. For example, if you have 3 Titanium Ingots, you receive an additional Ingot every 75 seconds.

However there is a catch! The greedies at Alterra only gives out payment once per check out, and they don't stack! You have to open the Locker every time you think you should receive your extra copy, 1 item per open and the time don't stack for the next one.

Details:
- Only apply to: Wall (Small) Locker, (Glass) Locker, Lockers in Seamoth Storage Module and Aquarium (but not in Alien Containment or Seamoth Aquarium due to technical issues with those, Alterra engineers will try to resolve this soon).
- Time starts counting when you: put item in/take item out of Locker, open the Locker to receive bonus item, load the game (you cannot save the time from previous session).
- Counting only the item type that has the most copies in the Locker:
-- For example, if you have 2 Titaniums and 2 Coppers, usually Titanium shows up first, you would receive extra copy of Titanium.
-- You receive extra copy after 300 seconds. If you have multiple copies, the times are: 300, 150, 75, 37.5, 18.75, 10. The minimum time to receive an interest is 10 seconds.
- Opening the Locker without changing anything (pick up/deposit) before the interest pays off doesn't reset the time.
- You won't receive extra item if the Locker do not have enough space for the payment even if it can contain smaller items.

You can change the numbers by editing config.json file:

{
  "InterestRate": 300.0,
  "MinTime": 10.0,
  "Containers": [
    "SmallLocker",
    "Aquarium",
    "Locker",
    "StorageContainer"
  ]
}

- InterestRate: base time to receive extra copy if you have one copy.
- MinTime: minimum time to receive extra copy.
- Containers: list of supported container names. Right now they are for Wall Locker, Aquarium, (Glass) Locker and Seamoth Storge Containers. If you have a mod that add more containers, you can add the names here if you know the internal name. You can also try adding other containers but as far as I test, they are not stable.