using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using ImGuiNET;
using ImGuiScene;
using CarnivaleHelper.Utilities;
using CarnivaleHelper.Modules;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace CarnivaleHelper.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly Plugin Plugin;
    private readonly Targets? Targets;

    public MainWindow(Plugin plugin) : base(
        "Carnivale Overlay", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.Plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {

        if (ImGui.Button("Show Settings"))
        {
            this.Plugin.DrawConfigUI();
        }

        ImGui.Spacing();

        if (ImGui.Button("Get Weekly Targets"))
        {
            try
            {
                //this.Targets = new Targets();
                //var targetDuty = this.Targets.GetWeeklyTarget(this.Targets.GetCurrentDuty());
                //for (int i = 0; i < targetDuty.Length; i++)
                //{
                //    if (targetDuty[i] != 0)
                //        PluginLog.Debug(targetDuty[i].ToString());
                //}
                //if (this.Targets.target0 != 0)
                //{
                //    PluginLog.Debug(this.Targets.target0.ToString());
                //}
                //if (this.Targets.target1 != 0)
                //{
                //    PluginLog.Debug(this.Targets.target1.ToString());
                //}
                //this.Targets.Dispose();
            }
            catch (Exception ex)
            {
                PluginLog.Debug(ex, "Something went very wrong");
            }
        }

        ImGui.Spacing();

        if (ImGui.Button("Display Target Overlay"))
        {
            try
            {
                this.Plugin.DrawDutyOverlay();
            }
            catch (Exception ex)
            {
                PluginLog.Debug(ex, "Could not display Target Overlay");
            }
        }
    }
}
