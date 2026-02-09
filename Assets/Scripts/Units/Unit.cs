using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;
using System;
using Unity.VisualScripting;

public enum Team { Adventurer, Enemy }

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected Team unitTeam; public Team UnitTeam => unitTeam;

    [Header("Base Stats")]
    [SerializeField] protected string unitID; public string UnitID => unitID;
    [SerializeField] protected string unitName; public string UnitName => unitName;

    [SerializeField] protected int STR; // Strength     - Physical Damage
    [SerializeField] protected int INT; // Intelligence - Magic Damage
    [SerializeField] protected int DEX; // Dexterity    - Chance of physical damage used in attack
    [SerializeField] protected int WIS; // Wisdom       - Chance of magic damage used in attack
    [SerializeField] protected int VIT; // Vitality     - Base HP
    [SerializeField] protected int END; // Endurance    - Physical damage reduction
    [SerializeField] protected int SPI; // Spirit       - Magic damage reduction
    [SerializeField] protected int AGI; // Agility      - turn order
    public int EffectiveSTR => GetStat(StatType.STR, STR);
    public int EffectiveINT => GetStat(StatType.INT, INT);
    public int EffectiveDEX => GetStat(StatType.DEX, DEX);
    public int EffectiveWIS => GetStat(StatType.WIS, WIS);
    public int EffectiveVIT => GetStat(StatType.VIT, VIT);
    public int EffectiveEND => GetStat(StatType.END, END);
    public int EffectiveSPI => GetStat(StatType.SPI, SPI);
    public int EffectiveAGI => GetStat(StatType.AGI, AGI);

    [SerializeField] protected int GRO; // Growth       - Rate of improvement or reduction of stats      # Maybe make it change every year and reduce as age goes by
    [SerializeField] protected int age;

    [SerializeField] protected AllSpritesLibrary library; public AllSpritesLibrary Library => library;
    [SerializeField] protected int spriteID; public int SpriteID => spriteID;
    [SerializeField] protected int partyNum; public int PartyNum => partyNum;
    [SerializeField] protected int baseStatTotal; public int BaseStatTotal => baseStatTotal;

    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float physicalDamage;
    [SerializeField] protected float magicDamage;

    [SerializeField] protected float outgoingDamage; public float OutgoingDamage => outgoingDamage;
    [SerializeField] protected float aggroWeight; public float AggroWeight => aggroWeight;
    [SerializeField] protected bool isHired = false; public bool IsHired => isHired;
    [SerializeField] protected int hiringPrice; public int HiringPrice => hiringPrice;
    [SerializeField] protected bool isDead = false; public bool IsDead => isDead;
    [SerializeField] protected int speed; 
    [SerializeField] protected int actionCost; public int ActionCost => actionCost;
    [SerializeField] public int nextActionTime;
    [SerializeField] protected float condition;

    public List<ItemSaveData> inventory = new List<ItemSaveData>();

    protected Dictionary<StatType, float> statBonuses = new Dictionary<StatType, float>();
    protected Dictionary<StatType, float> statMultipliers = new Dictionary<StatType, float>();

    // The "Radio Station" - other scripts can subscribe to this
    public event Action OnStatsChanged;
    // Whenever you change a stat, call this helper
    public void NotifyStatsChanged()
    {
        OnStatsChanged?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void RandomizeStats() { }
    public virtual void CalculateStats()
    {
        maxHealth = VIT;
        currentHealth = maxHealth;
        physicalDamage = STR / 2;
        magicDamage = INT / 2;

        aggroWeight = 100;

        speed = AGI + 300;
        actionCost = 1000 - speed;
        if (actionCost < 100) actionCost = 100;
    }

    // Convert this unit's current stats into a data object
    public UnitSaveData SaveToData()
    {
        return new UnitSaveData
        {
            unitTeam = this.unitTeam,
            unitID = this.unitID,
            unitName = this.unitName,
            STR = this.STR,
            INT = this.INT,
            DEX = this.DEX,
            WIS = this.WIS,
            VIT = this.VIT,
            END = this.END,
            SPI = this.SPI,
            AGI = this.AGI,
            GRO = this.GRO,
            age = this.age,
            spriteID = this.spriteID,
            partyNum = this.partyNum,
            isHired = this.isHired,
            inventory = this.inventory,
            condition = this.condition
        };
    }

    // Apply data from a saved object back onto this unit
    public void LoadFromData(UnitSaveData data)
    {
        this.unitTeam = data.unitTeam;
        this.unitID = data.unitID;
        this.unitName = data.unitName;
        this.STR = data.STR;
        this.INT = data.INT;
        this.DEX = data.DEX;
        this.WIS = data.WIS;
        this.VIT = data.VIT;
        this.END = data.END;
        this.SPI = data.SPI;
        this.AGI = data.AGI;
        this.GRO = data.GRO;
        this.age = data.age;
        this.spriteID = data.spriteID;
        this.partyNum = data.partyNum;
        this.isHired = data.isHired;
        this.inventory = data.inventory;
        this.condition = data.condition;

        CalculateStats();
    }

    //public void IncParty()
    //{
    //    partyNum += 1;
    //}
    //public void DecParty()
    //{
    //    partyNum -= 1;
    //}

    public void CalculateConditionLoss()
    {
        // 1. Combine stats to create a "Gravity" value (Power)
        // At 1/1 stats, power is 1.0 (perfectly random/linear)
        // At 500/500 stats, power is 4.0 (heavily weighted toward 20)
        float combinedStats = (EffectiveDEX + EffectiveWIS) / 1000f; // 0 to 1
        float power = Mathf.Lerp(1f, 4f, combinedStats);

        // 2. Generate a random value (0 to 1) and apply the power
        // This is the "magic" part.
        float biasedRandom = Mathf.Pow(UnityEngine.Random.value, power);

        // 3. Map that value to your 20-50 range
        // Since we want higher stats to result in LOWER numbers, 
        // we use (1 - biasedRandom) to flip the curve.
        float loss = Mathf.Lerp(20f, 50f, 1f - biasedRandom);

        condition = Mathf.Max(0, condition - loss);
    }

    public void CalculateConditionGain() // NOT USED TBH CHECK SAVE MANAGER
    {
        float gain = UnityEngine.Random.Range(70, 100);
        condition = Mathf.Max(condition + gain, 100);
    }

    // ===================================================================================================
    // Combat
    // ===================================================================================================

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        ShowDamagePopup(amount);

        RefreshMyUI();

        if (currentHealth <= 0) Die();
    }

    public virtual void Die()
    {
        if (unitTeam == Team.Adventurer) ProgressManager.Instance.rating -= 0.5f;
        isDead = true;
    }

    public virtual string GetDamageType()
    {
        float totalChance = DEX + WIS;
        float roll = UnityEngine.Random.Range(0f, totalChance);
        if (roll <= DEX) return "physical";
        else return "magic";
    }

    public virtual void CalculateOutgoingDamage(float damage, float targetArmour)
    {
        float k = 125f;                            // K     100DEF  300DEF	500DEF 
        float multiplier = k / (k + targetArmour); // 125	44.4%	70.6%	80.0% 
        outgoingDamage = damage * multiplier;
    }

    public virtual IEnumerator Attack(Unit target)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.transform.position;

        // 1. Lunge Forward (Move 90% of the way to the target)
        Vector3 lungeDestination = Vector3.Lerp(startPos, targetPos, 1f);
        float elapsedTime = 0f;
        float duration = 0.15f; // Fast lunge

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, lungeDestination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // --- HIT POINT ---
        // This is where the actual damage happens (mid-animation)
        ExecuteDamageLogic(target);

        // 2. Return to Original Position
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(lungeDestination, startPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos; // Ensure we are exactly back at home
    }

    private void ExecuteDamageLogic(Unit target)
    {
        string damageType = GetDamageType();

        if (damageType == "physical")
            CalculateOutgoingDamage(physicalDamage, target.EffectiveEND);
        else if (damageType == "magic")
            CalculateOutgoingDamage(magicDamage, target.EffectiveSPI);

        target.TakeDamage(outgoingDamage);
    }

    private void RefreshMyUI()
    {
        if (unitTeam == Team.Adventurer)
            GetComponent<AdventurerCombatUI>()?.DisplayStats();
        else
            GetComponent<EnemyCombatUI>()?.DisplayStats();
    }

    private void ShowDamagePopup(float amount)
    {
        CombatManager.Instance.SpawnPopup(transform.position + Vector3.up, amount);
    }


    // ===================================================================================================
    // Training
    // ===================================================================================================

    public void ApplyTraining(TrainingOption option)
    {
        StatType mainStat = option.changes.Count > 0 ? option.changes[0].stat : StatType.RandomStat;

        foreach (StatChange change in option.changes)
        {
            // 1. Calculate the value (Fixed or Random Range)
            int finalValue = UnityEngine.Random.Range(change.minAmount, change.maxAmount + 1);

            // 2. Determine the Stat to target
            StatType targetStat = change.stat;

            if (targetStat == StatType.RandomStat)
            {
                targetStat = GetRandomStat(exclude: mainStat);
            }

            // 3. FIX: Pass the 'mainStat' as the third argument
            ApplyToVariable(targetStat, finalValue, mainStat);
        }
    }

    StatType GetRandomStat(StatType exclude)
    {
        StatType picked = exclude;
        while (picked == exclude || picked == StatType.RandomStat)
        {
            picked = (StatType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(StatType)).Length - 1);
        }
        return picked;
    }

    public void ApplyHiddenResult(TrainingResult result)
    {
        StatType mainStat = result.visibleResults.Count > 0 ? result.visibleResults[0].stat : StatType.RandomStat;

        // 1. Apply knowns
        foreach (var change in result.visibleResults)
        {
            ApplyToVariable(change.stat, change.minAmount, mainStat);
        }

        // 2. Track which stats we've already used for THIS training
        List<StatType> usedStats = new List<StatType>();
        usedStats.Add(mainStat); // Don't pick the main stat

        // 3. Apply hidden mystery rolls
        foreach (var hidden in result.hiddenResults)
        {
            StatType actualStat = GetUniqueRandomStat(usedStats);
            usedStats.Add(actualStat); // Add to list so it's not picked again

            ApplyToVariable(actualStat, hidden.minAmount, mainStat);
        }

        // Trigger the update at the end
        NotifyStatsChanged();
    }

    // New helper to find a stat not in the 'exclude' list
    StatType GetUniqueRandomStat(List<StatType> excludeList)
    {
        StatType picked = StatType.RandomStat;
        int safetyNet = 0;

        while (safetyNet < 100)
        {
            picked = (StatType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(StatType)).Length);

            // Ensure it's not a helper Enum AND not in our exclusion list
            if (picked != StatType.RandomStat &&
                picked != StatType.AllOtherStats &&
                !excludeList.Contains(picked))
            {
                return picked;
            }
            safetyNet++;
        }
        return StatType.STR; // Fallback
    }

    // Updated ApplyToVariable to handle the "AllOtherStats" logic
    void ApplyToVariable(StatType type, int value, StatType excludedStat)
    {
        if (type == StatType.ReshuffleAll)
        {
            ReshuffleStats();
        }
        if (type == StatType.AllOtherStats)
        {
            foreach (StatType s in System.Enum.GetValues(typeof(StatType)))
            {
                // Don't apply to the helper Enums or the excluded "Main" stat
                if (s != StatType.AllOtherStats && s != StatType.RandomStat && s != excludedStat)
                {
                    ModifyActualStat(s, value);
                }
            }
        }
        else
        {
            ModifyActualStat(type, value);
        }
    }

    // This handles the actual math
    void ModifyActualStat(StatType type, int value)
    {
        switch (type)
        {
            case StatType.STR: STR = Mathf.Max(1, STR + value); break;
            case StatType.INT: INT = Mathf.Max(1, INT + value); break;
            case StatType.DEX: DEX = Mathf.Max(1, DEX + value); break;
            case StatType.WIS: WIS = Mathf.Max(1, WIS + value); break;
            case StatType.VIT: VIT = Mathf.Max(1, VIT + value); break;
            case StatType.END: END = Mathf.Max(1, END + value); break;
            case StatType.SPI: SPI = Mathf.Max(1, SPI + value); break;
            case StatType.AGI: AGI = Mathf.Max(1, AGI + value); break;
        }
    }

    public void ReshuffleStats()
    {
        List<StatType> validStats = new List<StatType> {
        StatType.STR, StatType.INT, StatType.DEX, StatType.WIS,
        StatType.VIT, StatType.END, StatType.SPI, StatType.AGI,
    };

        // 1. Calculate Total Pool
        int totalPool = STR + INT + DEX + WIS + VIT + END + SPI + AGI;

        // 2. Reset all stats to 0
        STR = INT = DEX = WIS = VIT = END = SPI = AGI = 0;

        // 3. Guarantee a minimum of 1 for every stat
        // This ensures no stat is ever 0.
        foreach (StatType stat in validStats)
        {
            if (totalPool > 0)
            {
                ApplyToVariable(stat, 1, StatType.RandomStat);
                totalPool--;
            }
        }

        // 4. Distribute the remaining pool in erratic chunks
        while (totalPool > 0)
        {
            StatType randomTarget = validStats[UnityEngine.Random.Range(0, validStats.Count)];

            // We take a random bite of the pool. 
            // Using a high upper bound (totalPool / 2) creates "Spiky" stats.
            int maxPossibleChunk = Mathf.Max(1, totalPool / 6);
            int amountToAdd = UnityEngine.Random.Range(1, maxPossibleChunk + 1);

            ApplyToVariable(randomTarget, amountToAdd, StatType.RandomStat);
            totalPool -= amountToAdd;
        }

        NotifyStatsChanged();
    }

    // ===================================================================================================
    // Items
    // ===================================================================================================

    public int GetStat(StatType type, int baseValue)
    {
        float flat = statBonuses.ContainsKey(type) ? statBonuses[type] : 0;
        float mult = statMultipliers.ContainsKey(type) ? statMultipliers[type] : 0;

        // Formula: (Base + 10 Flat) * (1 + 0.15 Percent)
        return Mathf.RoundToInt((baseValue + flat) * (1 + mult));
    }

    // Call this whenever an item is added, removed, or the game is loaded
    public void RefreshItemModifiers()
    {
        statBonuses.Clear();
        statMultipliers.Clear();
        // Clear unique effects here if you have a list of them

        foreach (ItemSaveData item in inventory)
        {
            // 1. Process Main Stat
            ApplyModToInternalLookup(item.mainStat);

            // 2. Process all Substats
            foreach (var mod in item.subStats)
            {
                ApplyModToInternalLookup(mod);
            }

            // 3. Handle Unique Effects
            if (item.rarity == Rarity.Unique && !string.IsNullOrEmpty(item.uniqueEffect))
            {
                ActivateUniqueEffect(item.uniqueEffect);
            }
        }

        CalculateStats();
        NotifyStatsChanged();
    }

    private void ApplyModToInternalLookup(StatModifier mod)
    {
        // Safety check: ensure we only modify the 8 core stats
        // This ignores things like StatType.ReshuffleAll if they were somehow added
        if (mod.type == StatType.RandomStat || mod.type == StatType.AllOtherStats) return;

        if (mod.isPercent)
        {
            if (!statMultipliers.ContainsKey(mod.type)) statMultipliers[mod.type] = 0;
            statMultipliers[mod.type] += mod.value;
        }
        else
        {
            if (!statBonuses.ContainsKey(mod.type)) statBonuses[mod.type] = 0;
            statBonuses[mod.type] += mod.value;
        }
    }

    public void AddItem(ItemData blueprint, Rarity rarity)
    {
        // Create the new "Rolled" item data
        ItemSaveData newItem = new ItemSaveData(blueprint, rarity);
        inventory.Add(newItem);

        RefreshItemModifiers();
    }

    public void RemoveItem(ItemSaveData item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            RefreshItemModifiers();
        }
    }

    public void LoadInventory(List<ItemSaveData> savedItems)
    {
        inventory = savedItems;
        RefreshItemModifiers();
    }

    private void ActivateUniqueEffect(string effectName)
    {
        // Placeholder for your unique logic (e.g., "Life Steal", "Double Turn")

    }
}
