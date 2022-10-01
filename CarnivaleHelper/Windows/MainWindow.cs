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
    private Plugin Plugin;
    private Targets Targets;

    public MainWindow(Plugin plugin, Targets target) : base(
        "Carnivale Overlay", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.Plugin = plugin;
        this.Targets = target;
    }

    public void Dispose() { }

    public override void Draw()
    {

        if (ImGui.Button("Show Settings"))
        {
            this.Plugin.DrawConfigUI();
        }

        ImGui.Spacing();

        if (ImGui.Button("Test"))
        {
            try
            {
                this.Targets.GetTargets();
            }
            catch (Exception ex)
            {
                PluginLog.Debug(ex, "Something went very wrong");
            }
        }
    }
}
