
using AYellowpaper.SerializedCollections;
using UnityEngine;
namespace Arcatech
{
    [CreateAssetMenu(menuName = "Configurations/Shield")]
    public class ShieldSettings : ScriptableObjectID
    {

        public SerializedDictionary<ShieldStatType, StatValueContainer> Stats;

    }

}