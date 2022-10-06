using CarnivaleHelper.Modules;
using CarnivaleHelper.Utilities;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using ImGuiNET;
using Lumina.Data.Parsing;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

//Overlay for displaying target conditions of current Masked Carnivale duty while in instance

namespace CarnivaleHelper.Windows
{
    public unsafe class TargetConditionOverlay : Window, IDisposable
    {
        private readonly Plugin Plugin;
        private readonly Configuration Configuration;
        private Targets? Targets;
        private readonly AgentAozContentBriefing* _agent = Framework.Instance()->UIModule->GetAgentModule()->GetAgentAozContentBriefing();

        public TargetConditionOverlay(Plugin plugin) : base(
            "Target Condition Overlay",
            ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
            ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.Size = new Vector2(232, 130);
            this.SizeCondition = ImGuiCond.Always;

            this.Plugin = plugin;
            this.Configuration = this.Plugin.Configuration;
        }
        public void Dispose() { }

        public override void Draw()
        {

            if (ImGui.Button("Set Current Duty"))
            {
                try
                {
                    this.Targets = new Targets(GetCurrentDuty());

                }
                catch (Exception ex)
                {
                    PluginLog.Debug(ex, "Could not set Duty");
                }
            }

            if (this.Targets != null)
            {
                ImGui.TextColored(Colors.Green, this.Targets?.target_0_Name);
                ImGui.TextColored(Colors.Red, this.Targets?.target_1_Name);
            }
        }

        public uint GetCurrentDuty()
        {
            //Increment to allign CurrentContentIndex with WeeklyAozContentId and duty numbers in AOZ Duty List
            return (uint)_agent->AozContentData->CurrentContentIndex + 1;
        }
    }
}
