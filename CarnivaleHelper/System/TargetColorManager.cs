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
        public Vector4[] ColorList;
        public Vector4 timerColor;
        public int? timer;

        public TargetColorManager(Targets targets)
        {
            this.SpellList = new SpellList();
            this.Targets = targets;
            this.ColorList = new Vector4[]
            {
                Colors.White,
                Colors.White,
                Colors.White
            };
        }

        public void Dispose()
        {
            this.SpellList.Dispose();
        }

        public Vector4 SetColor(byte targetId)
        {
            switch (targetId)
            {
                //Timer-Based Targets
                case 1:
                case 2:
                    return SetColor(this.timer, this.Targets!.finishTime);
                //Elemental-Based Targets
                //case 3:

                //Breath-Based Targets
                case 22:
                    if (this.SpellList.Spells.Intersect<uint>(Enums.SpellListEnums.BreathAttacks).Count() >= 4)
                    {
                        return Colors.Green;
                    }
                    else
                    {
                        return Colors.Orange;
                    }
                case 29:
                    if (this.SpellList.Spells.Intersect<uint>(Enums.SpellListEnums.BreathAttacks).Count() >= 8)
                    {
                        return Colors.Green;
                    }
                    else
                    {
                        return Colors.Orange;
                    }
                default:
                    return Colors.White;
            }
        }

        //Check Color for Timer-related Targets
        private Vector4 SetColor(int? timer, ushort? finishTime)
        {
            if (TimerCheck())
            {
                this.timer = (int)Math.Max(0, 1801 - EventFramework.Instance()->GetInstanceContentDirector()->ContentDirector.ContentTimeLeft);
                this.timerColor = SetColor(this.timer, this.Targets.finishTime);
                switch ((timer - finishTime) <= 0)
                {
                    case true:
                        return Colors.Green;
                    case false:
                        return Colors.Red;
                }
            }
            return Colors.White;
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
