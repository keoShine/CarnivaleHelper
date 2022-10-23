using CarnivaleHelper.Enums;
using CarnivaleHelper.Modules;
using CarnivaleHelper.Utilities;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Numerics;
using Dalamud.Utility.Signatures;
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
            Size = new Vector2(232, 130);
            SizeCondition = ImGuiCond.Once;
            Plugin = plugin;
            Configuration = plugin.Configuration;

            Service.ClientState.CfPop += OnContentFinderPop;
            Service.ClientState.TerritoryChanged += OnTerritoryChanged;
        }

        public void Dispose()
        {
            Service.ClientState.CfPop -= OnContentFinderPop;
            Service.ClientState.TerritoryChanged -= OnTerritoryChanged;
            if (Targets != null)
                Targets.Dispose();
            if (TargetColorManager != null)
                TargetColorManager.Dispose();
        }

        private void OnContentFinderPop(object? sender, ContentFinderCondition queuedDuty)
        {
            if (Enum.IsDefined(typeof(AOZContentID), queuedDuty.ShortCode.ToString()))
            {
                var dutyId = (uint)Enum.Parse<AOZContentID>(queuedDuty.ShortCode.ToString());
                Targets = new Targets(dutyId);

                Service.WindowManager.targetConditionOverlay.IsOpen = true;
                PluginLog.Debug("Duty Popped: " + dutyId.ToString());
            }
        }

        //Prevent timer and colors from persisting when exiting AOZ Content
        private void OnTerritoryChanged(object? sender, ushort territory)
        {
            //Territory 131 = Ul'dah - Steps of Thal
            if (territory == 131)
            {
                if (Targets != null)
                    Targets.Dispose();
                if (TargetColorManager != null)
                    TargetColorManager!.Dispose();
            }
            //Territory 796 = Blue Sky (AOZ Arena)
            if (territory == 796)
            {
                TargetColorManager = new TargetColorManager(Targets!);
            }
        }

        public override void Draw()
        {
#if DEBUG
            if (ImGui.Button("Clear Spell List"))
            {
                TargetColorManager!.SpellList.ClearSpellList();
            }

            if (ImGui.Button("Print Spell List"))
            {
                TargetColorManager!.SpellList.PrintSpellList();
            }
            
            //Button for quick testing debug stuff
            if (ImGui.Button("Debug Testing Button") && TargetColorManager != null)
            {
                PluginLog.Debug("Spell List: ");
                foreach (uint spells in TargetColorManager!.SpellList.Spells)
                {
                    PluginLog.Debug(spells.ToString());
                }
                PluginLog.Debug("Unique Spell Count: " + TargetColorManager!.SpellList.Spells.Count().ToString());
                PluginLog.Debug("Spell Ranks: ");
                foreach (byte rank in TargetColorManager!.SpellList.Ranks)
                {
                    PluginLog.Debug(rank.ToString());
                }
                PluginLog.Debug("Spell Aspects: ");
                foreach (byte element in TargetColorManager!.SpellList.ElementalAspect)
                {
                    PluginLog.Debug(element.ToString());
                }
                PluginLog.Debug("Spell Damage Types: ");
                foreach (byte physical in TargetColorManager!.SpellList.DamageType)
                {
                    PluginLog.Debug(physical.ToString());
                }
            }
#endif
            if (Targets != null)
            {
                try
                {
                    foreach (byte target in Targets.targetList)
                    {
                        if (TargetColorManager != null)
                            ImGui.TextColored(TargetColorManager.SetColor(target), Targets.GetWeeklyTargetName(target));
                        else
                            ImGui.TextColored(Colors.White, Targets.GetWeeklyTargetName(target));
                    }

                    //Territory 796 = Blue Sky (AOZ Arena)
                    //Only display Timer if inside Blue Sky zone and Timer-relevant Target is active
                    if (Configuration.Timer &&
                        (Targets?.targetList[0] == 1 || Targets?.targetList[0] == 2) &&
                        Service.ClientState.TerritoryType == 796)
                    {
                        ImGui.TextColored(TargetColorManager!.timerColor,
                            (TargetColorManager.timer / 60).ToString() + ":" + (TargetColorManager.timer % 60).ToString()!.PadLeft(2, '0'));
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
            return _agent->WeeklyAozContentId[1];
        }

        public uint GetAdvancedDuty()
        {
            return _agent->WeeklyAozContentId[2];
        }
#endif
    }
}
