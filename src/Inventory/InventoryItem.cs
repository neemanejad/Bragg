using Godot;

[GlobalClass]
public partial class InventoryItem : Resource
{
    [Export]
    public string ItemName;

    [Export]
    public string ItemID;
}
