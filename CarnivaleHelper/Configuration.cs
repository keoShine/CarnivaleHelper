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
        public bool UniqueSpellCounter { get; set; } = false;
        public bool SpellList { get; set; } = false;
        

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}
