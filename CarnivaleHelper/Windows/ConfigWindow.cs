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
        this.Size = new Vector2(232, 130);
        this.SizeCondition = ImGuiCond.Always;

        this.Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        // can't ref a property, so use a local copy
        var configValueTarget = this.Configuration.TargetTracker;
        if (ImGui.Checkbox("Display Target Conditions", ref configValueTarget))
        {
            this.Configuration.TargetTracker = configValueTarget;
            // can save immediately on change, if you don't want to provide a "Save and Close" button
            this.Configuration.Save();
        }
        var configValueUniqueSpells = this.Configuration.UniqueSpellCounter;
        if (ImGui.Checkbox("Display Unique Spell Count", ref configValueUniqueSpells))
        {
            this.Configuration.UniqueSpellCounter = configValueUniqueSpells;
            // can save immediately on change, if you don't want to provide a "Save and Close" button
            this.Configuration.Save();
        }
        var configValueSpellList = this.Configuration.SpellList;
        if (ImGui.Checkbox("Display Spells Used", ref configValueSpellList))
        {
            this.Configuration.SpellList = configValueSpellList;
            // can save immediately on change, if you don't want to provide a "Save and Close" button
            this.Configuration.Save();
        }
    }
}
