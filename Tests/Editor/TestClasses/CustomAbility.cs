using UnityEngine;

namespace Physalia.AbilityFramework.Tests
{
    public static class CustomAbility
    {
        public static AbilityGraphAsset HELLO_WORLD
        {
            get
            {
                return ReadAbilityFile("HelloWorld");
            }
        }

        public static AbilityGraphAsset NORAML_ATTACK
        {
            get
            {
                return ReadAbilityFile("NormalAttack");
            }
        }

        public static AbilityGraphAsset NORAML_ATTACK_SELECTION
        {
            get
            {
                return ReadAbilityFile("NormalAttackSelection");
            }
        }

        public static AbilityGraphAsset NORMAL_ATTACK_5_TIMES
        {
            get
            {
                return ReadAbilityFile("NormalAttack5Times");
            }
        }

        public static AbilityGraphAsset ATTACK_DECREASE
        {
            get
            {
                return ReadAbilityFile("AttackDecrease");
            }
        }

        public static AbilityGraphAsset ATTACK_DOUBLE
        {
            get
            {
                return ReadAbilityFile("AttackDouble");
            }
        }

        public static AbilityGraphAsset ATTACK_UP_WHEN_LOW_HEALTH
        {
            get
            {
                return ReadAbilityFile("AttackUpWhenLowHealth");
            }
        }

        public static AbilityGraphAsset ATTACK_DOUBLE_WHEN_DAMAGED
        {
            get
            {
                return ReadAbilityFile("AttackDoubleWhenDamaged");
            }
        }

        public static AbilityGraphAsset COUNTER_ATTACK
        {
            get
            {
                return ReadAbilityFile("CounterAttack");
            }
        }

        private static AbilityGraphAsset ReadAbilityFile(string fileName)
        {
            var asset = Resources.Load<AbilityGraphAsset>(fileName);
            return asset;
        }
    }
}
