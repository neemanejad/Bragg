using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

namespace Redwood.Enemies;

[GlobalClass]
public partial class EnemyData : Resource
{
	[Export]
	public string Id { get; set; }
	[Export]
	public string Name { get; set; }

	[Export]
	public double Speed = 200;

	[Export]
	public float Damage { get; set; }
}