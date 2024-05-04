using System.Text.Json;
namespace Sandbox.Services;

[GameResource( "Game Pass", "gamepass", "A game pass", Icon = "sbux.svg", IconBgColor = "#232B3D", IconFgColor = "#4B6EE3" )]
public class GamePass : GameResource
{
	/// <summary>
	/// The ident of this game pass. A short name with no special characters.
	/// </summary>
	[Category("Game Pass Setup")]
	public string Ident { get; set; }
	
	/// <summary>
	/// The amount of s&amp;bux this game pass costs.
	/// </summary>
	[Category("Game Pass Setup")]
	public int Cost { get; set; } = 100;
	
	/// <summary>
	/// Name of the game pass to show in UI.
	/// </summary>
	[Category("Display Information")]
	public string Title { get; set; } = "Spawn Box";

	/// <summary>
	/// Icon for this game pass. Only works with urls (for now).
	/// </summary>
	[Category( "Display Information" )]
	public string Icon { get; set; } = "https://i.imgur.com/uPfTvLn.png";
	
	internal string Serialize() => JsonSerializer.Serialize( new { Ident, Cost, Title, Icon } ).Base64Encode();
}
