using TCGBase;

namespace TCGClient
{
    internal partial class ConsoleClient : AbstractClient
    {
        public override NetEvent RequestEvent(ActionType demand, string help_txt = "Null") => Act(demand);

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
            return new ServerPlayerCardSet(ClientSetting.DefaultCardSet as PlayerNetCardSet);
        }
    }
}
