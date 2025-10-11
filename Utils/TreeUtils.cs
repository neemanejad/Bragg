using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

using Redwood.UI;

namespace Redwood.Utils;
public static partial class TreeUtils
{
	public static UIManager GetRootUINode(Node node)
	{
		return node.GetTree().Root.GetNode<UIManager>("Main/UIManager");
	}
}