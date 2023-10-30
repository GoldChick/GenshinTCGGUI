using System;
using System.Threading;
using System.Threading.Tasks;
using TCGBase;

namespace TCGClient
{
    public class GuiClient : AbstractClient
    {
        private readonly Action<ReadonlyGame> _bind;
        private readonly Action<ReadonlyGame> _render;
        private readonly Func<ActionType, string, NetEvent> _eventcallback;
        public GuiClient(Action<ReadonlyGame> bind,Action<ReadonlyGame> render,Func<ActionType,string,NetEvent> eventcallback) 
        {
            _bind = bind;
            _render = render;
            _eventcallback = eventcallback;
        }
        public override void InitServerSetting(ServerSetting setting)
        {
            ClientSetting = new()
            {
                Name = "DefaultBuiltIn",
                DefaultCardSet = new PlayerNetCardSet()
                {
                    Characters = new[] { "genshin3_3:qq", "genshin3_3:yoimiya", "genshin3_3:ayaka" },
                    ActionCards = new[] { "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪" ,
                                                      "genshin3_3:赌徒的耳环", "genshin3_3:赌徒的耳环", "genshin3_3:赌徒的耳环", "genshin3_3:寒天宣命祝词", "genshin3_3:寒天宣命祝词" ,
                                                      "genshin3_3:寒天宣命祝词", "genshin3_3:寒天宣命祝词", "genshin3_3:寒天宣命祝词", "genshin3_3:寒天宣命祝词", "genshin3_3:寒天宣命祝词" ,
                                                      "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword" ,
                                                      "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome" },
                }
            };
        }

        public override ServerPlayerCardSet RequestCardSet()
        {
            return new(new PlayerNetCardSet()
            {
                Characters = new[] { "genshin3_3:qq", "genshin3_3:yoimiya", "genshin3_3:ayaka" },
                ActionCards = new[] { "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪" ,
                                                      "genshin3_3:赌徒的耳环", "genshin3_3:赌徒的耳环", "genshin3_3:赌徒的耳环", "genshin3_3:寒天宣命祝词", "genshin3_3:寒天宣命祝词" ,
                                                      "genshin3_3:寒天宣命祝词", "genshin3_3:寒天宣命祝词", "genshin3_3:寒天宣命祝词", "genshin3_3:寒天宣命祝词", "genshin3_3:寒天宣命祝词" ,
                                                      "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword" ,
                                                      "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome" },
            });
        }
        public override void BindInit(ReadonlyGame game)
        {
            _bind.Invoke(game);
        }
        public override void Update(ClientUpdatePacket packet)
        {
            base.Update(packet);
            _render.Invoke(Game);
        }
        public override void UpdateRegion()
        {
            base.UpdateRegion();
            _render.Invoke(Game);
        }
        public override NetEvent RequestEvent(ActionType demand, string help_txt = "Null")
        {
            return _eventcallback.Invoke(demand, help_txt);
        }
    }
}
