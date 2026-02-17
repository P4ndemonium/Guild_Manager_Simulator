using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : Unit
{
    public override void RandomizeStats()
    {
        unitID = System.Guid.NewGuid().ToString();

        unitRank = Rank.F;

        STR = Random.Range(1, 101);
        INT = Random.Range(1, 101);
        DEX = Random.Range(1, 101);
        WIS = Random.Range(1, 101);
        VIT = Random.Range(1, 101);
        END = Random.Range(1, 101);
        SPI = Random.Range(1, 101);
        AGI = Random.Range(1, 101);

        GRO = Random.Range(1, 101);
        age = Random.Range(17, 31);

        spriteID = Random.Range(0, library.allPossibleSprites.Count);
        partyNum = 0;
        baseStatTotal = STR + INT + DEX + WIS + VIT + END + SPI + AGI;
        hiringPrice = ((baseStatTotal / 8) + Random.Range(0, 50));

        condition = 100;

        CalculateStats();
    }

    // Container for Given Names
    public List<string> givenNames = new List<string> {
        "Akari", "Aoi", "Yui", "Hina", "Ichika", "Himari", "Mei", "Kanna", "Sara", "Mio",
        "Ema", "Koharu", "Suzu", "Tsumugi", "Sakura", "Riko", "Yua", "Ren", "Asahi", "Mitsuki",
        "Nanami", "Koto", "Natsuki", "Ayaka", "Hana", "Misaki", "Rio", "Haruka", "Saki", "Yuna",
        "Miku", "Rin", "Momoka", "Anzu", "Rei", "Maki", "Sora", "Noa", "Honoka", "Karin",
        "Hikari", "Yuzuki", "Ami", "Eri", "Shiori", "Kyoko", "Mina", "Yuki", "Asami", "Risa",
        "Mayu", "Kasumi", "Kana", "Natsumi", "Miyu", "Yuka", "Hitomi", "Sae", "Aiko", "Yoko",
        "Nao", "Emi", "Rumi", "Ayumi", "Sayaka", "Chika", "Miki", "Minori", "Haruna", "Kaori",
        "Sayo", "Fumina", "Mizuki", "Chihiro", "Suzume", "Nagisa", "Tamaki", "Shizuka", "Mone", "Satsuki",
        "Mana", "Rika", "Ayane", "Chinatsu", "Tomoka", "Sana", "Kaho", "Umi", "Momo", "Kiyomi",
        "Sumire", "Michiko", "Kazue", "Setsuko", "Tomoko", "Amane", "Tsukiko", "Yuriko", "Ritsuko", "Maiko"
    };

    // Container for Family Names
    public List<string> familyNames = new List<string> {
        "Sato", "Suzuki", "Takahashi", "Tanaka", "Watanabe", "Ito", "Yamamoto", "Nakamura", "Kobayashi", "Kato",
        "Yoshida", "Yamada", "Sasaki", "Yamaguchi", "Matsumoto", "Inoue", "Kimura", "Hayashi", "Shimizu", "Abe",
        "Ikeda", "Hashimoto", "Yamashita", "Ishikawa", "Mori", "Nakajima", "Maeda", "Fujita", "Ogawa", "Okada",
        "Hasegawa", "Murakami", "Kondo", "Ishii", "Saito", "Sakamoto", "Endo", "Aoki", "Fujii", "Nishimura",
        "Fukuda", "Ota", "Miura", "Fujiwara", "Okamoto", "Matsuda", "Nakagawa", "Nakano", "Harada", "Ono",
        "Tamura", "Takeuchi", "Kaneko", "Wada", "Nakayama", "Ishida", "Ueda", "Morita", "Kojima", "Shibata",
        "Sakai", "Hara", "Kudo", "Yokoyama", "Miyazaki", "Miyamoto", "Uchida", "Takagi", "Ando", "Taniguchi",
        "Ohno", "Maruyama", "Imai", "Takada", "Fujimoto", "Murata", "Takeda", "Ueno", "Sugiyama", "Masuda",
        "Sugawara", "Hirano", "Koyama", "Otsuka", "Chiba", "Kubo", "Matsui", "Iwasaki", "Sakurai", "Kinoshita",
        "Noguchi", "Matsuo", "Nomura", "Kikuchi", "Sano", "Onishi", "Sugimoto", "Arai", "Ishihara", "Ichikawa"
    };

    virtual public void RandomName()
    {
        string family = familyNames[Random.Range(0, familyNames.Count)];
        string given = givenNames[Random.Range(0, givenNames.Count)];

        unitName = family + " " + given;
    }

    // Start is called before the first frame update
    void Start()
    {
        unitTeam = Team.Adventurer;
        RandomName();
        RandomizeStats();
    }
}
