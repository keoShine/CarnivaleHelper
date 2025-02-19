using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace CarnivaleHelper.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;

    public ConfigWindow(Plugin plugin) : base(
        "Carnivale Helper Config",
        ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {

        Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        
        var configValueTarget = Configuration.TargetTracker;
        if (ImGui.Checkbox("Display Target Conditions", ref configValueTarget))
        {
            Configuration.TargetTracker = configValueTarget;
            Configuration.Save();
        }

        var configValueTimer = Configuration.Timer;
        if (ImGui.Checkbox("Display Timer", ref configValueTimer))
        {
            Configuration.Timer = configValueTimer;
            Configuration.Save();
        }

        //Config settings not implemented yet -- the two below checkboxes do nothing
        var configValueUniqueSpells = Configuration.UniqueSpellCounter;
        if (ImGui.Checkbox("Display Unique Spell Count", ref configValueUniqueSpells))
        {
            Configuration.UniqueSpellCounter = configValueUniqueSpells;
            Configuration.Save();
        }

        var configValueSpellList = Configuration.SpellList;
        if (ImGui.Checkbox("Display Spells Used", ref configValueSpellList))
        {
            Configuration.SpellList = configValueSpellList;
            Configuration.Save();
        }
#if DEBUG
        var configValueDebugButtons = Configuration.DebugButtons;
        if (ImGui.Checkbox("Debug Buttons", ref configValueDebugButtons))
        {
            Configuration.DebugButtons = configValueDebugButtons;
            Configuration.Save();
        }
#endif
    }
}
