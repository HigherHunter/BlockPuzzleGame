using Core;
using Utility;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Managers
{
    public class GameController : MonoBehaviour
    {
        private Board _gameBoard;
        private Spawner _spawner;
        private UIManager _uiManager;
        private ScoreManager _scoreManager;
        private Shape[] _shapesInPlay;
        private Shape _currentlyHeldShape;
        private int _numberOfShapesInPlay;

        private static bool _isRestarted;
        private bool _isNewHighScoreMade;

        private ColorPalette _currentColorPalette;

        private readonly Color _unfitColor = new Color(0.3921569f, 0.3921569f, 0.3921569f, 1);

        private Camera _mainCamera;

        private Vector2 _touchPos;
        private float _touchDeltaX, _touchDeltaY;

        private readonly Vector2 _overlapBoxSize = new Vector2(2, 2);
        
        // Start is called before the first frame update
        private void Start()
        {
            string savedColorPalette = PlayerPrefs.GetString("ColorPalette");
            _currentColorPalette = savedColorPalette != "" ?
                Resources.Load<ColorPalette>("ColorPalettes/" + savedColorPalette) : Resources.Load<ColorPalette>("ColorPalettes/Day");

            _gameBoard = FindObjectOfType<Board>();
            _spawner = FindObjectOfType<Spawner>();
            _uiManager = FindObjectOfType<UIManager>();
            _scoreManager = FindObjectOfType<ScoreManager>();

            // If the game is restarted we don't need main menu displayed again
            if (_isRestarted)
            {
                _uiManager.CloseMainMenu();
                _isRestarted = false;
                SoundManager.Instance.PlayStartSound();
            }

            _mainCamera = Camera.main;
            if (!_mainCamera)
            {
                Debug.Log("Cant get main camera");
                return;
            }

            _shapesInPlay = new Shape[3];
            _numberOfShapesInPlay = _shapesInPlay.Length;

            _scoreManager.SetHighScore(PlayerPrefs.GetInt("highScore"));
            _uiManager.SetHighScoreUI(_scoreManager.GetHighScore());
            _uiManager.SetMainMenuHighScoreUI(_scoreManager.GetHighScore());

            if (!_gameBoard)
            {
                Debug.LogWarning("There is no game board");
                return;
            }

            if (!_spawner)
            {
                Debug.LogWarning("There is no spawner");
                return;
            }

            // Spawn first 3 shapes
            Shape spawnedShape = _spawner.SpawnShapeAtPositionWithScaleAndRandomRotation(
                new Vector3(1f, -3, -2),
                new Vector3(0.6f, 0.6f, 1.0f));
            _shapesInPlay[0] = spawnedShape;
            spawnedShape = _spawner.SpawnShapeAtPositionWithScaleAndRandomRotation(
                new Vector3(4.5f, -3, -2),
                new Vector3(0.6f, 0.6f, 1.0f));
            _shapesInPlay[1] = spawnedShape;
            spawnedShape = _spawner.SpawnShapeAtPositionWithScaleAndRandomRotation(
                new Vector3(8f, -3, -2),
                new Vector3(0.6f, 0.6f, 1.0f));
            _shapesInPlay[2] = spawnedShape;

            ChangeGameColorPalette(_currentColorPalette);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.touchCount <= 0) return;

            if (IsPointerOverUIObject()) return;

            Touch touch = Input.GetTouch(0);

            _touchPos = _mainCamera.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    foreach (Shape shape in _shapesInPlay)
                    {
                        if (shape)
                        {
                            if (shape.GetComponent<Collider2D>() == Physics2D.OverlapBox(_touchPos, _overlapBoxSize, 0))
                            {
                                _currentlyHeldShape = shape;
                                // Enlarge the shape when pressed
                                var shapeTransform = _currentlyHeldShape.transform;
                                shapeTransform.localScale = _currentlyHeldShape.GetScaleOnMove();

                                _touchDeltaX = _touchPos.x - shapeTransform.position.x;
                                _touchDeltaY = _touchPos.y - shapeTransform.position.y;
                            }
                        }
                    }

                    break;

                case TouchPhase.Moved:
                    if (_currentlyHeldShape)
                        _currentlyHeldShape.transform.position = new Vector3(_touchPos.x - _touchDeltaX,
                            _touchPos.y - _touchDeltaY, _currentlyHeldShape.GetInitialPosition().z - 1);
                    break;

                case TouchPhase.Ended:
                    if (_currentlyHeldShape)
                    {
                        // Try and place this shape on the board
                        if (!(TryToPlaceShape(_currentlyHeldShape)))
                        {
                            // If placement failed move and scale shape to its initial values
                            _currentlyHeldShape.ScaleShapeToInitialScale();
                            _currentlyHeldShape.MoveShapeToInitialPosition();
                        }
                        // In successful or failed case remove the shape from held shape
                        _currentlyHeldShape = null;
                    }

                    break;
            }
        }

        private static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        private bool TryToPlaceShape(Shape shape)
        {
            // Check if the shape can be placed on the board 
            if (!_gameBoard.IsValidPosition(shape))
            {
                SoundManager.Instance.PlayMissSound();
                return false;
            }

            SoundManager.Instance.PlayPlaceSound();

            _gameBoard.StoreShapeInGrid(shape);

            // Score every piece of shape placed on the board and update the score
            _scoreManager.AddScore(shape.NumberOfContainedPieces());
            _uiManager.SetScoreUI(_scoreManager.GetScore());

            // Score every cleared row and/or column and update the score
            _scoreManager.ScoreClearedRowsAndColumns(_gameBoard.ClearAllRowsAndColumns());
            _uiManager.SetScoreUI(_scoreManager.GetScore());

            // Update the high score
            if (!_isNewHighScoreMade)
            {
                if (_scoreManager.IsNewHighScore())
                {
                    SoundManager.Instance.PlayNewHighScoreSound();
                    _isNewHighScoreMade = true;
                }
            }

            // Remove the placed shape from the available shapes
            for (int i = 0; i < _shapesInPlay.Length; i++)
            {
                if (_shapesInPlay[i] == shape)
                {
                    _shapesInPlay[i] = null;
                }
            }

            _numberOfShapesInPlay--;

            // Check if the rest of the shapes can fit and change their color if they cant
            ChangeColorOfUnfitShape();

            if (_numberOfShapesInPlay == 0)
                SpawnNewShapes();

            return true;
        }

        // If shape can't fit anywhere on the board change its color to grey 
        private void ChangeColorOfUnfitShape()
        {
            int numberOfUnfitShapes = 0;
            foreach (Shape shape in _shapesInPlay)
            {
                if (shape)
                {
                    shape.ChangeToInitialColor();
                    if (!_gameBoard.CanShapeFitAnywhere(shape))
                    {
                        shape.ChangeShapeColor(_unfitColor);
                        numberOfUnfitShapes++;
                        if (_numberOfShapesInPlay > 0)
                        {
                            if (_numberOfShapesInPlay == numberOfUnfitShapes)
                            {
                                if (_scoreManager.IsNewHighScore())
                                {
                                    CheckAndUpdateHighScore();
                                    SoundManager.Instance.PlayHighScoreGameOverSound();
                                }
                                else
                                    SoundManager.Instance.PlayGameOverSound();

                                _uiManager.SetGameOverMenuHighScoreUI(_scoreManager.GetScore());
                                _uiManager.OpenGameOverMenu();
                            }
                        }
                    }
                }
            }
        }

        private void SpawnNewShapes()
        {
            SoundManager.Instance.PlayBlocksSpawnSound();

            Shape spawnedShape = _spawner.SpawnShapeAtPositionWithScaleAndRandomRotation(
                new Vector3(1f, -3, -2),
                new Vector3(0.6f, 0.6f, 1.0f));
            _shapesInPlay[0] = spawnedShape;

            spawnedShape = _spawner.SpawnShapeAtPositionWithScaleAndRandomRotation(
                new Vector3(4.5f, -3, -2),
                new Vector3(0.6f, 0.6f, 1.0f));
            _shapesInPlay[1] = spawnedShape;

            spawnedShape = _spawner.SpawnShapeAtPositionWithScaleAndRandomRotation(
                new Vector3(8f, -3, -2),
                new Vector3(0.6f, 0.6f, 1.0f));
            _shapesInPlay[2] = spawnedShape;

            _numberOfShapesInPlay = 3;

            ChangeColorOfUnfitShape();
        }

        public void CheckAndUpdateHighScore()
        {
            if (_scoreManager.IsNewHighScore())
            {
                _uiManager.SetHighScoreUI(_scoreManager.GetHighScore());
                _uiManager.SetMainMenuHighScoreUI(_scoreManager.GetHighScore());
                _scoreManager.SetHighScore(_scoreManager.GetScore());
                PlayerPrefs.SetInt("highScore", _scoreManager.GetHighScore());
            }
        }

        public void RestartGame()
        {
            CheckAndUpdateHighScore();

            _isRestarted = true;
            SceneManager.LoadScene(0);
        }

        public void RefreshGame() => SceneManager.LoadScene(0);

        public void UseColorPalette(ColorPalette palette)
        {
            ChangeGameColorPalette(palette);

            if (_currentColorPalette != palette)
                PlayerPrefs.SetString("ColorPalette", palette.name);
        }

        // Change the color of game
        private void ChangeGameColorPalette(ColorPalette palette)
        {
            _uiManager.ChangeUIColorPalette(palette);

            // Board cells
            _gameBoard.ChangeCellsColor(palette.boardCellsColor);

            // Change color of all shapes in game
            _spawner.ChangeShapesColor(palette);
        }
    }
}