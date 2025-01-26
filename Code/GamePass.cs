namespace Sandbox.Services;

/// <summary>
/// A game pass is used to provide perks to players in exchange for s&amp;bux.
/// </summary>
public sealed class GamePass
{
	/// <summary>
	/// The programmatic name you're going to refer to this game pass as.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// A nice name to use when displaying to users.
	/// </summary>
	public string Title { get; set; }

	/// <summary>
	/// Describe this game pass.
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// A square icon, ideally 128x128 pixels, to show with this game pass.
	/// </summary>
	public string Icon { get; set; }

	/// <summary>
	/// The amount of s&amp;bux this game pass costs to purchase.
	/// </summary>
	public int Cost { get; set; }
}
