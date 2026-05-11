using FinalProject.Interfaces;
using System.Collections.Generic;

namespace FinalProject.Classes
{
    public abstract class Character : ICombatant
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public virtual int Defense { get; set; }
        public int Wisdom { get; set; }

        // <-- Abilities are now at Character level
        [System.Text.Json.Serialization.JsonIgnore]
        public List<IAbility> Abilities { get; set; } = new List<IAbility>();

        public int ScottyCallCount { get; set; } = 0; // if fighting tribbles, scotty is only successful on the third attempt.  
        public int StunnedTurns { get; set; } = 0;

        // Temporary Stat Mods
        public int TempStrengthMod { get; set; } = 0;
        public int TempDefenseMod { get; set; } = 0;
        public int StatusDuration { get; set; } = 0;

        public void UpdateStatus()
        {
            if (StatusDuration > 0)
            {
                StatusDuration--;
                if (StatusDuration == 0)
                {
                    TempStrengthMod = 0;
                    TempDefenseMod = 0;
                }
            }
        }

        // Helper property for JSON serialization
        public List<string> AbilityNames
        {
            get => Abilities.ConvertAll(a => a.GetType().Name);
            set
            {
                Abilities = new List<IAbility>();
                foreach (var name in value)
                {
                    switch (name)
                    {
                        case "VulcanNervePinch": Abilities.Add(new FinalProject.Players.VulcanNervePinch()); break;
                        case "LogicalCounterattack": Abilities.Add(new FinalProject.Players.LogicalCounterattack()); break;
                        case "BatlethSlash": Abilities.Add(new FinalProject.Players.BatlethSlash()); break;
                        case "WarriorsRoar": Abilities.Add(new FinalProject.Players.WarriorsRoar()); break;
                        case "SuckerPunch": Abilities.Add(new FinalProject.Players.SuckerPunch()); break;
                        case "CommandersGambit": Abilities.Add(new FinalProject.Players.CommandersGambit()); break;
                        case "InspiringSpeech": Abilities.Add(new FinalProject.Players.InspiringSpeech()); break;
                        case "PrecisionStrike": Abilities.Add(new FinalProject.Players.PrecisionStrike()); break;
                        case "CallScottyAbility": Abilities.Add(new FinalProject.Players.CallScottyAbility()); break;
                    }
                }
            }
        }

        protected Character() { }

        protected Character(string name, int hp, int str, int dex, int def, int wis, List<IAbility> abilities = null)
        {
            Name = name;
            MaxHP = hp;
            CurrentHP = hp;
            Strength = str;
            Dexterity = dex;
            Defense = def;
            Wisdom = wis;
            Abilities = abilities ?? new List<IAbility>();
        }

        public virtual int DealDamage(bool hasWeapon = false)
        {
            int baseDamage = Strength + TempStrengthMod;
            if (hasWeapon)
                baseDamage += 45; // Increased bonus for Phaser/Weapon

            return System.Math.Max(0, baseDamage);
        }

        public virtual int DealDamage() => DealDamage(false);

        public virtual int TakeDamage(int amount)
        {
            int effectiveDefense = Defense + TempDefenseMod;
            int damageTaken = System.Math.Max(amount - effectiveDefense, 0);
            CurrentHP -= damageTaken;
            if (CurrentHP < 0) CurrentHP = 0;
            return damageTaken;
        }

        public bool IsAlive() => CurrentHP > 0;

        public int Heal(int healAmount)
        {
            CurrentHP += healAmount;
            if (CurrentHP > MaxHP) CurrentHP = MaxHP;
            return CurrentHP;
        }

        // Combat Dialogue Methods
        public virtual string GetAttackMessage(int damage, string targetName)
        {
            return $"{Name} attacks {targetName} for {damage} damage!";
        }

        public virtual string GetDefeatMessage()
        {
            return $"{Name} defeated!";
        }

        public virtual string GetHitMessage(int damage)
        {
            return null; // Default: no special message on hit
        }
    }
}
