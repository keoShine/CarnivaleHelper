using System;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Reflection;
using Dalamud.Interface.Windowing;
using CarnivaleHelper.Windows;
using CarnivaleHelper.Modules;
using Dalamud.Logging;
using FFXIVClientStructs;

namespace CarnivaleHelper
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Carnivale Helper";
        private const string CommandName = "/pcarnivale";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("CarnivaleHelper");


        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            Resolver.Initialize();

            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);
            this.PluginInterface.Create<Service>();

            // you might normally want to embed resources and load them from the manifest stream
            //var target = new Targets();
            var spellbook = new Spellbook();

            WindowSystem.AddWindow(new ConfigWindow(this));
            WindowSystem.AddWindow(new MainWindow(this));
            WindowSystem.AddWindow(new TargetConditionOverlay(this));

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Open the config menu"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            WindowSystem.GetWindow("Carnivale Overlay")!.IsOpen = !WindowSystem.GetWindow("Carnivale Overlay")!.IsOpen;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            WindowSystem.GetWindow("Carnivale Helper Config")!.IsOpen = !WindowSystem.GetWindow("Carnivale Helper Config")!.IsOpen;
        }

        public void DrawDutyOverlay()
        {
            WindowSystem.GetWindow("Target Condition Overlay")!.IsOpen = !WindowSystem.GetWindow("Target Condition Overlay")!.IsOpen;
        }
    }
}
