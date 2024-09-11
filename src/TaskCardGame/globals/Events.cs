using Godot;

namespace TaskCardGame.globals;

/// <summary>
///     Singleton Events that can be emitted/consumed from anywhere.
/// </summary>
public partial class Events : Node
{
    [Signal]
    public delegate void LifecycleStartGameRequestedEventHandler();


    // Define singleton logic
    public static Events Instance { get; private set; }

    public override void _EnterTree()
    {
        if (Instance != null)
        {
            QueueFree(); // The Singleton is already loaded, kill this instance
        }

        Instance = this;
    }
}
