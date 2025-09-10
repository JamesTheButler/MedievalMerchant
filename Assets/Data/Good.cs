namespace Data
{
    public enum Good
    {
        // Forest Town
        T1Logs = 0,
        T1WildGame = 1,
        T1Berries = 2,
        T1Herbs = 18,
        T2Planks = 3,
        T2Leather = 4,
        T2BerryWine = 19,
        T2HerbalMedicine = 20,
        T3Furniture = 5, // planks and leather

        // Mining Town
        T1RawStone = 6,
        T1RawOre = 7,
        T1Clay = 8,
        T1Goats = 21,
        T2Iron = 9,
        T2CutStone = 10,
        T2GoatWool = 22,
        T2Bricks = 23,
        T3Tools = 11, // cut stone and iron

        // Fishing Town
        T1Fish = 12,
        T1Shells = 13,
        T1Seaweed = 14,
        T1Mussels = 24,
        T2DriedFish = 15,
        T2Lime = 16,
        T2Pigments = 25,
        T2Pearls = 26,
        T3Ceramics = 17, // lime and pigments

        // Farming Town
        T1Grain = 33,
        T1Flax = 34,
        T1Hay = 35,
        T1Vegetables = 36,
        T2Flour = 37,
        T2Linen = 38,
        T2Pickles = 39,
        T2Thatch = 40,
        T3Tents = 41, // linen and thatch

        // Combos
        T3Weapons = 27, //Forest + Mountains: planks and iron
        T3PaddedClothing = 28, //Forest + Fields: planks and iron
        T3MedicalRations = 29, //Forest + Ocean: herbal meds and pigments 
        T3Bread = 30, //Mountains + Fields: bricks and flour
        T3Jewelery = 31, //Mountains + Ocean: iron and pearls
        T3Provisions = 32, //Fields + Ocean: pickles and dried fish
    }
}