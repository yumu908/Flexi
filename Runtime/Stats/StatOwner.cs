using System.Collections.Generic;
using UnityEngine;

namespace Physalia.AbilitySystem
{
    public class StatOwner
    {
        private readonly int id;
        private readonly StatDefinitionTable table;
        private readonly StatOwnerRepository repository;

        private readonly Dictionary<int, Stat> stats = new();
        private readonly HashSet<StatModifierInstance> modifiers = new();

        private bool isValid = true;

        public int Id => id;

        internal IReadOnlyDictionary<int, Stat> Stats => stats;
        internal IReadOnlyCollection<StatModifierInstance> Modifiers => modifiers;

        internal StatOwner(int id, StatDefinitionTable table, StatOwnerRepository repository)
        {
            this.id = id;
            this.table = table;
            this.repository = repository;
        }

        public bool IsValid()
        {
            return isValid;
        }

        public void AddStat(int statId, int baseValue)
        {
            StatDefinition definition = table.GetStatDefinition(statId);
            if (definition == null)
            {
                Debug.LogError($"Create stat failed! See upon messages for details");
                return;
            }

            if (stats.ContainsKey(statId))
            {
                Debug.LogError($"Create stat failed! Owner {id} already has stat {definition}");
                return;
            }

            var stat = new Stat(definition, baseValue);
            stats.Add(definition.Id, stat);
        }

        public void RemoveStat(int statId)
        {
            stats.Remove(statId);
        }

        public Stat GetStat(int statId)
        {
            if (!stats.TryGetValue(statId, out Stat stat))
            {
                return null;
            }

            return stat;
        }

        public void SetStat(int statId, int newBase)
        {
            if (stats.TryGetValue(statId, out Stat stat))
            {
                stat.CurrentBase = newBase;
            }
        }

        public void AppendModifier(StatModifierInstance modifier)
        {
            modifiers.Add(modifier);
        }

        public void RemoveModifier(StatModifierInstance modifier)
        {
            modifiers.Remove(modifier);
        }

        public void ClearAllModifiers()
        {
            modifiers.Clear();
        }

        internal void ResetAllStats()
        {
            foreach (Stat stat in stats.Values)
            {
                stat.CurrentValue = stat.CurrentBase;
            }
        }

        public void Destroy()
        {
            isValid = false;
            repository.RemoveOwner(this);
        }
    }
}
