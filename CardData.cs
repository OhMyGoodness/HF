using System.Collections.Generic;
using HeartFunnySERVER.Game.CardDB;

namespace HeartFunnySERVER.Game.CardNS
{
    public abstract class CardData
    {
        public string CardId;
        public bool AIFound;

        public string CardTitle { get; set; }

        public int Index { get; set; }

        public int CurrentArmor;
        public int CurrentAtk
        {
            get
            {
                return _currentAtk + TempAtk;
            }

            set
            {
                _currentAtk = value;
            }
        }

        public int GetRealCurrentAtk
        {
            get
            {
                return _currentAtk;
            }
        }
        private int _currentAtk;

        public int TempAtk { get; set; }

        public int CurrentCost { get; set; }

        public int CurrentDurability { get; set; }

        public int EntityId { get; set; }

        public int Id { get; set; }
        
        public int TempHealth { get; set; }

        private int _maxHealth;
        public int MaxHealth
        {
            get
            {
                return _maxHealth + TempHealth;
            }
            set
            {
                _maxHealth = value;
            }
        }
        public int CountAttack { get; set; }

        private int _currentHealth;
        public int CurrentHealth
        {
            get
            {
                return _currentHealth + TempHealth;
            }
            set
            {
                _currentHealth = value;
            }
        }

        public int SpellPower { get; set; }

        public bool IsCharge { get; set; }

        public bool IsDivineShield { get; set; }

        public bool IsTaunt { get; set; }

        public bool IsTired { get; set; }

        public bool IsEnraged { get; set; }

        public bool HasEnrage { get; set; }

        public bool HasFreeze { get; set; }

        public bool IsFrozen { get; set; }

        public bool IsImmune { get; set; }

        public bool HasPoison { get; set; }

        public bool IsSilenced { get; set; }

        public bool IsStealth { get; set; }

        public int CountTurnsInPlay { get; set; }

        public bool IsInspire { get; set; }

        public bool IsWindfury { get; set; }

        public int LostHealth { get; set; }

        public bool IsUsed { get; set; }

        public int UsedCount { get; set; }

        public int MaxUsedCount { get; set; }

        public bool HasChoices { get; set; }

        public bool IsFriend { get; set; }

        public bool IsDestroyed { get; set; }

        public bool HasBattlecry { get; set; }

        private bool _hasDeathRattle;

        public bool HasDeathRattle
        {
            get
            {
                return (_hasDeathRattle && !IsSilenced) && _hasDeathRattle;
            }
            set
            {
                _hasDeathRattle = value;
            }
        }
        public int Overload { get; set; }

        public bool TargetOnlyWhenCombo;

        public CardTypes.TargetType TargetTypeOnPlay;

        public bool EndDivineEot;

        public CardTypes.ProcSecretType ProcSecret;

        public bool IsBuffer;

        public bool WindfuryEot;

        public bool IsStuck;

        public bool IsTargetable;

        public bool TestAllIndexOnPlay;

        public bool IsDestroyedEot;

        public List<Buff> Buffs;

        public List<Enchant> Enchants;

        public bool ChoiceOneTarget;

        public bool ChoiceTwoTarget;

        public bool CanAttack => !IsStuck && (!IsTired || IsCharge) && CurrentAtk >= 1 && !IsFrozen && (CountAttack <= 0 || IsWindfury) && (CountAttack <= 1 || !IsWindfury);

        public bool CanAttackWithWeapon => !IsFrozen && (CountAttack <= 0 || IsWindfury) && (CountAttack <= 1 || !IsWindfury);

        public CardTypes.ProcType Proc { get; set; }

        public CardTypes.ProcEnchType ProcEnch { get; set; }

        public bool EnchantRemove;

        public CardTypes.CType Type { get; set; }

        public CardTypes.CRace Race { get; set; }

        public CardTypes.CClass Class { get; set; }

        public CardTypes.CRarity Rarity { get; set; }

        public Behavior Behavior;

        public CardData(bool enchantRemove = false)
        {
            EnchantRemove = enchantRemove;
        }

        public CardTemplate Template { get; set; }

        public void InitCard(CardTemplate template, bool isFriend, int id, int index = -1)
        {
            CardTitle = template.Id;
            Type = template.Type;
            Index = index;
            CurrentArmor = 0;
            CurrentAtk = template.Atk;
            CurrentCost = template.Cost;
            Class = template.Class;
            LostHealth = 0;
            CurrentDurability = template.Health;
            HasBattlecry = template.HasBattlecry;
            HasDeathRattle = template.HasDeathrattle;
            Overload = template.Overload;
            Id = id;
            MaxHealth = template.Health;
            CountAttack = 0;
            CurrentHealth = template.Health;
            SpellPower = template.Spellpower;
            IsCharge = template.Charge;
            IsDivineShield = template.Divineshield;
            IsTaunt = template.Taunt;
            IsWindfury = template.Windfury;
            IsTired = false;
            HasEnrage = template.Enrage;
            HasFreeze = template.Freeze;
            IsFrozen = false;
            IsImmune = false;
            HasPoison = template.Poison;
            IsSilenced = false;
            IsStealth = template.Stealth;
            IsTargetable = (!IsStealth);
            CountTurnsInPlay = 0;
            IsInspire = template.Inspire;
            Template = template;
            IsFriend = isFriend;
            Race = template.Race;
            MaxUsedCount = 1;
            Buffs = new List<Buff>();
            Enchants = new List<Enchant>();
            Init();
        }

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        public virtual void Init()
        {

        }

        public string GetHash()
        {
            return CardId + Id;
        }
    }

}
