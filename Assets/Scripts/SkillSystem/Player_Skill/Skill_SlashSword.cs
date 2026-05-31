using UnityEngine;

public class Skill_SlashSword : Skill_Base
{
    private Player player;
    [SerializeField] private GameObject slashSwordGo;
    private GameObject slashSword;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        player = entity as Player;
    }

    public override void SetSkill(Skill_DataSO skillData)
    {
        base.SetSkill(skillData);

        slashSwordGo = skillData.skillObjectPrefab;
    }

    public override bool TryUseSkill()
    {
        if (!base.TryUseSkill())
            return false;

        if (slashSword != null)
            return false;

        if (CheckEnemyRadius())
        {
            CreateSlashEffect();
            SetSkillOnCooldown();
        }

        return true;
    }

    public void CreateSlashEffect()
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        Vector3 offset = (Vector3)direction * 0.8f;

        slashSword = ObjectPool.instance.Spawn(slashSwordGo.name, transform.position + offset, rot);
        slashSword.GetComponent<SkillObject_SlashSword>().SetupSlashSword(this, duration, whatIsEnemy);
    }

    public void OnSlashSwordExpired()
    {
        slashSword = null;
    }
}
