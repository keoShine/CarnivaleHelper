using System.Collections.Generic;

namespace CarnivaleHelper.Enums
{
    public static class SpellListEnums
    {
        //Most of these are unneccesary due to changing strategy for condition tracking on spell-based targets -- do not attempt to code at 2AM again
        //public static List<uint> GeneralActions = new()
        //{
        //    3,     //Sprint
        //    7559,  //Surecast
        //    7560,  //Addle
        //    7561,  //Swiftcast
        //    7562,  //Lucid Dreaming
        //    25880, //Sleep
        //};

        //public static List<uint> BeginnerSpells = new()
        //{
        //    11385, //Water Cannon
        //    11386, //Song of Torment
        //    11391, //Plaincracker
        //    11392, //Acorn Bomb
        //    11393, //Bristle
        //    11394, //Mind Blast
        //    11395, //Blood Drain
        //    11396, //Bomb Toss
        //    11398, //Drill Cannons
        //    11399, //The Look
        //    11401, //Loom
        //    11407, //Final String
        //    11408, //Self-Destruct
        //    11409, //Transfusion
        //    11411, //Off-Guard
        //    11414, //Level 5 Petrify
        //    11415, //Moon Flute
        //    11418, //Ice Spikes
        //    11419, //The Ram's Voice
        //    11420, //The Dragon's Voice
        //    11421, //Pecurilar Light
        //    11423, //Flying Sardine
        //    18295, //Alpine Draft
        //    18298, //Electrogenesis
        //    18299, //Kaltstrahl
        //    18301, //Chirp
        //    18302, //Eerie Soundwave
        //    18305, //Magic Hammer
        //    18306, //Avail
        //    18307, //Frog Legs
        //    18310, //White Knight's Tour
        //    18311, //Black Knight's Tour
        //    18313, //Launcher
        //    18315, //Cactguard
        //    18316, //Revenge Blast
        //    18318, //Exuviation
        //    18320, //Devour
        //    23264, //Triple Trident
        //    23265, //Tingle
        //    23269, //Stotram
        //    23270, //Saintly Beam
        //    23271, //Feculent Flood
        //    23275, //The Rose of Destruction
        //    23276, //Basic Instinct
        //    23277, //Ultrabivration
        //    23278, //Blaze
        //    23281, //Aetherial Spark
        //    23283, //Malediction of Water
        //    23284, //Choco Meteor
        //    23416  //Stotram -- Healer
        //};

        //public static List<uint> ModerateSpells = new()
        //{
        //    11388, //Bad Breath
        //    11389, //Flying Frenzy
        //    11403, //Faze
        //    11406, //White Wind
        //    11410, //Toad Oil
        //    11422, //Ink Jet
        //    11425, //Fire Angon
        //    18296, //Protean Wave
        //    18303, //Pom Cure
        //    18309, //Whistle
        //    18319, //Reflux
        //    18321, //Condensced Libra
        //    23266, //Tatami-gaeshi
        //    23272, //Angel's Snack
        //    23280, //Dragon Force
        //    23285  //Matra Magic
        //};

        //public static List<uint> AdvancedSpells = new()
        //{
        //    11383, //Snort
        //    11384, //4-Tonze Weight
        //    11387, //High Voltage
        //    11388, //Aqua Breath
        //    11397, //1000 Needles
        //    11400, //Sharpened Knife
        //    11402, //Flamethrower
        //    11404, //Glower
        //    11405, //Missile
        //    11412, //Sticky Tongue
        //    11413, //Tail Screw
        //    11416, //Doom
        //    11417, //Mighty Guard
        //    11424, //Diamondback
        //    11426, //Feather Rain
        //    11427, //Eruption
        //    11428, //Mountain Buster
        //    11429, //Shock Strike
        //    11430, //Glass Dance
        //    11431, //Veil of the Whorl
        //    18297, //Northerlies
        //    18300, //Abyssal Transfixtion
        //    18304, //Gobskin
        //    18308, //Sonic Boom
        //    18312, //Level 5 Death
        //    18314, //Perpetual Ray
        //    18317, //Angel Whisper
        //    18322, //Aetheric Mimicry
        //    18323, //Surpanakha
        //    18324, //Quasar
        //    18325, //J Kick
        //    23267, //Cold Fog
        //    23273, //Chelonian Gate
        //    23279, //Mustard Bomb
        //    23287, //Both Ends
        //    23288, //Phantom Flurry
        //    23290, //Nightbloom
        //    23282, //Hydro Pull
        //    23286  //Peripheral Synthesis
        //};

        public static List<uint> BreathAttacks = new()
        {
            11383, //Snort
            11388, //Bad Breath
            11390, //Aqua Breath
            11399, //The Look
            11402, //Flamethrower
            11404, //Glower
            11412, //Sticky Tongue
            11414, //Level 5 Petrify
            11422, //Ink Jet
            11423, //Flying Sardine
            18297, //Northerlies
            18298, //Electrogenesis
            18134, //Perpetual Ray
            18319, //Reflux
            18320, //Devour
            23278, //Blaze
            23279  //Mustard Bomb
        };
    }
}
