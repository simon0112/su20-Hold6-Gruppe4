using DIKUArcade.EventBus;
using DIKUArcade.State;
namespace galaga.GalagaStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }
        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = MainMenu.GetInstance();
        }
        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
                case GameStateType.GameRunning:
                    ActiveState = GameStateType.GameRunning;
                    break;
                case GameStateType.GamePaused:
                    ActiveState = GameStateType.GamePaused;
                    break;
                case GameStateType.MainMenu:
                    ActiveState = GameStateType.MainMenu;
                    break;
            }
        }
        
        
        
    }
}