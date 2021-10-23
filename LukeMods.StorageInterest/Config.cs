using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace LukeMods.StorageInterest
{
    // This will generate a section in the Options window at the Mods tab.
    [Menu("Storage with Interest")]
    public class Config : ConfigFile
    {
        public static Config Instance;

        // Create a slider in the options section of this mod.
        [Slider("Interest Rate", 10.0f, 600.0f, DefaultValue = 300.0f)]
        public float InterestRate = 300.0f;

        // Create a slider in the options section of this mod.
        [Slider("Shortes Rate", 10.0f, 600.0f, DefaultValue = 10.0f)]
        public float MinTime = 10.0f;

        // Let the player decide if they want to see a message
        // hovering the mouse over the container that explain
        // that the interest is disabled for that type of container
        // or remove the message completely.
        [Toggle("See when interest is 'disabled'")]
        public bool WarnDisabledInterest = false;

        // We can use toggles for the core game storages to simplify.
        [Toggle("Wall Locker")]
        public bool SmallLocker = true;

        [Toggle("Locker")]
        public bool Locker = true;

        [Toggle("Fridge")]
        public bool Fridge = true;

        [Toggle("Aquarium")]
        public bool Aquarium = true;

        /**
         * Known issue:
         * 
         * The script check the name of the storages to validate which
         * storage container should have the interest rate calculated.
         * The Prawn Suit storage is "Storage", which is the same start
         * of the Seatruck storage name. Currently, the code check if
         * the storage name "starts with", so this option will always 
         * generate interest in both the Prawn Suit and Seatruck when
         * enabled, even if the Seatruck option is false.
         */
        [Toggle("Prawn Suit Container")]
        public bool Storage= false;

        [Toggle("Seatruck Container")]
        public bool StorageContainer= true;

        // The list of custom storages created by other mods will
        // be stored in a string array. This toggle will affects the
        // entire list. We enable or disable ALL custom storages.
        [Toggle("Custom (Mods) Storage Types")]
        public bool CustomModsStorageTypes = false;

        // To keep the original intended behavior, this new mechanic will
        // start disabled and available as an option. This allows the
        // player to receive the interest by just hovering the cursor over
        // the container instead of needing to open it. The player must
        // be close enought for the hovering event to compute.
        [Toggle("Receive interest on hover")]
        public bool PayInterestOnCursorHovering = false;

        // Provide an example to generate the config.json file so players
        // can extend the list of storages that have the interest feature.
        // By adding a valid storage type to the list, it will compute
        // interest just like the core game storages when enabled.
        public string[] Containers { get; set; } = new[] { "ModCustomStorageType" };

        public Config()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        /**
         * Get all the storage types allowed to have interest rates.
         */
        public string[] GetContainers()
        {
            List<string> containers = new List<string>();

            if ((Containers != null) && (Containers.Length >= 1) && CustomModsStorageTypes)
            {
                containers.AddRange(Containers);
            }

            if (SmallLocker) containers.Add("SmallLocker");
            if (Locker) containers.Add("Locker");
            if (Fridge) containers.Add("Fridge");
            if (Aquarium) containers.Add("Aquarium");
            if (Storage) containers.Add("Storage");
            if (StorageContainer) containers.Add("StorageContainer");

            return containers.ToArray();
        }
    }
}
