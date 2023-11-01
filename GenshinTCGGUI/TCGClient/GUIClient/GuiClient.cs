using System;
using TCGBase;

namespace TCGClient
{
    public class GuiClient : AbstractClient
    {
        public int MeID { get; private set; }
        private Action<ReadonlyGame> _bind;
        private Action<ReadonlyGame> _render;
        private Action<ClientUpdatePacket> _packetrender;
        private Func<ActionType, string, NetEvent> _eventcallback;
        public GuiClient()
        {
        }
        public GuiClient(Action<ReadonlyGame> bind, Action<ReadonlyGame> render, Action<ClientUpdatePacket> packetrender, Func<ActionType, string, NetEvent> eventcallback)
        {
            _bind = bind;
            _render = render;
            _packetrender = packetrender;
            _eventcallback = eventcallback;
        }
        public void ChangeCallBack(Action<ReadonlyGame> bind, Action<ReadonlyGame> render, Action<ClientUpdatePacket> packetrender, Func<ActionType, string, NetEvent> eventcallback)
        {
            _bind = bind;
            _render = render;
            _packetrender = packetrender;
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
                                                      "genshin3_3:partner_liusu", "genshin3_3:partner_liusu", "genshin3_3:partner_liusu", "genshin3_3:partner_liusu", "genshin3_3:partner_liusu" ,
                                                      "genshin3_3:location_dawnwinery", "genshin3_3:location_dawnwinery", "genshin3_3:location_dawnwinery", "genshin3_3:location_dawnwinery", "genshin3_3:location_dawnwinery" },
                }
            };
        }

        public override ServerPlayerCardSet RequestCardSet()
        {
            return new(new PlayerNetCardSet()
            {
                Characters = new[] { "genshin3_3:mona", "genshin3_3:xiangling", "genshin3_3:noel" },
                ActionCards = new[] { "genshin3_3:partner_timmie", "genshin3_3:partner_timmie", "genshin3_3:partner_timmie", "genshin3_3:partner_timmie", "genshin3_3:partner_timmie" ,
                                                      "genshin3_3:partner_timmie", "genshin3_3:partner_timmie", "genshin3_3:partner_timmie", "genshin3_3:partner_timmie", "genshin3_3:partner_timmie" ,
                                                      "genshin3_3:赌徒的耳环", "genshin3_3:partner_timmie", "genshin3_3:赌徒的耳环", "genshin3_3:partner_changtheninth", "genshin3_3:partner_changtheninth" ,
                                                      "genshin3_3:partner_changtheninth", "genshin3_3:partner_changtheninth", "genshin3_3:partner_changtheninth", "genshin3_3:partner_changtheninth", "genshin3_3:partner_changtheninth" ,
                                                      "genshin3_3:partner_liusu", "genshin3_3:partner_liusu", "genshin3_3:partner_liusu", "genshin3_3:partner_liusu", "genshin3_3:partner_liusu" ,
                                                      "genshin3_3:location_dawnwinery", "genshin3_3:location_dawnwinery", "genshin3_3:location_dawnwinery", "genshin3_3:location_dawnwinery", "genshin3_3:location_dawnwinery" },
            });
        }
        public override void BindInit(ReadonlyGame game)
        {
            MeID = game.MeID;
            _bind?.Invoke(game);
        }
        public override void Update(ClientUpdatePacket packet)
        {
            base.Update(packet);
            _packetrender?.Invoke(packet);
        }
        public override void UpdateRegion()
        {
            base.UpdateRegion();
            _render?.Invoke(Game);
        }
        public override NetEvent RequestEvent(ActionType demand, string help_txt = "Null")
        {
            return _eventcallback?.Invoke(demand, help_txt) ?? new NetEvent(new(ActionType.Pass));
        }
    }
}
