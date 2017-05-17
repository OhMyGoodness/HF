using System.Collections.Generic;
using System.Linq;
using HeartFunnySERVER.Game.BoardNS;
using HeartFunnySERVER.Game.CardNS;

namespace HeartFunnySERVER.Game.CardDB.CardsData
{
    /* Малое исцеление # Lesser heal */
    class CS1H_001 : Card
    {
        public override Card Create()
        {
            return new CS1H_001();
        }
        public override void Init()
        {
            base.Init();
            TargetTypeOnPlay = CardTypes.TargetType.ALL;
        }
        public override void OnPlay(Board board, Card target = null, int index = -1, int choice = 0)
        {
            base.OnPlay(board, target, index, choice);
            if (target != null)
            {
                if (IsFriend)
                {
                    target.Heal(2 * board.HealFactor, board);
                }
                else
                {
                    target.Heal(2 * board.EnemyHealFactor, board);
                }
            }
        }
    }
    /* Малое исцеление # Lesser Heal */
    class CS1H_001_H1 : Card
    {
        public override Card Create()
        {
            return new CS1H_001_H1();
        }
        public override void Init()
        {
            base.Init();
            TargetTypeOnPlay = CardTypes.TargetType.ALL;
        }
        public override void OnPlay(Board board, Card target = null, int index = -1, int choice = 0)
        {
            base.OnPlay(board, target, index, choice);
            if (target != null)
            {
                if (IsFriend)
                {
                    target.Heal(2 * board.HealFactor, board);
                }
                else
                {
                    target.Heal(2 * board.EnemyHealFactor, board);
                }
            }
        }
    }
    /* Солдат Златоземья # Goldshire Footman */
    class CS1_042 : Card
    {
        public override Card Create()
        {
            return new CS1_042();
        }
        public override void Init()
        {
            base.Init();
            IsTaunt = true;
        }
    }
    /* Трясинный ползун # Fen Creeper */
    class CS1_069 : Card
    {
        public override Card Create()
        {
            return new CS1_069();
        }
        public override void Init()
        {
            base.Init();
        }
    }
    /* Кольцо света # Holy Nova */
    class CS1_112 : Card
    {
        public override Card Create()
        {
            return new CS1_112();
        }
        public override void Init()
        {
            base.Init();
        }
        public override void OnPlay(Board board, Card target = null, int index = -1, int choice = 0)
        {
            base.OnPlay(board, target, index, choice);
            if (IsFriend)
            {
                foreach (var card in board.MinionEnemy)
                {
                    card.Damage(2 * board.HealFactor, board);
                }
                foreach (var card in board.MinionFriend)
                {
                    card.Heal(2 * board.HealFactor, board);
                }
                board.HeroFriend.Heal(2 * board.HealFactor, board);
                board.HeroEnemy.Damage(2 * board.HealFactor, board);
            }
            else if (!IsFriend)
            {
                foreach (var card in board.MinionEnemy)
                {
                    card.Heal(2 * board.EnemyHealFactor, board);
                }
                foreach (var card in board.MinionFriend)
                {
                    card.Damage(2 * board.EnemyHealFactor, board);
                }
                board.HeroEnemy.Heal(2 * board.EnemyHealFactor, board);
                board.HeroFriend.Damage(2 * board.EnemyHealFactor, board);
            }
        }
    }
    /* Контроль разума # Mind Control */
    class CS1_113 : Card
    {
        public override Card Create()
        {
            return new CS1_113();
        }
        public override void Init()
        {
            base.Init();
            TargetTypeOnPlay = CardTypes.TargetType.MINION_ENEMY;
        }
        public override void OnPlay(Board board, Card target = null, int index = -1, int choice = 0)
        {
            base.OnPlay(board, target, index, choice);
            target.IsFriend = true;
        }
    }
    /* Внутренний огон # Inner Fire */
    class CS1_129 : Card
    {
        public override Card Create()
        {
            return new CS1_129();
        }
        public override void Init()
        {
            base.Init();
        }
        public override void OnPlay(Board board, Card target = null, int index = -1, int choice = 0)
        {
            base.OnPlay(board, target, index, choice);
            target.CurrentAtk = target.CurrentHealth;
        }
    }
    /* Божественная кара # Holy Smite */
    class CS1_130 : Card
    {
        public override Card Create()
        {
            return new CS1_130();
        }
        public override void Init()
        {
            base.Init();
            TargetTypeOnPlay = CardTypes.TargetType.ALL;
        }
        public override void OnPlay(Board board, Card target = null, int index = -1, int choice = 0)
        {
            base.OnPlay(board, target, index, choice);
            if (IsFriend)
                target.Damage(2 + board.GetSpellPower(), board);
            else
                target.Damage(2 + board.GetSpellPowerEnemy(), board);
        }
    }
}
