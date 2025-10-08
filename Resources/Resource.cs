using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redwood.Resources;

public enum ResourceId
{
	IRON,
	COPPER,
	SULFUR,
	CRYSTALS,
	TITANIUM,
	SAND_GLASS
}

public class Resource
{
	public string Name { get; private set; }
	public string Description { get; private set; }
	public int Quanity { get; private set; }
	public ResourceId Id { get; private set; }
}

public class ResourceBundle
{
	public Resource Resource { get; private set; }
	public int Quantity { get; private set; }
}