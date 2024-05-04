using Sandbox.Diagnostics;
using Sandbox.UI;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
namespace Sandbox.Services;

public static class Monetization
{
	[ConVar("sbux", Help = "Your s&bux balance.", Saved = true )]
	private static string _balance { get; set; }

	private static List<string> _gamePass { get; set; }

	private const string URL = "https://sbux.party/";
	
	private static async Task<string> Identification() => $"?steamid={Game.SteamId}&token={await Auth.GetToken( "sbux" )}&ident={Game.Ident}&balance={_balance}";

	private static async Task Refresh()
	{
		var response = await Http.RequestJsonAsync<JsonNode>( URL + await Identification() );

		_balance = response["balance"].Deserialize<string>();
		_gamePass = response["gamepass"].Deserialize<List<string>>();
	}
	
	static Monetization()
	{
		_ = Refresh();
	}

	/// <summary>
	/// If the player owns the game pass.
	/// </summary>
	public static bool Has( this GamePass gamePass )
	{
		Assert.NotNull( gamePass );
		
		return _gamePass.Contains( gamePass.Ident );
	}

	/// <summary>Prompts the player to purchase the game pass.</summary>
	/// <returns>True if the game pass was bought.</returns>
	/// <remarks>A game pass can be purchased multiple times.</remarks>
	public static async Task<bool> Purchase( this GamePass gamePass )
	{
		Assert.NotNull( gamePass );
		
		var prompt = new Prompt( URL + gamePass.Serialize() + await Identification() );

		if ( await prompt.Purchased.Task )
		{
			await Refresh();
			return true;
		}

		return false;
	}

	/// <summary>
	/// If the player owns the game pass, if not - prompt a purchase.
	/// </summary>
	public static async Task<bool> HasOrPurchase( this GamePass gamePass )
	{
		return gamePass.Has() || await gamePass.Purchase();
	}
}
