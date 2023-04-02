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
            ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
            ImGuiWindowFlags.NoScrollWithMouse)
        {
            SignatureHelper.Initialise(this);
            //Size = new Vector2(175, 75);
            //SizeCondition = ImGuiCond.Once;
            SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(175, 75),
                MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
            };
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
            {
                TargetColorManager.Dispose();
                TargetColorManager = null;
            }     
        }

        private void OnContentFinderPop(object? sender, ContentFinderCondition queuedDuty)
        {
            if (Enum.IsDefined(typeof(AOZContentID), queuedDuty.ShortCode.ToString()))
            {
                var dutyId = (uint)Enum.Parse<AOZContentID>(queuedDuty.ShortCode.ToString());
                Targets = new Targets(dutyId);
                
                if (Targets.targetList.Any(x => x != 0))
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
                {
                    TargetColorManager!.Dispose();
                    TargetColorManager = null;
                }
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
            if (Configuration.DebugButtons == true)
            {
                if (ImGui.Button("Clear Spell List") && TargetColorManager != null)
                {
                    TargetColorManager!.SpellList.ClearSpellList();
                }

                if (ImGui.Button("Print Spell List") && TargetColorManager != null)
                {
                    TargetColorManager!.SpellList.PrintSpellList();
                }

                //Button for quick testing debug stuff
                if (ImGui.Button("Spell List Debug") && TargetColorManager != null)
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

                //Target Debug
                if (ImGui.Button("Target Debug"))
                {
                    PluginLog.Debug(Targets.currentDuty.ToString());
                }
                    ImGui.Separator();
            }
#endif
            if (Targets != null)
            {
                try
                {
                    foreach (byte target in Targets.targetList)
                    {
                        if (target != 0)
                        {
                            if (TargetColorManager != null)
                                ImGui.TextColored(TargetColorManager.SetColor(target), Targets.GetWeeklyTargetName(target));
                            else
                                ImGui.TextColored(Colors.White, Targets.GetWeeklyTargetName(target));
                        }
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

                if (TargetColorManager != null && Configuration.UniqueSpellCounter == true)
                {
                    try
                    {
                        ImGui.Separator();
                        ImGui.TextColored(Colors.White, "Unique Spells: " + TargetColorManager!.SpellList.Spells.Count.ToString());
                    }
                    catch (Exception ex)
                    {
                        PluginLog.Debug(ex, "Could not draw Unique Spell Counter");
                    }

                }
                if (TargetColorManager != null & Configuration.SpellList == true && TargetColorManager.SpellList.Spells.Count != 0)
                {
                    try
                    {
                        ImGui.Separator();
                        ImGui.TextColored(Colors.White, "Spell List: ");
                        foreach (uint spell in TargetColorManager!.SpellList.Spells)
                        {
                            ImGui.TextColored(Colors.White, Service.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Action>()!.GetRow(spell)!.Name.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        PluginLog.Debug(ex, "Could not draw Spell List");
                    }
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
