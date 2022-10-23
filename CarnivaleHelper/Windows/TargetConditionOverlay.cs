using CarnivaleHelper.Modules;
using CarnivaleHelper.Utilities;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Numerics;
using Dalamud.Utility.Signatures;
using Dalamud.Game.ClientState.Conditions;
using CarnivaleHelper.System;
using System.Linq;



//Overlay for displaying target conditions of current Masked Carnivale duty while in instance

namespace CarnivaleHelper.Windows
{
    public unsafe class TargetConditionOverlay : Window, IDisposable
    {
        private readonly Plugin Plugin;
        private readonly Configuration Configuration;
        private Targets? Targets;
        private TargetColorManager? TargetColorManager;
        private readonly AgentAozContentBriefing* _agent = Framework.Instance()->UIModule->GetAgentModule()->GetAgentAozContentBriefing();

        public TargetConditionOverlay(Plugin plugin) : base(
            "Target Condition Overlay",
            ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
            ImGuiWindowFlags.NoScrollWithMouse)
        {
            SignatureHelper.Initialise(this);
            this.Size = new Vector2(232, 130);
            this.SizeCondition = ImGuiCond.Once;
            this.Plugin = plugin;
            this.Configuration = plugin.Configuration;

            Service.ClientState.CfPop += OnContentFinderPop;
            Service.ClientState.TerritoryChanged += OnTerritoryChanged;
        }

        public void Dispose()
        {
            Service.ClientState.CfPop -= OnContentFinderPop;
            Service.ClientState.TerritoryChanged -= OnTerritoryChanged;
            if (this.Targets != null)
                this.Targets.Dispose();
            if (this.TargetColorManager != null)
                this.TargetColorManager.Dispose();
        }

        private void OnContentFinderPop(object? sender, ContentFinderCondition queuedDuty)
        {
            //Increment to allign Enum with WeeklyAozContentId and duty numbers in AOZ Duty List
            var dutyId = (uint) Enum.Parse<Enums.AOZContentID>(queuedDuty.ShortCode.ToString()) + 1;
            this.Targets = new Targets(dutyId);
            this.TargetColorManager = new TargetColorManager(this.Targets);

            Service.WindowManager.targetConditionOverlay.IsOpen = true;
            PluginLog.Debug("Duty Popped: " + dutyId.ToString());
        }

        //Prevent timer and colors from persisting when exiting AOZ Content
        private void OnTerritoryChanged(object? sender, ushort territory)
        {
            //Territory 131 = Ul'dah - Steps of Thal
            if (territory == 131)
            {
                if (this.TargetColorManager != null) 
                    this.TargetColorManager!.Dispose();
            }
        }

        public override void Draw()
        {
#if DEBUG
            if (ImGui.Button("Clear Spell List"))
            {
                this.TargetColorManager!.SpellList.ClearSpellList();
            }

            if (ImGui.Button("Print Spell List"))
            {
                this.TargetColorManager!.SpellList.PrintSpellList();
            }
            
            //Button for quick testing debug stuff
            if (ImGui.Button("Debug Testing Button") && this.TargetColorManager != null)
            {
                PluginLog.Debug("Spell List: ");
                foreach (uint spells in this.TargetColorManager!.SpellList.Spells)
                {
                    PluginLog.Debug(spells.ToString());
                }
                PluginLog.Debug("Unique Spell Count: " + this.TargetColorManager!.SpellList.Spells.Count().ToString());
                PluginLog.Debug("Spell Ranks: ");
                foreach (byte rank in this.TargetColorManager!.SpellList.Ranks)
                {
                    PluginLog.Debug(rank.ToString());
                }
                PluginLog.Debug("Spell Aspects: ");
                foreach (byte element in this.TargetColorManager!.SpellList.ElementalAspect)
                {
                    PluginLog.Debug(element.ToString());
                }
                PluginLog.Debug("Spell Damage Types: ");
                foreach (byte physical in this.TargetColorManager!.SpellList.DamageType)
                {
                    PluginLog.Debug(physical.ToString());
                }
            }
#endif
            if (this.Targets != null && this.TargetColorManager != null)
            {
                try
                {
                    foreach (byte target in this.Targets.targetList)
                    {
                        ImGui.TextColored(TargetColorManager!.SetColor(target), Targets.GetWeeklyTargetName(target));
                    }

                    //Territory 796 = Blue Sky (AOZ Arena)
                    //Only display Timer if inside Blue Sky zone and Timer-relevant Target is active
                    if (this.Configuration.Timer &&
                        (this.Targets?.targetList[0] == 1 || this.Targets?.targetList[0] == 2) &&
                        Service.ClientState.TerritoryType == 796)
                    {
                        ImGui.TextColored(this.TargetColorManager.timerColor,
                            (this.TargetColorManager.timer / 60).ToString() + ":" + (this.TargetColorManager.timer % 60).ToString()!.PadLeft(2, '0'));
                    }
                }
                catch (Exception ex)
                {
                    PluginLog.Error(ex, "Could not set Targets");
                }
            }
        }

#if DEBUG
        public uint GetCurrentDuty()
        {
            //Increment to allign CurrentContentIndex with WeeklyAozContentId and duty numbers in AOZ Duty List
            return (uint)_agent->AozContentData->CurrentContentIndex + 1;
        }

        public uint GetModerateDuty()
        {
            return (uint)_agent->WeeklyAozContentId[1];
        }

        public uint GetAdvancedDuty()
        {
            return (uint)_agent->WeeklyAozContentId[2];
        }
#endif
    }
}
