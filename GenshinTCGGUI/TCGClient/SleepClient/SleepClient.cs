using TCGBase;

namespace TCGClient
{
    /// <summary>
    /// 只是什么也不干的低能ai
    /// </summary>
    internal class SleepClient : AbstractClient
    {
        public override void InitServerSetting(ServerSetting setting)
        {
        }

        public override ServerPlayerCardSet RequestCardSet()
        {
            return new(new PlayerNetCardSet()
            {
                Characters = new[] { "genshin3_3:keqing", "genshin3_3:yoimiya", "genshin3_3:ayaka" },
                ActionCards = new[] { "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪" ,
                                                      "genshin3_3:赌徒的耳环", "genshin3_3:赌徒的耳环", "genshin3_3:赌徒的耳环", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword" ,
                                                      "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome" },
            });
        }

        public override NetEvent RequestEvent(ActionType demand, string help_txt = "Null")
        {
            return new NetEvent(new(ActionType.Pass));
        }
    }
}
