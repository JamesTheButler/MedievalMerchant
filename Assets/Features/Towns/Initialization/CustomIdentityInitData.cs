using System;
using UnityEngine;

namespace Features.Towns.Initialization
{
    [Serializable]
    public sealed class CustomIdentityInitData : IdentityInitData
    {
        [SerializeField]
        private string name;
        
        public override void Initialize(Town town)
        {
            town.Name = name;

        }
    }
}