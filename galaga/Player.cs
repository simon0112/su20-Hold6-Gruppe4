using DIKUArcade;

public class Player : IGameEventProcessor<object> {
    public Entity Entity {get; private set;}
    public Player(DynamicShape shape, IBaseImage image) {
        Entity = new Entity(shape, image);
}
}