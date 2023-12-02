namespace WebApi.Extensions
{
    public static class SessionJwtExtension
    {
        private const string ActiveJwtKey = "ActiveJwt";

        public static string? GetActiveJwt(this ISession session)
        {
            return session.GetString(ActiveJwtKey);
        }

        public static void SetActiveJwt(this ISession session, string jwtToken)
        {
            session.SetString(ActiveJwtKey, jwtToken);
        }

        public static void RemoveActiveJwt(this ISession session)
        {
            session.Remove(ActiveJwtKey);
        }
    }
}
