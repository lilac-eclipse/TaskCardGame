using Godot;

namespace TaskCardGame.globals;

public partial class SceneManager : Node
{
    private Node _game;
    private PackedScene _gameScene = GD.Load<PackedScene>("res://scenes/game/game.tscn");

    // Define singleton logic
    public static SceneManager Instance { get; private set; }

    public override void _Ready()
    {
        Events.Instance.LifecycleStartGameRequested += InstanceOnLifecycleStartGameRequested;
        Logger.Instance.Debug("here");
        InstanceOnLifecycleStartGameRequested();
    }

    private void InstanceOnLifecycleStartGameRequested()
    {
        Logger.Instance.Debug("InstanceOnLifecycleStartGameRequested");
        _game = _gameScene.Instantiate();
        AddChild(_game);
    }

    public override void _EnterTree()
    {
        if (Instance != null)
        {
            QueueFree(); // The Singleton is already loaded, kill this instance
        }

        Instance = this;
    }
}
