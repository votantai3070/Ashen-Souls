using UnityEngine;

public class EnemyRange_SkillManager : Entity_SkillManager
{
    [SerializeField] private SkillEnemy_DataSO[] skillData;

    public SkillEnemy_Base[] skills { get; private set; }
    public Enemy enemy { get; private set; }

    protected override void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        skills = GetComponentsInChildren<SkillEnemy_Base>();
    }

    private void Start()
    {
        for (int i = 0; i < skillData.Length; i++)
        {
            if (i <= skillData.Length - 1)
                if (skillData[i].upgradeData.enemyType == skills[i].skillEnemyType)
                    skills[i].SetSkillUpgrade(skillData[i]);
        }
    }

    protected override void Update()
    {
        foreach (var skill in skills)
        {
            skill.TryUseSkill();
        }
    }
}
