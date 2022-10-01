using System;
using System.Threading.Tasks;
using CarnivaleHelper.Utilities;
using Dalamud.Game.Gui;
using FFXIVClientStructs.FFXIV.Component.GUI;
using Dalamud.Logging;
using System.Diagnostics;

namespace CarnivaleHelper.Modules
{
    public unsafe class Targets
    {
        //Get Target Conditions on currently selected duty
        public unsafe void GetTargets()
        {
            try
            {
                var resNode = new BaseNode("AOZContentBriefing");

                if (resNode != null)
                {
                    var targetImageNode = resNode.GetNode<AtkImageNode>(25);
                    var targetDifficulty = resNode.GetNode<AtkTextNode>(7);
                    var firstTargetCondition = resNode.GetNode<AtkTextNode>(15);
                    var secondTargetCondition = resNode.GetNode<AtkTextNode>(16);

                    if (targetImageNode->AtkResNode.IsVisible && targetDifficulty->NodeText.ToString() != "Novice")
                    {
                        if (targetDifficulty->NodeText.ToString() == "Moderate" || targetDifficulty->NodeText.ToString() == "Advanced")
                        {
                            if (firstTargetCondition != null)
                            {
                                PluginLog.Debug(firstTargetCondition->NodeText.ToString());
                            }
                            else
                            {
                                PluginLog.Debug("Failed on First Text Condition");
                            }
                        }
                        if (targetDifficulty->NodeText.ToString() == "Advanced")
                        {
                            if (secondTargetCondition != null)
                            {
                                PluginLog.Debug(secondTargetCondition->NodeText.ToString());
                            }
                            else
                            {
                                PluginLog.Debug("Failed on Second Text Condition");
                            }
                        }
                    }
                    else
                    {
                        PluginLog.Debug("No Targets on selected Duty");
                    }
                }
                else
                {
                    PluginLog.Debug("Failed on AOZContentBriefing Addon");
                }
            }
            catch (Exception ex)
            {
                PluginLog.Debug(ex, "Something broke..");
            }
        }
    }
}
