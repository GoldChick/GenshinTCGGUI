using System;
using System.Linq;
using TCGBase;

namespace TCGClient
{
    public abstract class AbstractMainClient : AbstractClient
    {
        protected Action<string>? _tb;
        public void BindHelpTextAction(Action<string>? tb) => _tb = tb;
        public DiceCostPacket GetAllDiceCost()
        {
            return new(Game.Cards.Select((value, index) => new CardCost(GetCardCostRequirement(index), GetCardTargetEnums(index))),
                         Enumerable.Range(0, Game.Me.CurrCharacter == -1 ? 0 : Game.Me.Characters[Game.Me.CurrCharacter].SkillCount).Select(index => new SkillCost(GetSkillCostRequirement(index))),
                         Enumerable.Range(0, Game.Me.Characters.Count).Select(index => GetEventFinalDiceRequirement(new(ActionType.Switch, index))),
                         Game.Me.CurrCharacter == -1 ? null : new DiceCostVariable(false, 1));
        }
    }
}
