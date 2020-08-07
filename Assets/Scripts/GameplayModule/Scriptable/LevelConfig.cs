using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameplayModule.Scriptable
{
    [CreateAssetMenu(fileName = "LevelChallenge", menuName = "Game/Level Challenge")]
    public class LevelConfig : ScriptableObject, ILevel
    {
        public int turns = 10;
        public ChallengeType[] challenges;
        public int priceCoins = 0;

        public int Turns { get => turns; set => turns = value; }
        public ChallengeType[] Challenges { get => challenges; }

        public int PriceCoins { get => priceCoins; }
        public bool IsEnd => Turns <= 0;
        public bool IsVictory => Turns >= 0 && Challenges.All(c => c.amount <= 0);

        public void AddSolved(params (EContentType type, int amount)[] matches)
        {
            matches.ToList().ForEach(AddSolved);
        }

        private void AddSolved((EContentType type, int amount) match)
        {
            ChallengeType searchResult = challenges.FirstOrDefault(challenge => challenge.type == match.type);
            if (searchResult != null)
            {
                searchResult.amount = Mathf.Max(searchResult.amount - match.amount, 0);
            }
        }

        internal void AddSolved((object, int) p)
        {
            throw new NotImplementedException();
        }
    }

    [CreateAssetMenu(fileName = "Challenge", menuName = "Game/Challenge")]
    public class ChallengeType : ScriptableObject
    {
        public EContentType type = EContentType.APPLE;
        public int amount = 1;
    }
}
