using System;
using MonoDevelop.Components.Commands;

namespace BuildLightVSM
{
    public class DummyHandler : CommandHandler
    {
        static BuildEventsHandler eventsHandler =
            new BuildEventsHandler();

        protected override void Run()
        {
            base.Run();
        }
    }
}
