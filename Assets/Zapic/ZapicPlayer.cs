using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapicPlayer
{

	/// <summary>
	/// The unique player id
	/// </summary>
	public string PlayerId { get; set; }

	/// <summary>
	/// The push notification token used to identify the player
	/// </summary>
	public string NotificationToken { get; set; }
}