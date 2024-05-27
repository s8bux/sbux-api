using Sandbox.Diagnostics;
using Sandbox.UI;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
namespace Sandbox.Services;

public static class Monetization
{
	[ConVar( "sbux", Help = "Your s&bux balance.", Saved = true )] 
	private static string _balance { get; set; } = "0";

	private static List<string> _gamePass { get; set; } = new();
	private static Dictionary<string, DateTime> _quests { get; set; } = new();
	private static List<Notification> _notifications { get; set; } = new();
	
	private const string URL = "https://sbux.party/";
	//private const string URL = "https://localhost:44306/";
	
	private static async Task<string> Identification() => $"?steamid={Game.SteamId}&token={await Auth.GetToken( "sbux" )}&ident={Game.Ident}&balance={_balance}";
	
	public static async Task Refresh()
	{
		//Log.Info( "hi" );
		var response = await Http.RequestJsonAsync<JsonNode>( URL + await Identification() );

		//Log.Info( "finished" );
		_balance = response["balance"].Deserialize<string>();
		_gamePass = response["gamepass"].Deserialize<List<string>>();
		_notifications.Add( new Notification( "Daily Reward", "+25", TimeSpan.Zero ) );
		//_quests = response["quests"].Deserialize<Dictionary<string, DateTime>>();
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

		return Has( gamePass.Ident );
	}
	
	/// <summary>
	/// If the player owns the game pass.
	/// </summary>
	public static bool Has( string ident ) => _gamePass.Contains( ident );

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

	public static async Task<bool> Quest( string quest )
	{
		if ( _quests.TryGetValue( quest, out DateTime nextUseDate ) && nextUseDate > DateTime.UtcNow )
		{
			Log.Info( $"Try again in {nextUseDate - DateTime.UtcNow}");
			return false;
		}
		
		var response = await Http.RequestAsync( URL + $"quest/{quest}" + await Identification(), "post" );
		
		if ( response.IsSuccessStatusCode )
		{
			await Refresh();
			Log.Info( _balance );
			return true;
		}

		return false;
	}
}
