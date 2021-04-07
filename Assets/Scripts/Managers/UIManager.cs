using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Text highScoreText;
        [SerializeField] private Text mainMenuHighScoreText;
        [SerializeField] private Text gameOverMenuScoreText;

        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private GameObject paletteMenuPanel;
        [SerializeField] private GameObject gameOverMenuPanel;

        public void SetHighScoreUI(int score) => highScoreText.text = score.ToString();

        public void SetMainMenuHighScoreUI(int score) => mainMenuHighScoreText.text = score.ToString();

        public void SetGameOverMenuHighScoreUI(int score) => gameOverMenuScoreText.text = score.ToString();

        public void SetScoreUI(int score) => scoreText.text = score.ToString();

        public void OpenMainMenu() => mainMenuPanel.SetActive(true);

        public void CloseMainMenu() => mainMenuPanel.SetActive(false);

        public void OpenPauseMenu()
        {
            pauseMenuPanel.SetActive(true);
            StartCoroutine(PauseMenuPopUp());
        }

        public void ClosePauseMenu() => StartCoroutine(PauseMenuDropDown());

        public void OpenPaletteMenu() => paletteMenuPanel.SetActive(true);

        public void ClosePaletteMenu() => paletteMenuPanel.SetActive(false);

        public void OpenGameOverMenu() => gameOverMenuPanel.SetActive(true);

        public void CloseGameOverMenu() => gameOverMenuPanel.SetActive(false);

        // PopUp effect of pause menu
        private IEnumerator PauseMenuPopUp()
        {
            RectTransform pauseMenuRectTransform = pauseMenuPanel.transform.GetChild(0).GetComponent<RectTransform>();
            Vector2 target1 = new Vector2(0f, 100f);
            Vector2 target2 = new Vector2(0f, -100f);

            while (pauseMenuRectTransform.anchoredPosition != target1)
            {
                pauseMenuRectTransform.anchoredPosition = Vector2.MoveTowards(
                    pauseMenuRectTransform.anchoredPosition,
                    target1,
                    3000.0f * Time.deltaTime);
                yield return 0;
            }

            while (pauseMenuRectTransform.anchoredPosition != target2)
            {
                pauseMenuRectTransform.anchoredPosition = Vector2.MoveTowards(
                    pauseMenuRectTransform.anchoredPosition,
                    target2,
                    2000.0f * Time.deltaTime);
                yield return 0;
            }
        }

        // Drop down effect of pause menu
        private IEnumerator PauseMenuDropDown()
        {
            RectTransform pauseMenuRectTransform = pauseMenuPanel.transform.GetChild(0).GetComponent<RectTransform>();
            Vector2 target1 = new Vector2(0f, 100f);
            Vector2 target2 = new Vector2(0f, -750f);

            while (pauseMenuRectTransform.anchoredPosition != target1)
            {
                pauseMenuRectTransform.anchoredPosition = Vector2.MoveTowards(
                    pauseMenuRectTransform.anchoredPosition,
                    target1,
                    2000.0f * Time.deltaTime);
                yield return 0;
            }

            while (pauseMenuRectTransform.anchoredPosition != target2)
            {
                pauseMenuRectTransform.anchoredPosition = Vector2.MoveTowards(
                    pauseMenuRectTransform.anchoredPosition,
                    target2,
                    3000.0f * Time.deltaTime);
                yield return 0;
            }

            pauseMenuPanel.SetActive(false);
        }

        // Change the color of ui
        public void ChangeUIColorPalette(ColorPalette palette)
        {
            // Camera background
            if (Camera.main != null) Camera.main.backgroundColor = palette.cameraBackgroundColor;

            #region MainPanel
            // Top panel of main panel
            Transform mainPanelTopPanel = mainPanel.transform.GetChild(0);
            // Score text of main panel
            mainPanelTopPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = palette.mainPanelScoreTextColor;
            // High score text of main panel
            mainPanelTopPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = palette.mainPanelHighScoreTextColor;
            // Trophy icon of main panel
            mainPanelTopPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = palette.mainPanelTrophyColor;
            // Pause button of main panel
            mainPanelTopPanel.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = palette.mainPanelPauseButtonColor;
            #endregion

            #region MainMenu
            mainMenuPanel.GetComponent<Image>().color = palette.mainMenuBackgroundColor;
            //Panel center of main menu panel
            Transform mainMenuPanelCenterPanel = mainMenuPanel.transform.GetChild(0);
            // Trophy icon of main menu
            mainMenuPanelCenterPanel.GetChild(0).GetChild(0).GetComponent<Image>().color = palette.mainMenuTrophyColor;
            // High score text of main menu
            mainMenuPanelCenterPanel.GetChild(1).GetChild(0).GetComponent<Text>().color = palette.mainMenuHighScoreColor;
            // Start button of main menu
            mainMenuPanelCenterPanel.GetChild(2).GetComponent<Image>().color = palette.mainMenuStartButtonColor;
            // Palette button of main menu
            mainMenuPanelCenterPanel.GetChild(3).GetComponent<Image>().color = palette.mainMenuPaletteButtonColor;
            // Rate button of main menu
            mainMenuPanelCenterPanel.GetChild(4).GetComponent<Image>().color = palette.mainMenuRateButtonColor;
            // Ads button of main menu
            mainMenuPanel.transform.GetChild(1).GetComponent<Image>().color = palette.mainMenuAdsButtonColor;
            // Sound button of main menu
            mainMenuPanel.transform.GetChild(2).GetComponent<Image>().color = palette.mainMenuSoundButtonColor;
            mainMenuPanel.transform.GetChild(3).GetComponent<Image>().color = palette.mainMenuSoundButtonColor;
            #endregion

            #region Pause Menu
            // Pause menu background color
            pauseMenuPanel.GetComponent<Image>().color = palette.pauseMenuBackgroundColor;
            // Panel center of pause menu
            Transform pauseMenuPanelCenter = pauseMenuPanel.transform.GetChild(0);
            // Home button of pause menu
            pauseMenuPanelCenter.GetChild(0).GetComponent<Image>().color = palette.pauseMenuHomeButtonColor;
            // Restart button of pause menu
            pauseMenuPanelCenter.GetChild(1).GetComponent<Image>().color = palette.pauseMenuRestartButtonColor;
            // Palette button of pause menu
            pauseMenuPanelCenter.GetChild(2).GetComponent<Image>().color = palette.pauseMenuPaletteButtonColor;
            // Resume button of pause menu
            pauseMenuPanelCenter.GetChild(3).GetComponent<Image>().color = palette.pauseMenuResumeButtonColor;
            // Sound button of pause menu
            pauseMenuPanel.transform.GetChild(1).GetComponent<Image>().color = palette.pauseMenuSoundButtonColor;
            pauseMenuPanel.transform.GetChild(2).GetComponent<Image>().color = palette.pauseMenuSoundButtonColor;
            #endregion

            #region Game Over Menu
            // Background of game over menu
            gameOverMenuPanel.GetComponent<Image>().color = palette.gameOverMenuBackgroundColor;
            // Panel center top of game over menu
            Transform gameOverMenuPanelCenterTop = gameOverMenuPanel.transform.GetChild(0).GetChild(0);
            // Panel center bottom of game over menu
            Transform gameOverMenuPanelCenterBottom = gameOverMenuPanel.transform.GetChild(0).GetChild(1);
            // Trophy icon of game over menu
            gameOverMenuPanelCenterTop.GetChild(0).GetChild(0).GetComponent<Image>().color = palette.gameOverMenuTrophyColor;
            // Score text of game over menu
            gameOverMenuPanelCenterTop.GetChild(1).GetChild(0).GetComponent<Text>().color = palette.gameOverMenuScoreTextColor;
            // Home button of game over menu
            gameOverMenuPanelCenterBottom.GetChild(0).GetComponent<Image>().color = palette.gameOverMenuHomeButtonColor;
            // Restart button of game over menu
            gameOverMenuPanelCenterBottom.GetChild(1).GetComponent<Image>().color = palette.gameOverMenuRestartButtonColor;
            // Rate button of game over menu
            gameOverMenuPanelCenterBottom.GetChild(2).GetComponent<Image>().color = palette.gameOverMenuRateButtonColor;
            // Palette button of game over menu
            gameOverMenuPanelCenterBottom.GetChild(3).GetComponent<Image>().color = palette.gameOverMenuPaletteButtonColor;
            #endregion
        }
    }
}