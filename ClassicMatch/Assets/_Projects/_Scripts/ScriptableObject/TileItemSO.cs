using _Projects._Scripts.Enum;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Projects._Scripts.ScriptableObject
{
    [CreateAssetMenu(fileName = "Tile Item", menuName = "New TileItem", order = 0)]
    public class TileItemSO : UnityEngine.ScriptableObject
    {
        public TileItemType type;
        public Sprite iconSprite;
    }
}