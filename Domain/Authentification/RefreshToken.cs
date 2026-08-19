namespace TestCI.Domain.Authentification
{
    public class RefreshToken
    {
        public int Id { get; private set; }

        public string Token { get; private set; }

        public int UserId { get; private set; }

        public DateTime ExpiresAt { get; private set; }

        public bool IsRevoked { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public RefreshToken(
            string token,
            int userId,
            DateTime expiresAt)
        {
            Token = token;
            UserId = userId;
            ExpiresAt = expiresAt;
            IsRevoked = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void Revoke()
        {
            IsRevoked = true;
        }
        public bool IsValid()
        {
            return !IsRevoked &&
                   ExpiresAt > DateTime.UtcNow;
        }
    }
}
