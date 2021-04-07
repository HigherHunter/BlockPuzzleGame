using UnityEngine;

namespace Utility
{
    [CreateAssetMenu(fileName = "Palette", menuName = "Color Palettes", order = 51)]
    public class ColorPalette : ScriptableObject
    {
        public Color cameraBackgroundColor = Color.magenta;

        public Color boardCellsColor = Color.magenta;

        public Color mainPanelScoreTextColor = Color.magenta;
        public Color mainPanelHighScoreTextColor = Color.magenta;
        public Color mainPanelTrophyColor = Color.magenta;
        public Color mainPanelPauseButtonColor = Color.magenta;

        public Color mainMenuBackgroundColor = Color.magenta;
        public Color mainMenuTrophyColor = Color.magenta;
        public Color mainMenuHighScoreColor = Color.magenta;
        public Color mainMenuStartButtonColor = Color.magenta;
        public Color mainMenuPaletteButtonColor = Color.magenta;
        public Color mainMenuRateButtonColor = Color.magenta;
        public Color mainMenuAdsButtonColor = Color.magenta;
        public Color mainMenuSoundButtonColor = Color.magenta;

        public Color pauseMenuBackgroundColor = Color.magenta;
        public Color pauseMenuHomeButtonColor = Color.magenta;
        public Color pauseMenuRestartButtonColor = Color.magenta;
        public Color pauseMenuPaletteButtonColor = Color.magenta;
        public Color pauseMenuResumeButtonColor = Color.magenta;
        public Color pauseMenuSoundButtonColor = Color.magenta;

        public Color gameOverMenuBackgroundColor = Color.magenta;
        public Color gameOverMenuTrophyColor = Color.magenta;
        public Color gameOverMenuScoreTextColor = Color.magenta;
        public Color gameOverMenuHomeButtonColor = Color.magenta;
        public Color gameOverMenuRestartButtonColor = Color.magenta;
        public Color gameOverMenuRateButtonColor = Color.magenta;
        public Color gameOverMenuPaletteButtonColor = Color.magenta;

        public Color shapeCorner2X2Color = Color.magenta;
        public Color shapeCorner3X3Color = Color.magenta;
        public Color shapeLine2 = Color.magenta;
        public Color shapeLine3 = Color.magenta;
        public Color shapeLine4 = Color.magenta;
        public Color shapeLine5 = Color.magenta;
        public Color shapeSingleBlock = Color.magenta;
        public Color shapeSquare2X2 = Color.magenta;
        public Color shapeSquare3X3 = Color.magenta;
    }
}