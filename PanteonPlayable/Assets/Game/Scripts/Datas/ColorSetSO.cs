using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Datas
{
    [CreateAssetMenu(fileName = "ColorSet", menuName = "Datas/ColorSetSO")]
    public class ColorSetSO : ScriptableObject
    {
        public Material[] colorMaterials;

        public Material GetRandomColorMaterial()
        {
            return colorMaterials[Random.Range(0, colorMaterials.Length)];
        }
    }
}