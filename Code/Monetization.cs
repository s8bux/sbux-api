using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Sandbox.Services;

internal sealed class Monetization( Scene scene ) : GameObjectSystem( scene ), ISceneLoadingEvents
{
	/// <summary>
	/// Balance of the local player.
	/// </summary>
	[ConVar( "sbux", ConVarFlags.UserInfo | ConVarFlags.Saved | ConVarFlags.Protected )]
	public static int Balance { get; set; } = 0;
	
	/// <summary>
	/// Ensure all values are up-to-date.
	/// </summary>
	public async Task OnLoad( Scene scene, SceneLoadOptions options )
	{
		var data = await Http.RequestJsonAsync<JsonObject>( "https://sbux.party/" );
		
		Balance = data.GetPropertyValue( nameof(Balance), 0 );
	}

	public void AfterLoad( Scene scene ) { /* Garry forgot to add a body */ }
}
