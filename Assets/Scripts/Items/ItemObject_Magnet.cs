using System.Linq;
using UnityEngine;

public class ItemObject_Magnet : ItemObject_Base
{
    private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Player>();
            if (player == null)
                return;

            SkillObject_Soul[] activeSouls = FindObjectsByType<SkillObject_Soul>(FindObjectsSortMode.None)
                        .Where(s => s.gameObject.activeInHierarchy)
                        .ToArray();

            foreach (var soul in activeSouls)
            {
                soul.SetCanMove(player.transform);
            }

            ObjectPool.instance.Despawn(gameObject);
        }
    }
}
