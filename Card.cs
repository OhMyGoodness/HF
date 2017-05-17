using System;
using System.Collections.Generic;
using System.Linq;
using HeartFunnySERVER.Game.BoardNS;
using HeartFunnySERVER.Game.CardDB;

namespace HeartFunnySERVER.Game.CardNS
{
    public class Card : CardData
    {
        public static Card Create(string cardId, bool isFriend, int id, int index = -1, Behavior behavior = null)
        {
            Card card = null;
            CardTemplate cardTemplate = CardTemplate.LoadCachedCard(cardId);
            // загружаем карту из базового шаблона

            if (cardTemplate != null && cardTemplate.AIStatus != CardTemplate.AiStatus.Ai_Error)
            {
                try
                {
                    Type type = System.Type.GetType("HeartFunnySERVER.Game.CardDB.CardsData." + cardId);
                    if (type != null)
                    {
                        card = (Card)Activator.CreateInstance(type);
                    }
                    if (card != null) card.AIFound = true;
                    cardTemplate.AIStatus = CardTemplate.AiStatus.Ai_Loaded;
                }
                catch (Exception ex)
                {
                    cardTemplate.AIStatus = CardTemplate.AiStatus.Ai_Error;

                    MyLog.Log.debug("Ошибка загрузки карты " + cardId + ". Карта не добавлена в базу AI:" + ex);
                }

                if (card == null)
                {
                    card = new Card();
                }
            }
            else
            {
                cardTemplate = cardTemplate ?? CardTemplate.LoadCachedCard("CARD_NOT_FOUND");
                card = new Card();
            }

            card.InitCard(cardTemplate, isFriend, id);
            card.InitBehavior(behavior);
            card.CardId = cardId;
            card.Index = index;

            return card;
        }

        private void InitBehavior(Behavior behavior = null, string name = "Default")
        {
            if (behavior != null)
            {
                Behavior = behavior;
            }
            else
            {
                Behavior = BehaviorList.Instance.Get(Template.Id, name);
                if (Behavior == null)
                { 
                    Behavior = new Behavior();
                }
            }
        }

        protected Card Clone()
        {
            return Clone(this);
        }

        public static Dictionary<string, Card> Clone(Dictionary<string, Card> instances)
        {
            return instances.Select(card => Clone(card.Value)).ToDictionary(clone => clone.GetHash());
        }

        public static List<Card> Clone(IEnumerable<Card> instances)
        {
            return instances.Select(Clone).ToList();
        }

        public static Card Clone(Card instance)
        {
            if (instance == null)
                return null;

            Card card = Create(instance.CardId, instance.IsFriend, instance.Id, instance.Index, instance.Behavior);

            card.CardTitle = instance.CardTitle;
            card.ChoiceOneTarget = instance.ChoiceOneTarget;
            card.ChoiceTwoTarget = instance.ChoiceTwoTarget;
            card.Class = instance.Class;
            card.CountAttack = instance.CountAttack;
            card.CountTurnsInPlay = instance.CountTurnsInPlay;
            card.CurrentArmor = instance.CurrentArmor;
            card.CurrentAtk = instance.GetRealCurrentAtk;
            //card.CurrentAtk = instance.CurrentAtk;
            card.CurrentCost = instance.CurrentCost;
            card.CurrentDurability = instance.CurrentDurability;
            card.CurrentHealth = instance.CurrentHealth;
            card.Buffs = new List<Buff>();
            foreach (Buff buff in instance.Buffs)
            {
                card.Buffs.Add(new Buff(buff.CardId, buff.Id));
            }
            card.Enchants = new List<Enchant>();
            foreach (Enchant ench in instance.Enchants)
            {
                card.Enchants.Add(new Enchant(ench.EnchantCard));
            }
            card.EndDivineEot = instance.EndDivineEot;
            card.HasBattlecry = instance.HasBattlecry;
            card.HasChoices = instance.HasChoices;
            card.HasDeathRattle = instance.HasDeathRattle;
            card.HasEnrage = instance.HasEnrage;
            card.HasFreeze = instance.HasFreeze;
            card.HasPoison = instance.HasPoison;
            card.Id = instance.Id;
            card.EntityId = instance.EntityId;
            card.Index = instance.Index;
            card.IsBuffer = instance.IsBuffer;
            card.IsCharge = instance.IsCharge;
            card.IsDestroyed = instance.IsDestroyed;
            card.IsDestroyedEot = instance.IsDestroyedEot;
            card.IsDivineShield = instance.IsDivineShield;
            card.IsEnraged = instance.IsEnraged;
            card.IsFriend = instance.IsFriend;
            card.IsFrozen = instance.IsFrozen;
            card.IsImmune = instance.IsImmune;
            card.IsInspire = instance.IsInspire;
            card.IsSilenced = instance.IsSilenced;
            card.IsStealth = instance.IsStealth;
            card.IsStuck = instance.IsStuck;
            card.IsTargetable = instance.IsTargetable;
            card.IsTaunt = instance.IsTaunt;
            card.IsTired = instance.IsTired;
            card.IsUsed = instance.IsUsed;
            card.UsedCount = instance.UsedCount;
            card.MaxUsedCount = instance.MaxUsedCount;
            card.IsWindfury = instance.IsWindfury;
            card.LostHealth = instance.LostHealth;
            card.MaxHealth = instance.MaxHealth;
            card.Overload = instance.Overload;
            card.Proc = instance.Proc;
            card.ProcSecret = instance.ProcSecret;
            card.ProcEnch = instance.ProcEnch;
            card.Race = instance.Race;
            card.SpellPower = instance.SpellPower;
            card.TargetOnlyWhenCombo = instance.TargetOnlyWhenCombo;
            card.TargetTypeOnPlay = instance.TargetTypeOnPlay;
            card.TempAtk = instance.TempAtk;
            card.TempHealth = instance.TempHealth;
            card.Template = instance.Template;
            card.TestAllIndexOnPlay = instance.TestAllIndexOnPlay;
            card.Type = instance.Type;
            card.WindfuryEot = instance.WindfuryEot;
            return card;
        }

        public virtual Card Create()
        {
            return new Card();
        }

        public bool HasBuff(Card buffer)
        {
            Buff buff = Buffs.FirstOrDefault(b => b.CardId == buffer.CardId);
            return (buff != null);
        }

        public virtual void RemoveBuff(Card buffedCard)
        {
            Buff buff = buffedCard.Buffs.FirstOrDefault(b => b.CardId == CardId);
            if (buff != null)
            {
                buffedCard.Buffs.Remove(buff);
            }
        }

        public virtual void AddBuff(Card buffedCard)
        {
            Buff checkBuff = buffedCard.Buffs.FirstOrDefault(b => b.CardId == CardId);
            if (checkBuff != null)
            {
                RemoveBuff(buffedCard);
            }
            buffedCard.Buffs.Add(new Buff(CardId, EntityId));
        }
        public virtual void AddEnchant(Card enchantCard)
        {
            Enchants.Add(new Enchant(enchantCard));
        }
        public virtual void RemoveEnchant(Enchant enchantCard)
        {
            Enchants.Remove(enchantCard);
        }
        public void Damage(int amount, Board board)
        {
            if (IsImmune)
            {
                return;
            }

            if (IsDivineShield && amount > 0)
            {
                if (board.IsOwnTurn)
                {
                    board.WastedAtk += amount - 1;
                }
                else
                {
                    board.WastedEnemyATK += amount - 1;
                }
                IsDivineShield = false;
            }
            else
            {
                int resultHealth = amount - (CurrentHealth + CurrentArmor);
                if (resultHealth < 0)
                    resultHealth = 0;

                if (board.IsOwnTurn)
                {
                    board.WastedAtk += resultHealth;
                }
                else
                {
                    board.WastedEnemyATK += resultHealth;
                }

                if (CurrentArmor > 0)
                {
                    int num = amount - CurrentArmor;
                    if (num <= 0) num = 0;

                    CurrentHealth -= num;
                    CurrentArmor -= amount;
                    if (CurrentArmor <= 0)
                    {
                        CurrentArmor = 0;
                    }
                }
                else
                {
                    CurrentHealth -= amount;
                }

                if (CurrentHealth < MaxHealth && HasEnrage && !IsEnraged && !IsSilenced)
                {
                    OnEnrage(true, board);
                    IsEnraged = true;
                }

                if (CurrentHealth <= 0)
                {
                    IsDestroyed = true;
                }
            }

            if (!IsSilenced)
            {
                OnDamage(board, amount);
            }

            if (Type == CardTypes.CType.HERO || Type == CardTypes.CType.WEAPON) return;

            foreach (var minion in board.MinionFriend)
            {
                if (minion.Id != Id && !minion.IsSilenced)
                {
                    minion.OnOtherMinionDamage(board, this, amount);
                }
            }
            foreach (var minion in board.MinionEnemy)
            {
                if (minion.Id != Id && !minion.IsSilenced)
                {
                    minion.OnOtherMinionDamage(board, this, amount);
                }
            }
        }
        public bool Equals(Card c)
        {
            if (c == null)
            {
                return false;
            }
            return CurrentHealth == c.CurrentHealth &&
                   MaxHealth == c.MaxHealth &&
                   CurrentAtk == c.CurrentAtk &&
                    CountAttack == c.CountAttack &&
                    IsDivineShield == c.IsDivineShield &&
                    IsDestroyedEot == c.IsDestroyedEot &&
                    IsTaunt == c.IsTaunt &&
                    IsTargetable == c.IsTargetable &&
                    IsStuck == c.IsStuck &&
                    IsFrozen == c.IsFrozen &&
                    CurrentCost == c.CurrentCost &&
                    IsSilenced == c.IsSilenced &&
                    IsStealth == c.IsStealth &&
                    IsCharge == c.IsCharge &&
                    IsWindfury == c.IsWindfury &&
                    (c.Template.Id == Template.Id);
        }

        public void Heal(int amount, Board board)
        {
            if (amount < 0)
            {
                Damage(-amount, board);
            }
            else if (CurrentHealth != MaxHealth)
            {
                //board.WastedHeal += (worst < 0 && IsFriend) ? (worst * -1) : 0;

                if (!IsSilenced)
                {
                    OnHeal(board);
                }
                switch (Type)
                {
                    case CardTypes.CType.MINION:
                        foreach (var minion in board.MinionFriend)
                        {
                            if (!minion.IsSilenced)
                            {
                                minion.OnOtherMinionHeal(board, this);
                            }
                        }
                        foreach (var minion in board.MinionEnemy)
                        {
                            if (!minion.IsSilenced)
                            {
                                minion.OnOtherMinionHeal(board, this);
                            }
                        }
                        if (CurrentHealth + amount < MaxHealth)
                        {
                            CurrentHealth += amount;
                            return;
                        }
                        else
                        {
                            CurrentHealth = MaxHealth;
                        }
                        if (HasEnrage && IsEnraged && !IsSilenced)
                        {
                            OnEnrage(false, board);
                            IsEnraged = false;
                        }
                        break;
                    case CardTypes.CType.HERO:
                        if (CurrentHealth + amount > 30)
                        {
                            CurrentHealth = 30;
                        }
                        else
                        {
                            CurrentHealth += amount;
                        }
                        OnOtherMinionHeal(board, this);
                        break;
                }
            }
            else if(CurrentHealth == MaxHealth)
            {
                //board.WastedHeal += (worst < 0) ? (worst * -1) : 0;
            }
        }
        public bool HasEnchantment(string enchant)
        {
            foreach (Buff ench in Buffs)
            {
                if (ench.CardId == null)
                    continue;
                if (ench.CardId.Contains(enchant))
                {
                    return true;
                }
            }
            return false;
        }
        public void Silence(Board board)
        {
            IsTaunt = false;
            IsDivineShield = false;
            IsWindfury = false;

            IsFrozen = false;
            IsCharge = false;

            IsSilenced = true;
            IsBuffer = false;

            HasDeathRattle = false;
            HasPoison = false;
            IsStuck = false;

            if (IsEnraged)
            {
                OnEnrage(false, board);
            }
            IsEnraged = false;

            if (MaxHealth > Template.Health)
            {
                MaxHealth = Template.Health;
            }
            else if (MaxHealth <= Template.Health)
            {
                MaxHealth = Template.Health;
            }

            if (CurrentHealth >= MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            TempAtk = 0;
            TempHealth = 0;
            OnSilence(board);
            Buffs.Clear();
        }
        public List<Card> GetTargets(Board board)
        {
            List<Card> result = new List<Card>();
            switch (TargetTypeOnPlay)
            {
                case CardTypes.TargetType.HERO_FRIEND:
                    if (GetTargetHeroFriend(board) != null)
                    {
                        result.Add(GetTargetHeroFriend(board));
                    }
                    break;

                case CardTypes.TargetType.HERO_ENEMY:
                    if (GetTargetHeroEnemy(board) != null)
                        result.Add(GetTargetHeroEnemy(board));
                    break;

                case CardTypes.TargetType.HERO_BOTH:
                    if (GetTargetHeroEnemy(board) != null)
                    {
                        result.Add(GetTargetHeroEnemy(board));
                    }
                    if (GetTargetHeroFriend(board) != null)
                    {
                        result.Add(GetTargetHeroFriend(board));
                    }
                    break;

                case CardTypes.TargetType.MINION_FRIEND:
                    if (GetTargetMinionsFriend(board).Count > 0)
                    {
                        result.AddRange(GetTargetMinionsFriend(board));
                    }
                    break;

                case CardTypes.TargetType.MINION_ENEMY:
                    if (GetTargetMinionsEnemy(board).Count > 0)
                    {
                        result.AddRange(GetTargetMinionsEnemy(board));
                    }
                    break;

                case CardTypes.TargetType.MINION_BOTH:
                    if (GetTargetMinionsEnemy(board).Count > 0)
                    {
                        result.AddRange(GetTargetMinionsEnemy(board));
                    }
                    if (GetTargetMinionsFriend(board).Count > 0)
                    {
                        result.AddRange(GetTargetMinionsFriend(board));
                    }
                    break;

                case CardTypes.TargetType.BOTH_FRIEND:
                    if (GetTargetHeroFriend(board) != null)
                    {
                        result.Add(GetTargetHeroFriend(board));
                    }
                    if (GetTargetMinionsFriend(board).Count > 0)
                    {
                        result.AddRange(GetTargetMinionsFriend(board));
                    }
                    break;

                case CardTypes.TargetType.BOTH_ENEMY:
                    if (GetTargetHeroEnemy(board) != null)
                    {
                        result.Add(GetTargetHeroEnemy(board));
                    }
                    if (GetTargetMinionsEnemy(board).Count > 0)
                    {
                        result.AddRange(GetTargetMinionsEnemy(board));
                    }
                    break;

                case CardTypes.TargetType.ALL:
                    if (GetTargetHeroEnemy(board) != null)
                    {
                        result.Add(GetTargetHeroEnemy(board));
                    }
                    if (GetTargetHeroFriend(board) != null)
                    {
                        result.Add(GetTargetHeroFriend(board));
                    }
                    if (GetTargetMinionsEnemy(board).Count > 0)
                    {
                        result.AddRange(GetTargetMinionsEnemy(board));
                    }
                    if (GetTargetMinionsFriend(board).Count > 0)
                    {
                        result.AddRange(GetTargetMinionsFriend(board));
                    }
                    //{
                        //result.Add(this.GetTargetHeroFriend(board));
                    //}
                    //if (GetTargetMinionsFriend(board).Count > 0)
                    //{
                        //result.AddRange(GetTargetMinionsFriend(board));
                    //}
                    //if (GetTargetHeroEnemy(board) != null)
                    //{
                        //result.Add(GetTargetHeroEnemy(board));
                    //}
                    //if (GetTargetMinionsEnemy(board).Count > 0)
                    //{
                        //result.AddRange(GetTargetMinionsEnemy(board));
                    //}
                    break;
            }
            return result;
        }
        public List<Card> GetRealTargets(Board board)
        {
            List<Card> result = new List<Card>();
            foreach (Card card in GetTargets(board))
            {
                if (Behavior.ShouldBePlayedOnTarget(board, card))
                {
                    if (Type == CardTypes.CType.SPELL && card.IsImmune || card.IsStealth)
                    {
                        continue;
                    }
                    result.Add(card);
                }
            }
            return result;
        }
        private static Card GetTargetHeroFriend(Board board)
        {
            return board.HeroFriend;
        }
        private static Card GetTargetHeroEnemy(Board board)
        {
            return board.HeroEnemy;
        }
        private List<Card> GetTargetMinionsFriend(Board board)
        {
            List<Card> result = new List<Card>();

            foreach (var card in board.MinionFriend)
            {
                if (Type == CardTypes.CType.HEROPOWER || card.Type == CardTypes.CType.SPELL)
                {
                    if (!card.IsTargetable)
                        continue;
                }
                if (!card.IsStealth)
                {
                    result.Add(card);
                }
            }
            return result;
        }

        private List<Card> GetTargetMinionsEnemy(Board board)
        {
            List<Card> result = new List<Card>();
            foreach (var card in board.MinionEnemy)
            {
                if (Type == CardTypes.CType.HEROPOWER || card.Type == CardTypes.CType.SPELL)
                {
                    if (!card.IsTargetable || card.IsStealth)
                        continue;
                }

                result.Add(card);
            }

            return result;
        }

        public virtual float GetValue(Board board)
        {
            // Value //
            float value = 0f;

            if (Type == CardTypes.CType.MINION)
            {
                var atackValue = (IsFriend) ? board.ValuesMgr.ValueAttackMinionFriend : board.ValuesMgr.ValueAttackMinionEnemy;
                var healthValue = (IsFriend) ? board.ValuesMgr.ValueHealthMinionFriend : board.ValuesMgr.ValueHealthMinionEnemy;

                // Стартовая точка считается case 7
                if (!IsStuck && CurrentAtk == 0)
                {
                    if (!IsSilenced)
                    {
                        var num4 = CurrentAtk / 8f;
                        if (num4 <= 1f) num4 = 1f;

                        value += (num4 * CurrentHealth);
                    }
                }

                value += (healthValue * CurrentHealth);
                value += ((MaxHealth - CurrentHealth) * healthValue) * 0.2f;

                if (!IsEnraged)
                {
                    value += (atackValue * CurrentAtk);
                }

                if (IsTaunt)
                {
                    if (board.GetHeroFriendHpAndArmor() > 8)
                    {
                        value += board.ValuesMgr.ValueTaunt;
                    }
                    else if (board.GetHeroFriendHpAndArmor() <= 5)
                    {
                        value += board.ValuesMgr.ValueTaunt * 3;
                    }
                }

                if (IsDivineShield)
                {
                    value += board.ValuesMgr.ValueDivineShield * CurrentAtk;
                }

                if (IsFrozen)
                {
                    value -= board.ValuesMgr.ValueFrozen;
                }

                if (HasDeathRattle)
                {
                    value += 3f;
                }

                if (IsBuffer && !IsFriend)
                {
                    value += 5f;
                }

                if (!IsFriend && CurrentAtk == 0 && !IsSilenced)
                {
                    value += SpellPower * 2;
                }
                else if (!IsSilenced && CurrentAtk > 0)
                {
                    value += SpellPower;
                }

                if (IsStealth)
                {
                    value++;
                }

                if (IsWindfury && IsFriend)
                {
                    value++;
                }
                else if (IsWindfury && !IsFriend)
                {
                    value += 5f;
                }

                if (board.EnemyClass == CardTypes.CClass.PRIEST)
                {
                    if (IsFriend && CurrentAtk > 4)
                    {
                        if (Template.Atk <= 4)
                        {
                            value /= 3f;
                        }
                    }
                }

                if (board.EnemyClass == CardTypes.CClass.DRUID && IsFriend)
                {
                    if (CurrentHealth <= 1 && Template.Health > 1 && board.EnemyCardCount > 3 && board.TurnCount > 4)
                    {
                        value -= value / 3f;
                    }
                }
            }
            else if (Type == CardTypes.CType.WEAPON)
            {
                value += (board.ValuesMgr.ValueDurabilityWeapon * CurrentDurability);
                value += (board.ValuesMgr.ValueAttackWeapon * CurrentAtk);
            }
            else if (Type == CardTypes.CType.SPELL)
            {
                //value += Behavior.GetHandValue(board);
            }
            return value;
        }
        public virtual void OnAttack(Board board, Card target)
        {
            if (target == null) return;

            if (Type == CardTypes.CType.WEAPON && !target.IsFriend && board.FriendWeapon != null)
            {
                board.FriendWeapon.OnWeaponAttack(board, target);
            }
            else if (Type == CardTypes.CType.WEAPON && target.IsFriend)
            {
                board.EnemyWeapon?.OnWeaponAttack(board, target);
            }

            ProcEnchantments(board, CardTypes.ProcEnchType.ATTACK);

            if (Type == CardTypes.CType.HERO)
            {
                TriggerFriendSecret(board, ref target, CardTypes.ProcSecretType.HERO_ATK);
            }
            if (Type == CardTypes.CType.MINION)
            {
                TriggerFriendSecret(board, ref target, CardTypes.ProcSecretType.MINION_ATK);
            }

            if (target.Type == CardTypes.CType.HERO)
            {
                TriggerFriendSecret(board, ref target, CardTypes.ProcSecretType.ATK_HERO);
            }
            if (target.Type == CardTypes.CType.MINION)
            {
                TriggerFriendSecret(board, ref target, CardTypes.ProcSecretType.ATK_MINION);
            }

            if (Type == CardTypes.CType.HERO && target.Type == CardTypes.CType.HERO)
            {
                TriggerFriendSecret(board, ref target, CardTypes.ProcSecretType.HERO_ATK_HERO);
            }

            if (Type == CardTypes.CType.HERO && target.Type == CardTypes.CType.MINION)
            {
                TriggerFriendSecret(board, ref target, CardTypes.ProcSecretType.HERO_ATK_MINION);
            }

            if (target.Type == CardTypes.CType.HERO && Type == CardTypes.CType.MINION)
            {
                TriggerFriendSecret(board, ref target, CardTypes.ProcSecretType.MINION_ATK_HERO);
            }

            if (target.Type == CardTypes.CType.MINION && Type == CardTypes.CType.MINION)
            {
                TriggerFriendSecret(board, ref target, CardTypes.ProcSecretType.MINION_ATK_MINION);
            }

            if (board.GetCard(Id) == null)
            {
                return;
            }

            int resultHealth = CurrentAtk - target.CurrentHealth;
            if (resultHealth > 0 && board.IsOwnTurn)
            {
                board.WastedAtk += (resultHealth);
            }
            if (resultHealth > 0 && !board.IsOwnTurn)
            {
                board.WastedEnemyATK += (resultHealth);
            }

            switch (Type)
            {
                case CardTypes.CType.WEAPON:
                    CurrentDurability--;
                    CountAttack++;

                    if (IsFriend)
                    {
                        board.HeroFriend.CountAttack++;

                        target.OnHit(board, board.HeroFriend);

                        if (target.Type != CardTypes.CType.HERO)
                        {
                            if (board.FriendWeapon != null && board.FriendWeapon.Template.Id != "DS1_188")
                            {
                                board.HeroFriend.OnHit(board, target);
                            }
                        }

                        if (CurrentDurability < 1)
                        {
                            OnWeaponDeath(board);
                            return;
                        }
                    }
                    else
                    {
                        board.HeroEnemy.CountAttack++;
                        //board.HeroEnemy.TempAtk += currentAtk;

                        target.OnHit(board, board.HeroEnemy);

                        if (target.Type != CardTypes.CType.HERO)
                        {
                            if (board.HeroEnemy.Template.Id != "DS1_188")
                            {
                                board.HeroEnemy.OnHit(board, target);
                            }
                        }

                        if (CurrentDurability < 1)
                        {
                            OnWeaponDeath(board);

                        }
                    }
                    return;
                case CardTypes.CType.HERO:
                    CountAttack++;
                    OnHit(board, target);
                    target.OnHit(board, this);
                    return;
            }

            CountAttack++;
            //Card actor = Clone(this);

            target.OnHit(board, this);
            if (target.Type != CardTypes.CType.HERO)
            {
                OnHit(board, target);
            }
        }

        private void TriggerFriendSecret(Board board, ref Card target, CardTypes.ProcSecretType type)
        {
            foreach (var secret in board.Secret)
            {
                secret.OnTriggerSecret(board, this, ref target, type);
            }
        }

        public virtual void OnPlay(Board board, Card target = null, int index = -1, int choice = 0)
        {
            switch (Type)
            {
                case CardTypes.CType.HEROPOWER:
                    if (board.HeroAbility != null && IsFriend && !board.HeroAbility.IsUsed)
                    {
                        board.PlayAbility();
                        board.MinionFriend.Where(c => !c.IsSilenced).ToList().ForEach(i => i.OnPlayAbility(board, IsFriend));
                    }
                    if (board.EnemyAbility != null && !IsFriend && !board.EnemyAbility.IsUsed)
                    {
                        board.PlayEnemyAbility();

                        board.MinionEnemy.Where(c => !c.IsSilenced).ToList().ForEach(i => i.OnPlayAbility(board, IsFriend));
                    }
                    break;
                case CardTypes.CType.SPELL:
                case CardTypes.CType.WEAPON:
                    if (!IsFriend)
                    {
                        if (HasChoices)
                        {
                            board.EnemyManaAvailable -= CurrentCost;
                            board.EnemyHand.Remove(this);
                        }
                        else
                        {
                            board.PlaySpell(Id);
                        }
                    }
                    else
                    {
                        if (HasChoices)
                        {
                            board.ManaAvailable -= CurrentCost;
                            board.RemoveCardFromHand(Id);
                        }
                        else
                        {
                            board.PlaySpell(Id);
                        }
                    }
                    break;
                case CardTypes.CType.MINION:
                    if (IsFriend)
                    {
                        if (HasChoices)
                        {
                            board.ManaAvailable -= CurrentCost;
                            board.RemoveCardFromHand(Id);
                        }
                        else
                        {
                            board.PlayMinion(Id, index);
                        }
                    }
                    else
                    {
                        if (HasChoices)
                        {
                            board.EnemyManaAvailable -= CurrentCost;
                            board.EnemyHand.Remove(this);
                        }
                        else
                        {
                            board.PlayEnemyMinion(Id, index);
                        }
                    }
                    break;
            }
            if(IsFriend)
                board.GlobalModifier += Behavior.GetActionValue(board);
            else
                board.GlobalModifier -= Behavior.GetActionValue(board);
        }

        public virtual void OnPlayAbility(Board board, bool isFriend)
        {
        }

        public virtual void OnPlayOtherMinion(Board board, Card minion)
        {
        }

        public virtual void OnHit(Board board, Card actor)
        {
            if (IsImmune)
            {
                return;
            }

            if (actor.HasFreeze)
            {
                IsFrozen = true;
            }
            if (actor.HasPoison && !IsDivineShield)
            {
                IsDestroyed = true;
            }
            else
            {
                Damage(actor.CurrentAtk, board);
            }
        }

        public virtual void OnUpdate(Board board)
        {
            if (CurrentHealth < 1)
            {
                IsDestroyed = true;
            }
            if (Buffs.Count <= 0) return;
            if (HasEnchantment("EX1_316"))
            {
                IsDestroyedEot = true;
            }
        }

        public virtual void OnEndTurn(Board board)
        {
        }

        public virtual bool OnTriggerSecret(Board board, Card Actor, ref Card target, CardTypes.ProcSecretType type)
        {
            if (ProcSecret == type)
            {
                board.RemoveSecret(Template.Id);
            }
            return true;
        }

        public virtual void OnPlaySecret(Board board, Card Secret)
        {
        }

        public void ProcEnchantments(Board board, CardTypes.ProcEnchType proc)
        {
            Card th = this;
            foreach(Enchant ench in Enchants)
            {
                if (proc == ench.EnchantCard.ProcEnch)
                    ench.EnchantCard.ProcEnchantment(board, proc, th);
            }
            Enchants.RemoveAll(e => e.EnchantCard.EnchantRemove);
        }

        public virtual void ProcEnchantment(Board board, CardTypes.ProcEnchType proc, Card enchantedCard)
        {

        }

        public virtual void OnCardDraw(Board board, bool isFriend)
        {

        }

        public virtual void OnSilence(Board board)
        {
        }
        public virtual void OnDeath(Board board)
        {
            if (!IsDestroyed) return;

            if (IsTired)
                board.WastedMinion += (int)((Template.Atk + Template.Health) * 0.1f);

            board.MinionFriend.Where(m => m.Id != Id).ToList().ForEach(a => a.OnOtherMinionDeath(board, IsFriend, this));
            board.MinionEnemy.Where(m => m.Id != Id).ToList().ForEach(a => a.OnOtherMinionDeath(board, IsFriend, this));

            if (IsFriend)
            {
                board.FriendGraveyardTurn.Add(Template.Id);
            }
            else
            {
                board.EnemyGraveyardTurn.Add(Template.Id);
            }
        }

        public virtual void OnOtherMinionDeath(Board board, bool friend, Card minion)
        {
        }

        public virtual void OnDamage(Board board, int amount)
        {
        }

        public virtual void OnOtherMinionDamage(Board board, Card minionDamaged, int amount)
        {
        }

        public virtual void OnEnrage(bool enraged, Board board)
        {
        }

        public virtual void OnHeal(Board board)
        {
        }
        public virtual void OnCastWeapon(Board board, Card weapon)
        {
        }
        public virtual void OnUpdateHand(Board board)
        {
        }

        public virtual void OnOtherMinionHeal(Board board, Card minionHealed)
        {
        }
        public virtual void OnBeginTurn(Board board)
        {
        }
        public virtual void OnCastSpell(Board board, Card Spell)
        {
        }

        public virtual void OnWeaponAttack(Board board, Card target)
        {
        }

        public virtual void OnWeaponDeath(Board board)
        {
            if (IsFriend)
            {
                board.DeleteWeapon();
            }
            else
            {
                board.EnemyWeapon = null;
            }
        }
    }
}
