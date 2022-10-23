using System;
using System.Collections.Generic;
using Dalamud.Logging;
using Dalamud.Game.ClientState.Objects.SubKinds;
using System.Runtime.InteropServices;
using Dalamud.Hooking;
using Lumina.Excel.GeneratedSheets;
using System.Linq;

namespace CarnivaleHelper.Modules
{
    public unsafe class SpellList : IDisposable
    {

        public List<uint> Spells;
        public List<byte> Ranks;
        public List<byte> ElementalAspect;
        public List<byte> DamageType;

        public SpellList()
        {
            Spells = new List<uint>();
            Ranks = new List<byte>();
            ElementalAspect = new List<byte>();
            DamageType = new List<byte>();
            try
            {
                _onActionUsedHook = Hook<OnActionUsedDelegate>.FromAddress(Service.SigScanner.ScanText("4C 89 44 24 ?? 55 56 41 54 41 55 41 56"), OnActionUsed);
                _onActionUsedHook?.Enable();
            }
            catch (Exception ex)
            {
                PluginLog.Debug(ex, "Could not construct SpellList");
            }
        }

        private delegate void OnActionUsedDelegate(uint sourceId, IntPtr sourceCharacter, IntPtr pos, IntPtr effectHeader, IntPtr effectArray, IntPtr effectTrail);
        private Hook<OnActionUsedDelegate>? _onActionUsedHook;

        private void OnActionUsed(uint sourceId, IntPtr sourceCharacter, IntPtr pos, IntPtr effectHeader,
        IntPtr effectArray, IntPtr effectTrail)
        {
            _onActionUsedHook?.Original(sourceId, sourceCharacter, pos, effectHeader, effectArray, effectTrail);

            PlayerCharacter? player = Service.ClientState.LocalPlayer;
            if (player == null || sourceId != player.ObjectId)
                return;

            try
            {
                uint actionId = (uint)Marshal.ReadInt32(effectHeader, 0x8);
                byte rank = 0;
                if (Service.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Action>()!.GetRow(actionId)!.ClassJob.Row == 36)
                {
                    rank = Service.DataManager.GetExcelSheet<AozAction>()!.Where(row => row.Action.Row == actionId).FirstOrDefault()!.Rank;
                }
                byte element = Service.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Action>()!.GetRow(actionId)!.Aspect;
                byte attackType = (byte) Service.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Action>()!.GetRow(actionId)!.AttackType.Row;


                if (!Spells.Contains(actionId))
                    Spells.Add(actionId);
                if (!Ranks.Contains(rank) && rank != 0)
                    Ranks.Add(rank);
                if (!ElementalAspect.Contains(element) && element != 0 && element != 7)
                    ElementalAspect.Add(element);
                if (!DamageType.Contains(attackType) && attackType != 0)
                    DamageType.Add(attackType);

                PluginLog.Debug("Action ID: " + actionId.ToString());
                PluginLog.Debug("Action Rank: " + rank.ToString());
                PluginLog.Debug("Action Aspect: " + element.ToString());
                PluginLog.Debug("Action Attack Type: " + attackType.ToString());
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Could not set spell attributes");
            }
            return;
        }

        public void Dispose() 
        {
            Spells.Clear();
            Spells.TrimExcess();
            Ranks.Clear();
            Ranks.TrimExcess();
            ElementalAspect.Clear();
            ElementalAspect.TrimExcess();
            DamageType.Clear();
            DamageType.TrimExcess();
            _onActionUsedHook?.Disable();
        }

#if DEBUG
        //Clear spell list without disabling and disposing of Hook -- for Debug purposes
        public void ClearSpellList()
        {
            Spells.Clear();
            Spells.TrimExcess();
            Spells.Clear();
            Spells.TrimExcess();
            Ranks.Clear();
            Ranks.TrimExcess();
            ElementalAspect.Clear();
            ElementalAspect.TrimExcess();
            DamageType.Clear();
            DamageType.TrimExcess();
        }

        //Print each Action ID for current spell list to Log
        public void PrintSpellList()
        {
            foreach (uint spell in Spells)
            {
                PluginLog.Debug(spell.ToString());
            }
        }
#endif
    }
}
