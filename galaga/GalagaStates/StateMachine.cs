using DIKUArcade.EventBus;
using DIKUArcade.State;
using galaga.GalagaTypes;
using System;

namespace galaga.GalagaStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }
        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = MainMenu.GetInstance();
        }
        public void SwitchState(GameStateType stateType) {
             switch (stateType) { }
        } 
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
        {

        }
        
    }
}
