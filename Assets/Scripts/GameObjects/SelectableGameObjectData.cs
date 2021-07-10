using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexagon.GameObjects
{
    public enum SelectableGameObjectColor
    {
        Red, Blue, Yellow, Green, Purple, Cyan, White
    }

    [CreateAssetMenu(menuName = "Hexagon/Game Objects/SelectableGameObjectData")]
    public class SelectableGameObjectData : ScriptableObject
    {
        [SerializeField]
        private float _colorAlpha = 0.75f;

        [SerializeField]
        public float LerpSpeed = 10f;

        public readonly float COLUMN_STEP = 1.75f;
        public readonly float ROW_STEP = 1;

        [SerializeField]
        public int ScoreValue;

        public Vector3[,] Corners = {/* TOP RIGHT CORNER */
                                    {Vector3.up, (Vector3.right * 1.73f + Vector3.up).normalized},
                                    /* RIGHT CORNER */
                                    {(Vector3.right * 1.73f + Vector3.up).normalized, (Vector3.right * 1.73f + Vector3.down).normalized},
                                    /* BOTTOM RIGHT CORNER */
                                    {(Vector3.right * 1.73f + Vector3.down).normalized, Vector3.down},
                                    /* BOTTOM LEFT CORNER */
                                    {Vector3.down, (Vector3.left * 1.73f + Vector3.down).normalized},
                                    /* LEFT CORNER */
                                    {(Vector3.left * 1.73f + Vector3.down).normalized, (Vector3.left * 1.73f + Vector3.up).normalized},
                                    /* TOP LEFT CORNER */
                                    {(Vector3.left * 1.73f + Vector3.up).normalized, Vector3.up},
                                    };

        public Color GetColor(SelectableGameObjectColor gameObjectColor)
        {
            Color color;

            switch (gameObjectColor)
                {
                    case SelectableGameObjectColor.Red:
                        color = new Color(1f, 0f, 0f, _colorAlpha);
                        break;
                    case SelectableGameObjectColor.Blue:
                        color = new Color(0f, 0f, 1f, _colorAlpha);
                        break;
                    case SelectableGameObjectColor.Yellow:
                        color = new Color(0.74f, 0.75f, 0.15f, _colorAlpha);
                        break;
                    case SelectableGameObjectColor.Green:
                        color = new Color(0.14f, 0.52f, 0.11f, _colorAlpha);
                        break;
                    case SelectableGameObjectColor.Purple:
                        color = new Color(1f, 0.1f, 1f, _colorAlpha);
                        break;
                    case SelectableGameObjectColor.Cyan:
                        color = new Color(0f, 0.89f, 1f, _colorAlpha);
                        break;
                    case SelectableGameObjectColor.White:
                        color = new Color(0.8f, 0.8f, 0.8f, _colorAlpha);
                        break;
                    default:
                        color = Color.white;
                        break;
                }    

            return color;
        }        
    
    }

    
}
