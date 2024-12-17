using NanoEcs;

public class HelloWorldSystem : SystemEcs, IExecutable
{
    Contexts contexts;

    GameCollector messages;

    public HelloWorldSystem(Contexts contexts)
    {
        this.contexts = contexts;

        //messages = contexts.Game.GetGroup()
        //    .With.Message
        //    .With.Position
        //    .OnAdd;
    }

    public void Execute()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    var e = contexts.Game.CreateEntity()
        //        .AddMessage("Hello World!")
        //        .AddPosition(3,5)
        //        .AddReference(contexts.Game.CreateEntity());
        //}

        //foreach (var e in messages)
        //{
        //    Debug.Log($"Hi! I'm entity {e.ID} placed in ({e.Position.X}:{e.Position.Y}). The message is: \"{e.Message.Value}\".");
        //}
        //messages.Clear();
    }


}
