using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using DIKUArcade.EventBus;

namespace galaga.GalagaStates {
    public class MainMenu : IGameState {
        private static MainMenu instance = null;
        private Entity backGroundImage;
        private Text[] menuButtons;
        private int maxMenuButtons;
        private int activeMenuButton = 0;
        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }

        public void GameLoop() {

        }

        public void InitializeGameState() {
            menuButtons = new Text[2] {new Text("- New Game", new Vec2F(0.45f,0.5f), new Vec2F(0.1f,0.1f)), new Text("- Quit", new Vec2F(0.45f,0.3f), new Vec2F(0.1f,0.1f))};

            backGroundImage = new Entity(
                new DynamicShape(new Vec2F(0.43f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "TitleImage.png")));
        }

        public void UpdateGameLogic() {

        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") {
                if (keyValue == "KEY_UP") {
                    activeMenuButton = 0;
                } else if (keyValue == "KEY_DOWN") {
                    activeMenuButton = 1;
                } else if (keyValue == "KEY_ENTER") {
                    if (activeMenuButton == 0) {
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent,
                            this,
                            "CHANGE_STATE",
                            "GAME_RUNNING", "");
                    } else if (activeMenuButton == 1) {
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", "");
                    }
                }
            }

        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            if (activeMenuButton == 0) {
                menuButtons[0].SetColor(new Vec3F(1f,0f,0f));
                menuButtons[1].SetColor(new Vec3F(0.5f,0.5f,0f));
            } else if (activeMenuButton == 1) {
                menuButtons[1].SetColor(new Vec3F(1,0,0));
                menuButtons[0].SetColor(new Vec3F(0.5f,0.5f,0f));
            }
            foreach (Text text in menuButtons) {
                text.RenderText();
            }
        }
    
    }
}