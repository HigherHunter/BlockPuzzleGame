using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Core
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private Transform cell;
        [SerializeField] private int boardHeight, boardWidth;

        private Transform[,] _grid;

        // Every cell piece of a shape that needs to be cleared on row and/or column clearance 
        private HashSet<Transform> _partsInGrid;

        private int _numberOfClearedRows;
        private int _numberOfClearedColumns;

        private void Awake()
        {
            _grid = new Transform[boardWidth, boardHeight];
            _partsInGrid = new HashSet<Transform>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            DrawEmptyCells();
        }

        // Check if every piece of the shape is within the board and on the valid position
        // Used for user interaction
        public bool IsValidPosition(Shape shape)
        {
            foreach (Transform child in shape.transform.GetChild(0).transform)
            {
                float x = (float)Math.Round(Math.Round(child.position.x, 1, MidpointRounding.AwayFromZero), MidpointRounding.AwayFromZero);
                float y = (float)Math.Round(Math.Round(child.position.y, 1, MidpointRounding.AwayFromZero), MidpointRounding.AwayFromZero);
                Vector2 pos = new Vector2(x, y);

                if (!IsWithinBoard((int)pos.x, (int)pos.y))
                    return false;

                if (IsOccupied((int)pos.x, (int)pos.y))
                    return false;
            }
            return true;
        }

        private bool IsWithinBoard(int x, int y)
        {
            return (x >= 0 && x < boardWidth && y >= 0 && y < boardHeight);
        }

        private bool IsOccupied(int x, int y)
        {
            return _grid[x, y] != null;
        }

        // Check if there is any valid position left on the board for the given shape
        public bool CanShapeFitAnywhere(Shape shape)
        {
            int zRotation = (int)shape.transform.rotation.eulerAngles.z;
            if (zRotation == 360)
                zRotation = 0;
            if (shape.CompareTag("ShapeCorner2x2"))
            {
                if (zRotation == 0)
                {
                    for (int y = 0; y < boardHeight - 1; y++)
                    {
                        for (int x = 0; x < boardWidth - 1; x++)
                        {
                            if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x, y + 1] == null)
                                return true;
                        }
                    }
                }
                else if (zRotation == 90)
                {
                    for (int y = 0; y < boardHeight - 1; y++)
                    {
                        for (int x = 0; x < boardWidth - 1; x++)
                        {
                            if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 1, y + 1] == null)
                                return true;
                        }
                    }
                }
                else if (zRotation == 180)
                {
                    for (int y = 1; y < boardHeight; y++)
                    {
                        for (int x = 0; x < boardWidth - 1; x++)
                        {
                            if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 1, y - 1] == null)
                                return true;
                        }
                    }
                }
                else if (zRotation == 270)
                {
                    for (int y = 1; y < boardHeight; y++)
                    {
                        for (int x = 0; x < boardWidth - 1; x++)
                        {
                            if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x, y - 1] == null)
                                return true;
                        }
                    }
                }

                return false;
            }
            if (shape.CompareTag("ShapeCorner3x3"))
            {
                if (zRotation == 0)
                {
                    for (int y = 0; y < boardHeight - 2; y++)
                    {
                        for (int x = 0; x < boardWidth - 2; x++)
                        {
                            if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null &&
                                _grid[x, y + 1] == null && _grid[x, y + 2] == null)
                                return true;
                        }
                    }
                }
                else if (zRotation == 90)
                {
                    for (int y = 0; y < boardHeight - 2; y++)
                    {
                        for (int x = 0; x < boardWidth - 2; x++)
                        {
                            if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null &&
                                _grid[x + 2, y + 1] == null && _grid[x + 2, y + 2] == null)
                                return true;
                        }
                    }
                }
                else if (zRotation == 180)
                {
                    for (int y = 2; y < boardHeight; y++)
                    {
                        for (int x = 0; x < boardWidth - 2; x++)
                        {
                            if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null &&
                                _grid[x + 2, y - 1] == null && _grid[x + 2, y - 2] == null)
                                return true;
                        }
                    }
                }
                else if (zRotation == 270)
                {
                    for (int y = 2; y < boardHeight; y++)
                    {
                        for (int x = 0; x < boardWidth - 2; x++)
                        {
                            if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null &&
                                _grid[x, y - 1] == null && _grid[x, y - 2] == null)
                                return true;
                        }
                    }
                }
                return false;
            }
            if (shape.CompareTag("ShapeLine2"))
            {
                if (zRotation == 0 || zRotation == 180)
                {
                    for (int y = 0; y < boardHeight - 1; y++)
                    {
                        for (int x = 0; x < boardWidth; x++)
                        {
                            if (_grid[x, y] == null && _grid[x, y + 1] == null)
                                return true;
                        }
                    }
                }
                else if (zRotation == 90 || zRotation == 270)
                {
                    for (int y = 0; y < boardHeight; y++)
                    {
                        for (int x = 0; x < boardWidth - 1; x++)
                        {
                            if (_grid[x, y] == null && _grid[x + 1, y] == null)
                                return true;
                        }
                    }
                }

                return false;
            }
            if (shape.CompareTag("ShapeLine3"))
            {
                if (zRotation == 0 || zRotation == 180)
                {
                    for (int y = 0; y < boardHeight - 2; y++)
                    {
                        for (int x = 0; x < boardWidth; x++)
                        {
                            if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null)
                                return true;
                        }
                    }
                }
                else if (zRotation == 90 || zRotation == 270)
                {
                    for (int y = 0; y < boardHeight; y++)
                    {
                        for (int x = 0; x < boardWidth - 2; x++)
                        {
                            if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null)
                                return true;
                        }
                    }
                }

                return false;
            }
            if (shape.CompareTag("ShapeLine4"))
            {
                if (zRotation == 0 || zRotation == 180)
                {
                    for (int y = 0; y < boardHeight - 3; y++)
                    {
                        for (int x = 0; x < boardWidth; x++)
                        {
                            if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null && _grid[x, y + 3] == null)
                                return true;
                        }
                    }
                }
                else if (zRotation == 90 || zRotation == 270)
                {
                    for (int y = 0; y < boardHeight; y++)
                    {
                        for (int x = 0; x < boardWidth - 3; x++)
                        {
                            if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null && _grid[x + 3, y] == null)
                                return true;
                        }
                    }
                }

                return false;
            }
            if (shape.CompareTag("ShapeLine5"))
            {
                if (zRotation == 0 || zRotation == 180)
                {
                    for (int y = 0; y < boardHeight - 4; y++)
                    {
                        for (int x = 0; x < boardWidth; x++)
                        {
                            if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null &&
                                _grid[x, y + 3] == null && _grid[x, y + 4] == null)
                                return true;
                        }
                    }
                }
                else if (zRotation == 90 || zRotation == 270)
                {
                    for (int y = 0; y < boardHeight; y++)
                    {
                        for (int x = 0; x < boardWidth - 4; x++)
                        {
                            if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null &&
                                _grid[x + 3, y] == null && _grid[x + 4, y] == null)
                                return true;
                        }
                    }
                }

                return false;
            }
            if (shape.CompareTag("ShapeSingleBlock"))
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    for (int x = 0; x < boardWidth; x++)
                    {
                        if (_grid[x, y] == null)
                            return true;
                    }
                }

                return false;
            }
            if (shape.CompareTag("ShapeSquare2x2"))
            {
                for (int y = 0; y < boardHeight - 1; y++)
                {
                    for (int x = 0; x < boardWidth - 1; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x, y + 1] == null &&
                            _grid[x + 1, y + 1] == null)
                            return true;
                    }
                }

                return false;
            }
            if (shape.CompareTag("ShapeSquare3x3"))
            {
                for (int y = 0; y < boardHeight - 2; y++)
                {
                    for (int x = 0; x < boardWidth - 2; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null &&
                            _grid[x, y + 1] == null && _grid[x + 1, y + 1] == null && _grid[x + 2, y + 1] == null &&
                            _grid[x, y + 2] == null && _grid[x + 1, y + 2] == null && _grid[x + 2, y + 2] == null)
                            return true;
                    }
                }

                return false;
            }

            return false;
        }

        public void ChangeCellsColor(Color color)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<SpriteRenderer>().color = color;
            }
        }

        public void StoreShapeInGrid(Shape shape)
        {
            if (!shape)
                return;

            Vector3 currentPos = shape.transform.position;
            currentPos.z = -2;
            shape.transform.position = currentPos;

            // Snap every piece of shape to right position and occupy place in the grid
            foreach (Transform child in shape.transform.GetChild(0).transform)
            {
                float x = (float)Math.Round(Math.Round(child.position.x, 1, MidpointRounding.AwayFromZero), MidpointRounding.AwayFromZero);
                float y = (float)Math.Round(Math.Round(child.position.y, 1, MidpointRounding.AwayFromZero), MidpointRounding.AwayFromZero);
                Vector2 pos = new Vector2(x, y);

                _grid[(int)pos.x, (int)pos.y] = child;
                child.position = new Vector3((int)pos.x, (int)pos.y, -1);
            }
        }

        public int ClearAllRowsAndColumns()
        {
            // Store every piece of every shape in every row and column that needs to be cleared
            ClearAllRows();
            ClearAllColumns();
            // Total number of cleared rows and columns returned for scoring
            int totalNoOfRowsAndColumnsCleared = _numberOfClearedRows + _numberOfClearedColumns;
            switch (totalNoOfRowsAndColumnsCleared)
            {
                case 1:
                    SoundManager.Instance.PlayOneClearRandomSuccessSound();
                    break;
                case 2:
                    SoundManager.Instance.PlayTwoOrThreeClearRandomSuccessSound();
                    break;
                case 3:
                    SoundManager.Instance.PlayTwoOrThreeClearRandomSuccessSound();
                    break;
                case 4:
                    SoundManager.Instance.PlayFourClearRandomSuccessSound();
                    break;
                case 5:
                    SoundManager.Instance.PlayFiveOrSixClearRandomSuccessSound();
                    break;
                case 6:
                    SoundManager.Instance.PlayFiveOrSixClearRandomSuccessSound();
                    break;
            }

            if (_partsInGrid.Count != 0)
            {
                foreach (var part in _partsInGrid)
                {
                    // Store parent of given shape piece
                    Transform blockParent = part.transform.parent;

                    // Slowly make blocks disappear
                    StartCoroutine(DisappearingBlocks(part));

                    // Remove the piece from its parent
                    part.transform.parent = null;
                    // If parent(container) doesn't have any pieces left destroy the whole shape
                    if (blockParent.transform.childCount == 0)
                        Destroy(blockParent.parent.gameObject);
                    // Clear pieces place in grid
                    _grid[(int)Mathf.Round(part.position.x), (int)Mathf.Round(part.transform.position.y)] = null;
                }
                // Reset storage for all shapes pieces
                _partsInGrid = new HashSet<Transform>();
            }

            _numberOfClearedRows = 0;
            _numberOfClearedColumns = 0;

            return totalNoOfRowsAndColumnsCleared;
        }

        private void ClearAllRows()
        {
            for (int y = 0; y < boardHeight; ++y)
            {
                if (IsCompleteRow(y))
                {
                    StorePiecesForRowClearance(y);
                    _numberOfClearedRows++;
                }
            }
        }

        private void ClearAllColumns()
        {
            for (int x = 0; x < boardWidth; ++x)
            {
                if (IsCompleteColumn(x))
                {
                    StorePiecesForColumnClearance(x);
                    _numberOfClearedColumns++;
                }
            }
        }

        private bool IsCompleteRow(int y)
        {
            for (int x = 0; x < boardWidth; ++x)
            {
                if (_grid[x, y] is null)
                    return false;
            }

            return true;
        }

        private bool IsCompleteColumn(int x)
        {
            for (int y = 0; y < boardHeight; ++y)
            {
                if (_grid[x, y] is null)
                    return false;
            }

            return true;
        }

        private void StorePiecesForRowClearance(int y)
        {
            for (int x = 0; x < boardWidth; ++x)
            {
                if (_grid[x, y] != null)
                {
                    _partsInGrid.Add(_grid[x, y]);
                }
            }
        }

        private void StorePiecesForColumnClearance(int x)
        {
            for (int y = 0; y < boardHeight; ++y)
            {
                if (_grid[x, y] != null)
                {
                    _partsInGrid.Add(_grid[x, y]);
                }
            }
        }

        // Slowly make blocks "disappear" by shrinking their scale
        private static IEnumerator DisappearingBlocks(Transform shapePartTransform)
        {
            while (shapePartTransform.localScale.x > 0)
            {
                yield return new WaitForSeconds(0.015f);
                Vector3 localScale = shapePartTransform.localScale;
                localScale = new Vector3(localScale.x - 0.1f, localScale.y - 0.1f);
                shapePartTransform.localScale = localScale;
            }
            // Destroy the piece
            Destroy(shapePartTransform.gameObject);
        }

        // Display empty places on the board in grey color
        private void DrawEmptyCells()
        {
            if (cell == null) Debug.LogWarning("Please assign the _cell object!");
            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    Transform clone = Instantiate(cell, new Vector3(x, y, 0), Quaternion.identity);
                    clone.name = "Board Space (x = " + x + " ,y =" + y + " )";
                    clone.transform.parent = transform;
                    clone.GetComponent<SpriteRenderer>().color = new Color(0.2117647f, 0.2117647f, 0.2117647f, 1);
                }
            }
        }
    }
}