namespace HeartFunnySERVER.Game.CardNS
{
    public static class CardTypes
    {
        public enum CClass
        {
            SHAMAN,
            PRIEST,
            MAGE,
            PALADIN,
            WARRIOR,
            WARLOCK,
            HUNTER,
            ROGUE,
            DRUID,
            DEATHKNIGHT,
            NONE,
            JARAXXUS,
            DREAM,
            NEUTRAL,
            INVALID
        }

        public enum CRace
        {
            MURLOC,
            PIRATE,
            TOTEM,
            DRAGON,
            NONE,
            MECH,
            BEAST,
            DEMON,
            MECHANICAL,
            ORC,
            ELEMENTAL,
            INVALID
        }

        public enum CRarity
        {
            INVALID,
            COMMON,
            FREE,
            RARE,
            EPIC,
            LEGENDARY,
            UNKNOWN_6
        }

        public enum CSet
        {
            INVALID,
            TEST_TEMPORARY,
            CORE,
            EXPERT1,
            REWARD,
            MISSIONS,
            DEMO,
            NONE,
            CHEAT,
            BLANK,
            DEBUG_SP,
            PROMO,
            FP1,
            PE1,
            BRM,
            TGT,
            CREDITS,
            HERO_SKINS,
            TB,
            SLUSH,
            LOE
        }

        public enum CType
        {
            HERO,
            MINION,
            SPELL,
            WEAPON,
            HEROPOWER,
            ENCHANTMENT
        }

        public enum ProcEnchType
        {
            NONE,
            DEATH,
            ATTACK,
            END_TURN,
            INSPIRE
        }

        public enum ProcSecretType
        {
            NONE,
            MINION_ATK_HERO,
            MINION_ATK_MINION,
            MINION_ATK,
            HERO_ATK_HERO,
            HERO_ATK_MINION,
            HERO_ATK,
            ATK,
            ATK_MINION,
            ATK_HERO,
            DEATH
        }

        public enum ProcType
        {
            NONE,
            BEFORE_SPELL,
            AFTER_SPELL
        }

        public enum TargetSide
        {
            FRIEND,
            ENEMY,
            BOTH
        }

        public enum TargetType
        {
            NONE,
            HERO_FRIEND,
            HERO_ENEMY,
            HERO_BOTH,
            MINION_FRIEND,
            MINION_ENEMY,
            MINION_BOTH,
            BOTH_FRIEND,
            BOTH_ENEMY,
            ALL,
            ALL_NONE
        }
    }
}