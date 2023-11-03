using System.Linq;
using TCGBase;

namespace TCGClient
{
    public abstract class AbstractMainClient : AbstractClient
    {
        public DiceCostPacket GetAllDiceCost()
        {
           return new(Game.Cards.Select((value, index) => new CardCost(GetEventFinalDiceRequirement(new(ActionType.UseCard, index)), GetTargetEnums(new(ActionType.UseCard, index)))),
                        Enumerable.Range(0, Game.Me.CurrCharacter == -1 ? 0 : Game.Me.Characters[Game.Me.CurrCharacter].SkillCount).Select(index => GetEventFinalDiceRequirement(new(ActionType.UseSKill, index))),
                        Enumerable.Range(0, Game.Me.Characters.Count).Select(index => GetEventFinalDiceRequirement(new(ActionType.Switch, index))),
                        Game.Me.CurrCharacter == -1 ? null : new DiceCostVariable(false, 1));
        }
    }
}
