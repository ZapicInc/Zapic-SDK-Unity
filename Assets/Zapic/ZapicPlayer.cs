using System;

/// <summary>A Zapic player.</summary>
/// <remarks>Added in 1.1.0.</remarks>
public sealed class ZapicPlayer
{
    /// <summary>The URL of the profile icon.</summary>
    private readonly Uri iconUrl;

    /// <summary>The unique identifier.</summary>
    private readonly string id;

    /// <summary>The profile name.</summary>
    private readonly string name;

    /// <summary>The notification token.</summary>
    private readonly string notificationToken;

    /// <summary>Initializes a new instance of the <see cref="ZapicPlayer"/> class.</summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="name">The profile name.</param>
    /// <param name="iconUrl">The URL of the profile icon.</param>
    /// <param name="notificationToken">The notification token.</param>
    public ZapicPlayer(string id, string name, Uri iconUrl, string notificationToken)
    {
        this.iconUrl = iconUrl;
        this.id = id;
        this.name = name;
        this.notificationToken = notificationToken;
    }

    /// <summary>Gets the URL of the profile icon.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public Uri IconUrl
    {
        get
        {
            return iconUrl;
        }
    }

    /// <summary>Gets the unique identifier.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public string Id
    {
        get
        {
            return id;
        }
    }

    /// <summary>Gets the profile name. This is not unique and may be changed by the player at any time.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public string Name
    {
        get
        {
            return name;
        }
    }

    /// <summary>Gets the notification token.</summary>
    /// <remarks>Added in 1.1.0.</remarks>
    public string NotificationToken
    {
        get
        {
            return notificationToken;
        }
    }

    /// <summary>Gets the unique identifier.</summary>
    /// <remarks>Added in 1.1.0.</remarks>
    [Obsolete("Replaced with property ZapicPlayer.Id")]
    public string PlayerId
    {
        get
        {
            return id;
        }
    }
}
