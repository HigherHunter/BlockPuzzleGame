using UnityEngine;

namespace Core
{
    public class Shape : MonoBehaviour
    {
        private Vector3 _initialPosition;
        private Vector3 _initialScale;
        private Vector3 _scaleOnMove;
        private Color _initialColor;

        private void Awake()
        {
            _initialPosition = transform.position;
            _initialScale = transform.localScale;
            _scaleOnMove = new Vector3(1.0f, 1.0f, 1.0f);
            _initialColor = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color;
        }

        public Vector3 GetInitialPosition() => _initialPosition;

        public Vector3 GetScaleOnMove() => _scaleOnMove;

        public void MoveShapeToInitialPosition() => transform.position = new Vector3(_initialPosition.x, _initialPosition.y, _initialPosition.z);

        public void ScaleShapeToInitialScale() => transform.localScale = _initialScale;

        // Number of block pieces the shape contains
        public int NumberOfContainedPieces() => transform.GetChild(0).transform.childCount;

        public void ChangeShapeColor(Color color)
        {
            foreach (Transform child in transform.GetChild(0).transform)
            {
                child.GetComponent<SpriteRenderer>().color = color;
            }
        }

        public void ChangeToInitialColor()
        {
            foreach (Transform child in transform.GetChild(0).transform)
            {
                child.GetComponent<SpriteRenderer>().color = _initialColor;
            }
        }

        public void ChangeInitialColor(Color color)
        {
            _initialColor = color;
        }
    }
}