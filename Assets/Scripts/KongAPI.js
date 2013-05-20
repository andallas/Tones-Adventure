#pragma strict

var isKongregate = false;
var userId = 0;
static var username = "Guest";
var gameAuthToken = "";

function OnKongregateAPILoaded(userInfoString : String)
{
	isKongregate = true;
	Debug.Log("On Kongregate");

	var params = userInfoString.Split("|"[0]);
	userId = parseInt(params[0]);
	username = params[1];
	gameAuthToken = params[2];
	GameController.Log("Connected: " + isKongregate + " UserID: " + userId + " UserName: " + username + " token: " + gameAuthToken);
}

function Awake()
{
	DontDestroyOnLoad(this);
	Application.ExternalEval(
		"if(typeof(kongregateUnitySupport) != 'undefined'){" +
		"kongregateUnitySupport.initAPI('KongAPI', 'OnKongregateAPILoaded');" +
		"}"
		);
}