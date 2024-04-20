namespace MyMcRealms.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireMinecraftCookieAttribute : Attribute
    {
        public RequireMinecraftCookieAttribute()
        {
        }

        public bool HasMinecraftCookie(string cookie)
        {
            return cookie.Contains("sid") && cookie.Contains("user") && cookie.Contains("version");
        }
    }
}
