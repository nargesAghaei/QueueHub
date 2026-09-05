using Domain.Interfaces;

namespace Domain.Entities;

public class RefreshToken : IEntity<int>
{
    public int Id { get; private set; }
    public string Token { get; private set; } = "";
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public DateTime ExpireAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsRevokes { get; private set; }

    private RefreshToken(){}

    public RefreshToken(string token, Guid userId,DateTime expireAt)
    {
        Token = token;
        UserId = userId;
        ExpireAt = expireAt;
        CreatedAt = DateTime.UtcNow;
        IsRevokes = false;
    }
    
    public bool IsActive=>!IsRevokes && ExpireAt > DateTime.UtcNow;
    public void Revoke()=>IsRevokes = true;
}