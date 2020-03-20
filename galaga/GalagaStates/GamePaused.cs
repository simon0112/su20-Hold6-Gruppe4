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
        private int activeMenuButton = 0;
        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }

        public void GameLoop() {

        }

        public void InitializeGameState() {
            menuButtons = new Text[2] {new Text("- Continue", new Vec2F(0.35f,0.5f), new Vec2F(0.5f,0.4f)), new Text("- Main Menu", new Vec2F(0.35f,0.0f), new Vec2F(0.5f,0.4f))};

            backGroundImage = new Entity(
                new DynamicShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f)),
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
                        galaga.GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_RUNNING", ""));
                    } else if (activeMenuButton == 1) {
                        galaga.GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "MAIN_MENU", ""));
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