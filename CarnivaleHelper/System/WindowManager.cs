using CarnivaleHelper.Windows;
using Dalamud.Interface.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarnivaleHelper.System
{
    internal class WindowManager : IDisposable
    {

        private readonly Plugin Plugin;
        private readonly WindowSystem WindowSystem = new("CarnivaleHelper");
        public ConfigWindow configWindow;
        public MainWindow mainWindow;
        public TargetConditionOverlay targetConditionOverlay;

        public WindowManager(Plugin plugin)
        {
            this.Plugin = plugin;
            this.configWindow = new ConfigWindow(plugin);
            this.mainWindow = new MainWindow(plugin);
            this.targetConditionOverlay = new TargetConditionOverlay(plugin);
            WindowSystem.AddWindow(configWindow);
            WindowSystem.AddWindow(mainWindow);
            WindowSystem.AddWindow(targetConditionOverlay);
            Service.PluginInterface.UiBuilder.Draw += DrawUI;
            Service.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.configWindow.Dispose();
            this.mainWindow.Dispose();
            this.targetConditionOverlay.Dispose();
            Service.PluginInterface.UiBuilder.Draw -= DrawUI;
            Service.PluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUI;

            WindowSystem.RemoveAllWindows();
        }
        private void DrawUI()
        {
            WindowSystem.Draw();
        }
        public void DrawMainWindow()
        {
            WindowSystem!.GetWindow("Carnivale Overlay")!.IsOpen = !WindowSystem!.GetWindow("Carnivale Overlay")!.IsOpen;
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
