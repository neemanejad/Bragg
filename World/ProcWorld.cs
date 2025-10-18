using Godot;

namespace Redwood.World;

public partial class ProcWorld : TileMapLayer
{
	[Export]
	public ChunkGenerator ChunkGenerator { get; set; }

	private Player _player = PlayerManager.Instance.PlayerNode;

	public override void _Ready()
	{
		ChunkGenerator.Connect(ChunkGenerator.SignalName.TileGenerated, new Callable(this, MethodName.SetWorldCell));

		ChunkGenerator.GenerateChunk(Vector2I.Zero);
	}

	public void SetWorldCell(Vector2I coords, int sourceId, Vector2I tileAtlas)
	{
		SetCell(coords, sourceId, tileAtlas);
	}
}