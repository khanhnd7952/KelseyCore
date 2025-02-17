using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kelsey.UGUI
{
    public class TestCollector : MonoBehaviour
    {
        [SerializeField] private CoinCollector collectorPrefab;
        [SerializeField] private Transform target;
        [SerializeField] private float height = 0.5f;

        private void Update()
        {
            //SpawnCoin();
        }

        [Button]
        async void Test(int count)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnCoin();
                await UniTask.Delay(20);
            }
        }

        void SpawnCoin()
        {
            var collector = Instantiate(collectorPrefab, transform);
            // collector.SetUp(target, height * Random.Range(-1f, 1f));
            // collector.Move();
        }
    }
}