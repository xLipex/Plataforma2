using UnityEngine;

namespace UnityTemplateProjects
{
    [CreateAssetMenu(fileName = "CollectCoinsGameMode", menuName = "GameModes/Collect Coins", order = 0)]
    public class CollectCoinsGameMode : GameMode
    {
        public int CoinsToCollect;

        public override void UpdateGameMode(int coins = 0, float time = 0, bool arrivedAtLocation = false)
        {
            if (coins >= CoinsToCollect)
            {
                gameState = GameState.Victory;
                OnGameModeStateChanged?.Invoke(gameState);
            }
        }
    }
}