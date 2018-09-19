using System;

/// <summary>
/// Information about a Zapic player.
/// </summary>
public sealed class ZapicPlayer
{
    /// <summary>
    /// The unique player id
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    /// The display name. This value is not
    /// unique and may change at any time.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The push notification token used to identify the player
    /// </summary>
    public string NotificationToken { get; private set; }

    /// <summary>
    /// The url to the players profile photo
    /// </summary>
    /// <value></value>
    public Uri IconUrl { get; private set; }

    /// <summary>
    /// (Obsolete) The unique player id
    /// </summary>
    [Obsolete("Please use Id instead")]
    public string PlayerId { get { return Id; } }

    public ZapicPlayer(string id, string name, Uri iconUrl, string notificationToken)
    {
        Id = id;
        Name = name;
        IconUrl = iconUrl;
        NotificationToken = notificationToken;
    }
}