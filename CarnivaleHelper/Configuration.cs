using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace CarnivaleHelper
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool TargetTracker { get; set; } = true;
        public bool Timer { get; set; } = true;
        public bool UniqueSpellCounter { get; set; } = false;
        public bool SpellList { get; set; } = false;

#if DEBUG
        public bool DebugButtons { get; set; } = false;
#endif

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            PluginInterface = pluginInterface;
        }

        public void Save()
        {
            PluginInterface!.SavePluginConfig(this);
        }
    }
}
