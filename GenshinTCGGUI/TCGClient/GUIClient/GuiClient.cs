using GenshinTCGGUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TCGBase;

namespace TCGClient
{
    public class GuiClient : AbstractMainClient
    {
        public int MeID { get; private set; }
        private Action<ReadonlyGame>? _bind;
        private Action<ReadonlyGame>? _render;
        public void BindInitRenderAction(Action<ReadonlyGame>? bind) => _bind = bind;
        public void BindUpdateRenderAction(Action<ReadonlyGame>? render) => _render = render;
        public override void InitServerSetting(ServerSetting setting)
        {
        }

        public override ServerPlayerCardSet RequestCardSet()
        {
            var setjson = File.ReadAllText(Directory.GetCurrentDirectory() + "/cardsets/0.json");
            var set = JsonSerializer.Deserialize<CardSetSetting>(setjson);
            return new(set.CardSet);
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
        public override NetEvent RequestEvent(ActionType demand)
        {
            MainWindow.Instance.ClientUpdateCosts(GetAllDiceCost());
            return MainWindow.Instance.RequestEventCallBack(demand) ?? new NetEvent(new(ActionType.Pass));
        }
        public override void RequestEnemyEvent(ActionType demand)
        {
            base.RequestEnemyEvent(demand);
            MainWindow.Instance.RequestEnemyEventCallBack(demand);
        }
    }
}
