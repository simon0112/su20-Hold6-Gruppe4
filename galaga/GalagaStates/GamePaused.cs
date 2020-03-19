using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using DIKUArcade.EventBus;

namespace galaga.GalagaStates {
    public class GamePaused : IGameState {
        private static GamePaused instance = null;
        private Entity backGroundImage;
        private Text[] menuButtons;
        private int maxMenuButtons;
        private int activeMenuButton = 0;
        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }

        public void GameLoop() {

        }

        public void InitializeGameState() {
            menuButtons = new Text[2] {new Text("- Continue", new Vec2F(0.45f,0.5f), new Vec2F(0.1f,0.1f)), new Text("- Main Menu", new Vec2F(0.45f,0.3f), new Vec2F(0.1f,0.1f))};

            backGroundImage = new Entity(
                new DynamicShape(new Vec2F(0.43f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
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
                            GameEventType.GameStateEvent,
                            this,
                            "CHANGE_STATE",
                            "MAIN_MENU", "");
                    }
                }
            }

        }

        public void RenderState() {
            backGroundImage.RenderEntity();

            foreach (Text text in menuButtons) {
                text.RenderText();
            }
        }
    
    }
}