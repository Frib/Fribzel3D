using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Fribzel3D.Screens;
using Fribzel3D.ResourceFiles;

namespace Fribzel3D.Management
{
    /// <summary>
    /// ScreenManager. Add your screen creation stuff here
    /// </summary>
    public static class ScreenManager
    {
        public static Screen MainMenu()
        {
            MenuScreen ms = new MenuScreen(ControlMode.KeyboardAndMouse);
            ms.AddMenuEntry("Play", new Action(() => { Fribzel.BaseGame.ShowScreen(ScreenManager.GameScreen()); }));
            ms.AddMenuEntry("Options", new Action(() => { Fribzel.BaseGame.ShowScreen(ScreenManager.ControlScreen(ms)); }));
            ms.AddMenuEntry("Quit", new Action(() => { Fribzel.BaseGame.Exit(); }));

            ms.AddHeader(string.Format(CultureInfo.CurrentCulture, Text.MenuNavigation, RM.GetFirstMappedButton(InputAction.Up), RM.GetFirstMappedButton(InputAction.Down)));
            ms.AddHeader(string.Format(CultureInfo.CurrentCulture, Text.MenuSelection, RM.GetFirstMappedButton(InputAction.Accept), RM.GetFirstMappedButton(InputAction.Back)));

            ms.AddFooter(Text.MadeBy);
            return ms;
        }

        public static Screen ControlScreen(Screen screenToGoBackTo)
        {
            return new ControlScreen(screenToGoBackTo);
        }

        public static Screen GameScreen()
        {
            return new MenuScreen(ControlMode.Keyboard);
        }
    }
}
