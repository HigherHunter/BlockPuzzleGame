using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Core
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Shape[] allShapes;
        private float[] _shapeRotations;

        private void Start()
        {
            _shapeRotations = new[]
            {
                0, 90f, 180f, 270f
            };
        }

        public Shape SpawnShapeAtPositionWithScaleAndRandomRotation(Vector3 pos, Vector3 scale)
        {
            Shape shape = Instantiate(GetRandomShape(), pos, Quaternion.Euler(0, 0, GetRandomZaxisRotationForShapes()));
            shape.transform.localScale = scale;
            shape.transform.parent = transform;

            if (shape)
                return shape;

            Debug.LogWarning("Invalid shape");
            return null;
        }

        private Shape GetRandomShape()
        {
            int i = Random.Range(0, allShapes.Length);

            if (allShapes[i])
                return allShapes[i];

            Debug.LogWarning("Invalid shape");
            return null;
        }

        private float GetRandomZaxisRotationForShapes()
        {
            int i = Random.Range(0, _shapeRotations.Length);

            return _shapeRotations[i];
        }

        public void ChangeShapesColor(ColorPalette palette)
        {
            foreach (Shape shape in allShapes)
            {
                ChangeShapeColor(shape, palette);
            }

            foreach (Transform shapeObj in transform)
            {
                Shape shape = shapeObj.GetComponent<Shape>();
                ChangeShapeColor(shape, palette);
            }
        }

        private static void ChangeShapeColor(Shape shape, ColorPalette palette)
        {
            switch (shape.tag)
            {
                case "ShapeCorner2x2":
                    ChangeBlocksColorInShape(shape, palette.shapeCorner2X2Color);
                    shape.ChangeInitialColor(palette.shapeCorner2X2Color);
                    break;
                case "ShapeCorner3x3":
                    ChangeBlocksColorInShape(shape, palette.shapeCorner3X3Color);
                    shape.ChangeInitialColor(palette.shapeCorner3X3Color);
                    break;
                case "ShapeLine2":
                    ChangeBlocksColorInShape(shape, palette.shapeLine2);
                    shape.ChangeInitialColor(palette.shapeLine2);
                    break;
                case "ShapeLine3":
                    ChangeBlocksColorInShape(shape, palette.shapeLine3);
                    shape.ChangeInitialColor(palette.shapeLine3);
                    break;
                case "ShapeLine4":
                    ChangeBlocksColorInShape(shape, palette.shapeLine4);
                    shape.ChangeInitialColor(palette.shapeLine4);
                    break;
                case "ShapeLine5":
                    ChangeBlocksColorInShape(shape, palette.shapeLine5);
                    shape.ChangeInitialColor(palette.shapeLine5);
                    break;
                case "ShapeSingleBlock":
                    ChangeBlocksColorInShape(shape, palette.shapeSingleBlock);
                    shape.ChangeInitialColor(palette.shapeSingleBlock);
                    break;
                case "ShapeSquare2x2":
                    ChangeBlocksColorInShape(shape, palette.shapeSquare2X2);
                    shape.ChangeInitialColor(palette.shapeSquare2X2);
                    break;
                case "ShapeSquare3x3":
                    ChangeBlocksColorInShape(shape, palette.shapeSquare3X3);
                    shape.ChangeInitialColor(palette.shapeSquare3X3);
                    break;
                default:
                    Debug.Log("Invalid shape!");
                    break;
            }
        }

        // Change the color of individual blocks in each shape
        private static void ChangeBlocksColorInShape(Shape shape, Color color)
        {
            foreach (Transform block in shape.transform.GetChild(0).transform)
            {
                block.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}