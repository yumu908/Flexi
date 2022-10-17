using UnityEngine;

namespace Physalia.AbilitySystem.Tests
{
    public class AbilitySystemBuilder
    {
        private StatDefinitionListAsset asset;
        private AbilityRunner runner;

        public AbilitySystem Build()
        {
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<StatDefinitionListAsset>();
                Debug.LogWarning($"[{nameof(AbilitySystemBuilder)}] The stat definition asset is null. Internally created an empty one.");
            }

            if (runner == null)
            {
                runner = new DefaultAbilityRunner();
            }

            Debug.Log($"[{nameof(AbilitySystemBuilder)}] Runner Type: {runner.GetType().Name}");

            return new AbilitySystem(asset, runner);
        }

        public void SetStatDefinitions(StatDefinitionListAsset asset)
        {
            this.asset = asset;
        }

        public void SetRunner(AbilityRunner runner)
        {
            this.runner = runner;
        }
    }
}