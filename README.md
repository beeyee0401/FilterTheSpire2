Filter the Spire 2 is a mod for Slay the Spire 2 to find vanilla seeds with specific criteria.
This mod will mainly support the beta branch of Slay the Spire 2 while it's in Early Access.

# Before you start
1. This mod is primarily supporting the beta branch at this time, in the future I may separate releases, but for now, mostly supporting Beta
2. Filter the Spire is unfinished. I want to add more filters and fix any bugs that may come up.
3. Dependent on [BaseLib](https://github.com/Alchyr/BaseLib-StS2)
4. This mod assumes ALL available unlocks are finished. If you don't have a relic unlocked, it changes the entire list for RNG. Possibly looking to adjust this to look at your current unlocks, but it's simpler to not have to check the logic as a first release
5. Does not support multiplayer
6. Many relics have conditional mid-run checks to see if they can spawn, this mod assumes you meet them, because this mod will not choose your path or know your deck at the time the relic would appear.
    - For example, Nonupeipe only adds Beautiful Bracelet to the pool if you have at least 4 cards to enchant with Swift, this mod assumes that you will
7. English support only so far

<img width="1517" height="1225" alt="image" src="https://github.com/user-attachments/assets/0a4b09d6-bd86-49f9-bd0b-4992565a06e5" />

<img width="1580" height="1002" alt="image" src="https://github.com/user-attachments/assets/6f70614e-1f6b-42b1-b926-42e501ac53be" />


Installation instructions
1. If there isn't already a mods folder located at C:\Program Files (x86)\Steam\steamapps\common\Slay the Spire 2 (or wherever your file path equivalent is), make one. I'd also highly recommend making a folder within the mods folder called `FilterTheSpire2`, which is where you should put all the related files. You will also need [BaseLib](https://github.com/Alchyr/BaseLib-StS2) installed to use this mod.
1. Put the latest release of this mod's .pck, .dll, and .json files in that folder.
1. Launch the game. Note that launching the game for the first time created new "modded" save folders, this is intentional and your unmodded saves have not been wiped.

To unify your saves, you can use the [UnifiedSavePath](https://www.nexusmods.com/slaythespire2/mods/6) mod or follow the steps below:
- In-game, while on the modded saves (having any mod loaded), change to profile B
- Copy saves over (while the game is still open). These are located at the following paths:
  - Windows - %appdata%\SlayTheSpire2\steam\your_steam_id\
  - MacOS - ~/Library/Application Support/SlayTheSpire2/steam/your_steam_id/
  - Linux - ~/.local/share/SlayTheSpire2/steam/your_steam_id/
- In-game, change to profile A
- start a run, just to make sure it saves across the next restart
