namespace Assets.Scripts.GameplayModule.Scriptable
{
    public interface ILevel
    {
        int Turns { get; set; }
        ChallengeType[] Challenges { get; }
        int PriceCoins { get; }
        bool IsEnd { get; }
        bool IsVictory { get; }
    }
}