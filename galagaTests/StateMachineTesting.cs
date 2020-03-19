using galaga;
using galaga.GalagaStates;
using galaga.GalagaTypes;
using NUnit.Framework;
using DIKUArcade.EventBus;
using System.Collections.Generic;


namespace galagaTests {
    [TestFixture]
    public class StateMachineTesting {
        private StateMachine stateMachine;
        private GameEventBus<object> eventBus;

        [SetUp]
        public void InitiateStateMachine() {
            DIKUArcade.Window.CreateOpenGLContext();

            eventBus = galaga.GalagaBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent,
                GameEventType.WindowEvent,
                GameEventType.GameStateEvent,
                });

            stateMachine = new StateMachine();
            // (1) Initialize a GalagaBus with proper GameEventTypess
            // (2) Instantiate the StateMachine
            // (3) Subscribe the GalagaBus to proper GameEventTypes
            // and GameEventProcessors
        }
        
        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }

        [Test]
        public void TestEventGamePaused() {
            galaga.GalagaBus.GetBus().RegisterEvent(
            GameEventFactory<object>.CreateGameEventForAllProcessors(
            GameEventType.GameStateEvent,
            this,
            "CHANGE_STATE",
            "GAME_PAUSED", ""));
            galaga.GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }
    }
}