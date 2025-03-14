﻿using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using TCGBase;

namespace TCGClient
{
    public abstract class AbstractMainClient : AbstractClient
    {
        protected Action<string>? _tb;
        public void BindHelpTextAction(Action<string>? tb) => _tb = tb;
        public DiceCostPacket GetAllDiceCost()
        {
            return new(Game.Cards.Select((value, index) => new CardCost(GetEventFinalDiceRequirement(new(OperationType.UseCard, index)), GetCardTargetEnums(index))),
                         Enumerable.Range(0, Game.Me.CurrCharacter == -1 ? 0 : Game.Me.Characters[Game.Me.CurrCharacter].SkillCount).Select(idx => GetEventFinalDiceRequirement(new(OperationType.UseSKill, idx))),
                         Enumerable.Range(0, Game.Me.Characters.Count).Select(index => GetEventFinalDiceRequirement(new(OperationType.Switch, index))),
                         GetEventFinalDiceRequirement(new(OperationType.Blend, 0)));
        }
    }
}
