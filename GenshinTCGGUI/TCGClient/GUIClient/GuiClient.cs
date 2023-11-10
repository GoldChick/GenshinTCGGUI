using GenshinTCGGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TCGBase;

namespace TCGClient
{
    public class GuiClient : AbstractMainClient
    {
        public int MeID { get; private set; }
        private Action<ReadonlyGame> _bind;
        private Action<ReadonlyGame> _render;
        public GuiClient(Action<ReadonlyGame> bind, Action<ReadonlyGame> render)
        {
            _bind = bind;
            _render = render;
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
                                                      "genshin3_3:food_sweetchicken", "genshin3_3:food_sweetchicken", "genshin3_3:food_sweetchicken", "genshin3_3:寒天宣命祝词", "genshin3_3:寒天宣命祝词" ,
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
                Characters = new[] { "genshin3_3:hydro", "genshin3_3:fischl", "genshin3_3:debt" },
                ActionCards = new[] { "genshin3_3:food_sweetchicken", "genshin3_3:food_sweetchicken", "genshin3_3:food_sweetchicken", "genshin3_3:food_sweetchicken", "genshin3_3:food_sweetchicken" ,
                                                      "genshin3_3:food_sweetchicken", "genshin3_3:food_sweetchicken", "genshin3_3:food_sweetchicken", "genshin3_3:food_sweetchicken", "genshin3_3:food_sweetchicken" ,
                                                      "genshin3_3:food_sweetchicken", "genshin3_3:food_sweetchicken", "genshin3_3:food_sweetchicken", "genshin3_3:partner_changtheninth", "genshin3_3:partner_changtheninth" ,
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
            MainWindow.Instance.UpdatePacketRender(MeID, packet);
        }
        public override void UpdateRegion()
        {
            base.UpdateRegion();
            _render?.Invoke(Game);
        }
        public override NetEvent RequestEvent(ActionType demand, string help_txt = "Null")
        {
            MainWindow.Instance.ClientUpdateCosts(GetAllDiceCost());
            return MainWindow.Instance.RequestEventCallBack(demand, help_txt) ?? new NetEvent(new(ActionType.Pass));
        }
        public override void RequestEnemyEvent(ActionType demand)
        {
            base.RequestEnemyEvent(demand);
            MainWindow.Instance.RequestEnemyEventCallBack(demand);
        }
    }
}
