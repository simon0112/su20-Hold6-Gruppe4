using NUnit.Framework;
using galaga.GalagaTypes;

namespace galagaTests {
    public class TestStateToString {

        [Test]
        public void TestRunStringToState() {
            var test = galaga.GalagaTypes.StateTransformer.TransformStringToState("GAME_RUNNING");
            Assert.AreEqual(galaga.GalagaTypes.GameStateType.GameRunning, test);
        }

        [Test]
        public void TestPausStringToState() {
            var test = galaga.GalagaTypes.StateTransformer.TransformStringToState("GAME_PAUSED");
            Assert.AreEqual(galaga.GalagaTypes.GameStateType.GamePaused, test);
        }

        [Test]
        public void TestMainStringToState() {
            var test = galaga.GalagaTypes.StateTransformer.TransformStringToState("MAIN_MENU");
            Assert.AreEqual(galaga.GalagaTypes.GameStateType.MainMenu, test);
        }


        [Test]
        public void TestRunStateToString() {
            var test = galaga.GalagaTypes.StateTransformer.TransformStateToString(galaga.GalagaTypes.GameStateType.GameRunning);
            Assert.AreEqual("GAME_RUNNING", test);
        }

        [Test]
        public void TestPausStateToString() {
            var test = galaga.GalagaTypes.StateTransformer.TransformStateToString(galaga.GalagaTypes.GameStateType.GamePaused);
            Assert.AreEqual("GAME_PAUSED", test);
        }

        [Test]
        public void TestMainStateToString() {
            var test = galaga.GalagaTypes.StateTransformer.TransformStateToString(galaga.GalagaTypes.GameStateType.MainMenu);
            Assert.AreEqual("MAIN_MENU", test);
        }
    }
}