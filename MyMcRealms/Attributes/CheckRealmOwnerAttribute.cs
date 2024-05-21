namespace MyMcRealms.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckRealmOwnerAttribute : Attribute
    {
        public bool IsRealmOwner(string playerUUID, string ownerUUID)
        {
            return playerUUID == ownerUUID;
        }
    }
}
