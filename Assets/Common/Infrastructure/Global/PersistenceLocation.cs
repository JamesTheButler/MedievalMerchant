using System.IO;
using UnityEngine;

namespace Common.Infrastructure.Global
{
    public static class PersistenceLocation
    {
        public static readonly string Root = $"{Application.persistentDataPath}/PlayerSaves";

        public static readonly string Levels = Path.Combine(Root, "Levels");
        public static readonly string Settings = Path.Combine(Root, "Settings");
    }
}