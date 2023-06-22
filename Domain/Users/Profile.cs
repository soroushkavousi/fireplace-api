namespace FireplaceApi.Domain.Users;

public class Profile
{
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string About { get; set; }
    public string AvatarUrl { get; set; }
    public string BannerUrl { get; set; }
    public DateTime CreationDate { get; set; }

    public Profile(string username, string displayName, string about,
        string avatarUrl, string bannerUrl, DateTime creationDate)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
        DisplayName = displayName;
        About = about;
        AvatarUrl = avatarUrl;
        BannerUrl = bannerUrl;
        CreationDate = creationDate;
    }

    public Profile(User user) : this(user.Username, user.DisplayName, user.About,
        user.AvatarUrl, user.BannerUrl, user.CreationDate)
    {

    }
}
