
using Arcatech.Stats;
using AYellowpaper.SerializedCollections;
using UnityEngine;
namespace Arcatech
{
    [CreateAssetMenu(menuName = "Configurations/Shield")]
    public class GeneratorSettings : ScriptableObjectID
    {

        public SerializedDictionary<GeneratorStatType, StatValueContainer> Stats;

    }

}