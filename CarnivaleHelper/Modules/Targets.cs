using System;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Lumina.Excel.GeneratedSheets;

namespace CarnivaleHelper.Modules
{
    public unsafe class Targets :IDisposable
    {
        public uint currentDuty;
        public byte[] targetList;
        public ushort? finishTime;
        private readonly AgentAozContentBriefing* _agent = Framework.Instance()->UIModule->GetAgentModule()->GetAgentAozContentBriefing();

        public Targets(uint duty)
        {
            currentDuty = duty;
            targetList = GetWeeklyTarget();
            finishTime = GetFinishTime(targetList[0]);
        }

        public void Dispose()
        {

        }

        //Return currentDuty for debug purposes
        public uint GetCurrentDuty()
        {
            return currentDuty;
        }


        public byte[] GetWeeklyTarget()
        {
            var currentTarget = new byte[3];
            if (currentDuty == Convert.ToUInt16(_agent->WeeklyAozContentId[1]))
            {
                currentTarget[0] = _agent->ModerateRequirement[0];
                currentTarget[1] = _agent->ModerateRequirement[1];
                currentTarget[2] = _agent->ModerateRequirement[2];
            }
            else if (currentDuty == Convert.ToUInt16(_agent->WeeklyAozContentId[2]))
            {
                currentTarget[0] = _agent->AdvancedRequirement[0];
                currentTarget[1] = _agent->AdvancedRequirement[1];
                currentTarget[2] = _agent->AdvancedRequirement[2];

            }
            return currentTarget;
        }

        public ushort? GetFinishTime(byte timerTargetId)
        {
            switch (timerTargetId)
            {
                case 1:
                    return Service.DataManager.GetExcelSheet<AOZContent>()!.GetRow(currentDuty)?.IdealFinishTime!;
                case 2:
                    return Service.DataManager.GetExcelSheet<AOZContent>()!.GetRow(currentDuty)?.StandardFinishTime!;
                default:
                    return 0;
            }
        }

        public string? GetWeeklyTargetName(byte targetId)
        {
            switch (targetId) 
            {
                //Add Finish Time to Target Name if Target is 'Too Fast, Too Furious' or 'Slow and Steady'
                case 1: case 2:
                    return Service.DataManager.GetExcelSheet<AOZScore>()!.GetRow(targetId)?.Name.ToString() + " (" + (finishTime / 60).ToString() + ":" + (finishTime % 60).ToString() + ")";

                default:
                    return Service.DataManager.GetExcelSheet<AOZScore>()!.GetRow(targetId)?.Name.ToString();
            }
        }
    }
}
