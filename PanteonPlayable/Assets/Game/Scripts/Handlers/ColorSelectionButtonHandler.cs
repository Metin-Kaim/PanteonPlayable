using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Handlers
{
    public class ColorSelectionButtonHandler : MonoBehaviour
    {
        [SerializeField] private Color selectionColor;

        public Color SelectionColor => selectionColor;
    }
}