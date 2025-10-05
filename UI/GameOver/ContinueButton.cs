using Godot;

using Redwood.Singletons.SceneSwitcher;

namespace Redwood.UI.GameOver;

public partial class ContinueButton : Button
{
	public void Continue()
	{
		SceneSwitcher.Instance.GotoScene("res://Hub/Hub.tscn");
	}
}