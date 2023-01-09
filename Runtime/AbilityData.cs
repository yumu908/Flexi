using System;
using System.Collections.Generic;

namespace Physalia.Flexi
{
    [Serializable]
    public class AbilityData
    {
        public string name;
        public List<BlackboardVariable> blackboard = new();
        public List<string> graphJsons = new();
    }
}