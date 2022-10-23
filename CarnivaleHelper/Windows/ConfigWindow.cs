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
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.Size = new Vector2(232, 150);
        this.SizeCondition = ImGuiCond.Always;

        this.Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        
        var configValueTarget = this.Configuration.TargetTracker;
        if (ImGui.Checkbox("Display Target Conditions", ref configValueTarget))
        {
            this.Configuration.TargetTracker = configValueTarget;
            this.Configuration.Save();
        }

        var configValueTimer = this.Configuration.Timer;
        if (ImGui.Checkbox("Display Timer", ref configValueTimer))
        {
            this.Configuration.Timer = configValueTimer;
            this.Configuration.Save();
        }

        //Config settings not implemented yet -- the two below checkboxes do nothing
        var configValueUniqueSpells = this.Configuration.UniqueSpellCounter;
        if (ImGui.Checkbox("Display Unique Spell Count", ref configValueUniqueSpells))
        {
            this.Configuration.UniqueSpellCounter = configValueUniqueSpells;
            this.Configuration.Save();
        }

        var configValueSpellList = this.Configuration.SpellList;
        if (ImGui.Checkbox("Display Spells Used", ref configValueSpellList))
        {
            this.Configuration.SpellList = configValueSpellList;
            this.Configuration.Save();
        }
    }
}
