using System;
using System.Threading.Tasks;
using CarnivaleHelper.Utilities;
using Dalamud.Game.Gui;
using FFXIVClientStructs.FFXIV.Component.GUI;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Dalamud.Hooking;
using Dalamud.Logging;
using System.Diagnostics;
using Lumina.Excel.GeneratedSheets;

namespace CarnivaleHelper.Modules
{
    public unsafe class Targets :IDisposable
    {
        public uint currentDuty = 0;
        public byte target_0 = 0;
        public string? target_0_Name = "";
        public byte target_1 = 0;
        public string? target_1_Name = "";
        public ushort? standardFinishTime = 0;
        public ushort? idealFinishTime = 0;
        private AgentAozContentBriefing* _agent = Framework.Instance()->UIModule->GetAgentModule()->GetAgentAozContentBriefing();

        public Targets(uint duty)
        {
            currentDuty = duty;
            target_0 = GetWeeklyTarget()[0];
            target_0_Name = GetWeeklyTargetName(target_0);
            target_1 = GetWeeklyTarget()[1];
            target_1_Name = GetWeeklyTargetName(target_1);
            standardFinishTime = GetStandardFinishTime();
            idealFinishTime = GetIdealFinishTime();
        }

        public void Dispose() { }

        public uint GetCurrentDuty()
        {
            return currentDuty;
        }


        public byte[] GetWeeklyTarget()
        {
            var currentTarget = new byte[2];
            if (currentDuty == Convert.ToUInt16(_agent->WeeklyAozContentId[1]))
            {
                currentTarget[0] = _agent->ModerateRequirement[0];
            }
            else if (currentDuty == Convert.ToUInt16(_agent->WeeklyAozContentId[2]))
            {
                currentTarget[0] = _agent->AdvancedRequirement[0];
                currentTarget[1] = _agent->AdvancedRequirement[1];
            }
            return currentTarget;
        }

        public ushort? GetStandardFinishTime()
        {
            return Service.DataManager.GetExcelSheet<AOZContent>()!.GetRow(currentDuty)?.StandardFinishTime;
        }

        public ushort? GetIdealFinishTime()
        {
            return Service.DataManager.GetExcelSheet<AOZContent>()!.GetRow(currentDuty)?.IdealFinishTime;
        }
        public string? GetWeeklyTargetName(byte targetId)
        {
            return Service.DataManager.GetExcelSheet<AOZScore>()!.GetRow(targetId)?.Name.ToString();
        }
    }
}
