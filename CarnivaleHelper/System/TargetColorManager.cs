using CarnivaleHelper.Enums;
using CarnivaleHelper.Modules;
using CarnivaleHelper.Utilities;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CarnivaleHelper.System
{
    internal unsafe class TargetColorManager : IDisposable
    {
        public SpellList SpellList;
        public Targets Targets;
        public Vector4 timerColor;
        public int? timer;

        public TargetColorManager(Targets targets)
        {
            SpellList = new SpellList();
            Targets = targets;
        }

        public void Dispose()
        {
            SpellList.Dispose();
        }

        public Vector4 SetColor(byte targetId)
        {
            //Only color text if inside Blue Sky zone
            if (Service.ClientState.TerritoryType == 796)
            {
                //Targets to still make tests for:
                    //5: Can't Touch This
                    //6: Ain't Got Time to Bleed
                    //7: Modus Interruptus
                    //17: Enfeeble Me Tender
                    //18: Enfeeble Me Tenderer
                switch (targetId)
                {
                    //Timer-Based Targets
                    case 1:
                    case 2:
                        timerColor = SetColor(timer, Targets.finishTime);
                        return timerColor;

                    //Spell Count Targets
                    case 3:
                        if (SpellList.Spells.Count >= 2 && SpellList.Spells.Count <= 10)
                            return Colors.Green;
                        if (SpellList.Spells.Count > 10)
                            return Colors.Red;
                        return Colors.Orange;
                    case 4:
                        if (SpellList.Spells.Count >= 2 && SpellList.Spells.Count <= 4)
                            return Colors.Green;
                        if (SpellList.Spells.Count > 4)
                            return Colors.Red;
                        return Colors.Orange;

                    //Sprint
                    case 8:
                        return SpellList.sprintCheck;

                    //Spell Aspect Targets
                    case 9:
                        return SetColor(Aspect.Fire);
                    case 10:
                        return SetColor(Aspect.Water);
                    case 11:
                        return SetColor(Aspect.Earth);
                    case 12:
                        return SetColor(Aspect.Wind);
                    case 13:
                        return SetColor(Aspect.Ice);
                    case 14:
                        return SetColor(Aspect.Lightning);
                    case 15:
                        if (SpellList.ElementalAspect.Count == 6)
                            return Colors.Green;
                        return Colors.Orange;
                    case 16:
                        if (SpellList.ElementalAspect.Count == 6 && SpellList.DamageType.Intersect(new List<byte> { 1, 2, 3}).Count() == 3)
                        {
                            return Colors.Green;
                        }
                        return Colors.Orange;

                    //Physical Damage Only
                    case 19:
                        if (SpellList.DamageType.Contains(5))
                            return Colors.Red;
                        if (SpellList.DamageType.Any())
                            return Colors.Green;
                        return Colors.Orange;

                    //Spell Rank Targets
                    case 20:
                        if (SpellList.Ranks.Intersect(new List<byte> { 1, 2, 3 }).Any())
                            return Colors.Red;
                        if (SpellList.DamageType.Any())
                            return Colors.Green;
                        return Colors.Orange;
                    case 21:
                        if (SpellList.Ranks.Intersect(new List<byte> { 3, 4, 5 }).Any())
                            return Colors.Red;
                        if (SpellList.DamageType.Any())
                            return Colors.Green;
                        return Colors.Orange;

                    //Breath-Based Targets
                    case 22:
                        if (SpellList.Spells.Intersect(SpellListEnums.BreathAttacks).Count() >= 4)
                            return Colors.Green;
                        else
                            return Colors.Orange;
                    case 29:
                        if (SpellList.Spells.Intersect(SpellListEnums.BreathAttacks).Count() >= 8)
                            return Colors.Green;
                        else
                            return Colors.Orange;
                    case 30:
                        if (SpellList.Spells.Intersect(new List<uint> { 11406, 18303, 18318 }).Count() == 3)
                            return Colors.Green;
                        return Colors.Orange;
                    default:
                        return Colors.White;
                }
            }
            return Colors.White;
        }

        //Check Color for Timer-related Targets
        private Vector4 SetColor(int? timer, ushort? finishTime)
        {
            if (TimerCheck())
            {
                this.timer = (int) Math.Max(0, 1801 - EventFramework.Instance()->GetInstanceContentDirector()->ContentDirector.ContentTimeLeft);
                if ((timer - finishTime) <= 0)
                    return Colors.Green;
                return Colors.Red;
            }
            return Colors.White;
        }

        //Check for Elemental Damage targets
        private Vector4 SetColor(Aspect element)
        {
            if (SpellList.ElementalAspect.Count != 0)
            {
                foreach (var aspect in SpellList.ElementalAspect)
                    if (aspect != (byte) element)
                        return Colors.Red;
                return Colors.Green;
            }
            return Colors.Orange;
        }

        //Condition check to prevent plugin timer from setting until in-game timer is actually set to 30:01
        private bool TimerCheck()
        {
            return Service.Condition[ConditionFlag.BoundByDuty] &&
                    !(Service.Condition[ConditionFlag.BetweenAreas51] ||
                        Service.Condition[ConditionFlag.OccupiedInQuestEvent] ||
                        Service.Condition[ConditionFlag.Occupied33]);
        }
    }
}
