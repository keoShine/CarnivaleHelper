using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
//using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnivaleHelper.Modules
{
    internal class EnemyList
    {
        //This does nothing currently; will eventually attempt to work towards keeping track of inflicted debuffs and spells from enemies started but not finished
        public List<BattleChara> Enemies = new List<BattleChara>();
        public List<uint> Statuses = new List<uint>();

        public EnemyList()
        {
            foreach (GameObject obj in Service.Objects)
            {
                BattleChara? actor = obj as BattleChara;
                foreach (Status status in actor.StatusList)
                {
                    if (!Statuses.Contains(status.StatusId) && Service.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Status>()!.GetRow(status.StatusId)!.StatusCategory == 2)
                    {
                        Statuses.Add(status.StatusId);
                    }
                }
            }
        }
    }
}
