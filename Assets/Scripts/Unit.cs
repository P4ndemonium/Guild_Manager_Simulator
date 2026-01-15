using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header("Base Statistics")]
    [SerializeField] protected string unitName;

    [SerializeField] protected int STR; // Strength     - Physical Damage
    [SerializeField] protected int INT; // Intelligence - Magic Damage
    [SerializeField] protected int DEX; // Dexterity    - Chance of physical damage used in attack
    [SerializeField] protected int WIS; // Wisdom       - Chance of magic damage used in attack
    [SerializeField] protected int VIT; // Vitality     - Base HP
    [SerializeField] protected int END; // Endurance    - Physical damage reduction
    [SerializeField] protected int SPI; // Spirit       - Magic damage reduction
    [SerializeField] protected int AGI; // Agility      - Chance of getting an attack on this units turn

    [SerializeField] protected float GRO; // Growth     - Rate of improvement or reduction of stats      # Maybe make it change every year and reduce as age goes by
    [SerializeField] protected float age;

    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float physicalDamage;
    [SerializeField] protected float magicDamage;

    [Header("UI References")]
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected TextMeshProUGUI statsTextLeft;
    [SerializeField] protected TextMeshProUGUI statsTextRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void RandomizeStats() { }
    public virtual void DisplayStats() { }
}
