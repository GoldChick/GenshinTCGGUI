using GenshinTCGGUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TCGBase;

namespace TCGClient
{
    public class GuiClient : AbstractMainClient, IGuiCllient
    {
        public int MeID { get; private set; }
        private Action<ReadonlyGame>? _bind;
        private Action<ReadonlyGame>? _render;
        public void BindInitRenderAction(Action<ReadonlyGame>? bind) => _bind = bind;
        public void BindUpdateRenderAction(Action<ReadonlyGame>? render) => _render = render;

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
            var g = Game;
            MainWindow.Instance.UpdatePacketRender(MeID, packet);
        }
        public override void UpdateRegion()
        {
            base.UpdateRegion();
            _render?.Invoke(Game);
            var g= Game;
        }
        public override NetEvent RequestEvent(OperationType demand)
        {
            MainWindow.Instance.ClientUpdateCosts(GetAllDiceCost());
            return MainWindow.Instance.RequestEventCallBack(demand) ?? new NetEvent(new(OperationType.Pass));
        }
        public override void RequestEnemyEvent(OperationType demand)
        {
            base.RequestEnemyEvent(demand);
            MainWindow.Instance.RequestEnemyEventCallBack(demand);
        }

        public void GetCardNextValidTargets(int cardindex, int[] already_params, Action<List<int>>? callback) => callback?.Invoke(GetCardNextValidTargets(cardindex, already_params));
    }
}
