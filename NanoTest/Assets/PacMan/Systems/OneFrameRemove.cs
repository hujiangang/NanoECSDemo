using NanoEcs;

namespace PacMan.Gameplay
{
    public class OneFrameRemove<T> : SystemEcs, IExecutable where T : class
    {
        private readonly PacManContexts contexts;
        readonly GameCollector entitys;


        public OneFrameRemove(PacManContexts contexts)
        {
            this.contexts = contexts;
            entitys = contexts.Game.GetGroup()
            .With.WithGroup<T>().OnAdd;
        }

        public void Execute()
        {
            foreach (var e in entitys)
            {
                e.Remove<T>();
            }
        }
    }
}
