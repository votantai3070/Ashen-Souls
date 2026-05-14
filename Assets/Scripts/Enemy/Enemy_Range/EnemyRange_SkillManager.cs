using UnityEngine;

public class EnemyRange_SkillManager : Entity_SkillManager
{
    [SerializeField] private SkillEnemy_DataSO[] skillData;

    public SkillEnemy_Base[] skills { get; private set; }
    public Enemy enemy { get; private set; }

    public SkillEnemy_Base energyBall { get; private set; }

    protected override void Awake()
    {
        enemy = GetComponentInParent<Enemy>();

        skills = GetComponentsInChildren<SkillEnemy_Base>();
        energyBall = GetComponentInChildren<SkillEnemy_EnergyBall>();
    }

    private void Start()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (i <= skillData.Length - 1)
                if (skillData[i].upgradeData.enemyType == skills[i].skillEnemyType)
                    skills[i].SetSkillUpgrade(skillData[i]);
        }
    }

    protected override void Update()
    {

    }

    public SkillEnemy_Base GetSkillEnemyByType(SkillEnemyType type)
    {
        switch (type)
        {
            case SkillEnemyType.EnergyBall: return energyBall;

            default:
                Debug.Log($"Skill enemy type {type} is not implemented yet.");
                return null;
        }
    }
}
