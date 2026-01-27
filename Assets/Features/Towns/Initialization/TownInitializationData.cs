using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Towns.Initialization
{
    [Serializable]
    public sealed class TownInitializationData
    {
        [field: SerializeReference, SubclassSelector]
        public IdentityInitData Identity { get; private set; } = new RandomIdentityInitData();

        [field: SerializeReference, SubclassSelector]
        public ProductionInitData Production { get; private set; } = new RandomProductionInitData();

        [field: SerializeReference, SubclassSelector]
        public List<InitData> OptionalInitDatas { get; private set; } = new();
    }
}