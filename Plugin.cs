using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using DiskCardGame;
using UnityEngine;
using TribeAPI;
using System.Linq;
using System.Reflection;

namespace ZephtPvZ
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "zepht.inscryption.ZephtPvZ";
        private const string PluginName = "PvZbyZepht";
        private const string PluginVersion = "0.2.1";

        public static int Sun;
        public static Texture[] costDecals;

        public static NewTribe PlantsSun;
        public static NewTribe PlantsAttack;
        public static NewTribe PlantsDefend;
        public static NewTribe PlantsSupport;
        public static NewTribe PlantsBomb;

        public static Trait Mushroom = (Trait)7801;
        public static Trait NoSacs = (Trait)7802;

        public static ResourceType SunResource = (ResourceType)78;

        public static Card SunCounterCard;
        public static CardInfo SunCounterCardInfo = new CardInfo();
        public static GameObject SunCounterObject;

        public static Card SunCoinCard;
        public static CardInfo SunCoinCardInfo = new CardInfo();
        public static GameObject SunCoinObject;


        internal static ManualLogSource Log;

        public static System.Random rng = new System.Random();

        private void Awake()
        {
            Logger.LogInfo($"Loaded {PluginName}!");

            //---this adds the stuff defined below---
            Log = base.Logger;

            Harmony harmony = new Harmony(PluginGuid);
            harmony.PatchAll();

            makeDecals();

            SunCounterBackground.Initialize();
            InstantCardBackground.Initialize();

            AddSunCost();
            AddTribes();
            AddSigils();

            AddPlants();

        }

        private void AddTribes()
        {
            string tribeName;

            //sun
            tribeName = "PlantTribeSun";
            PlantsSun = new NewTribe(findTexture(tribeName), true, findTexture(tribeName + "Pick"));

            //attacker
            tribeName = "PlantTribeAttack";
            PlantsAttack = new NewTribe(findTexture(tribeName), true, findTexture(tribeName + "Pick"));

            //defend
            tribeName = "PlantTribeDefend";
            PlantsDefend = new NewTribe(findTexture(tribeName), true, findTexture(tribeName + "Pick"));

            //support
            tribeName = "PlantTribeSupport";
            PlantsSupport = new NewTribe(findTexture(tribeName), true, findTexture(tribeName + "Pick"));

            //bomb
            tribeName = "PlantTribeBomb";
            PlantsBomb = new NewTribe(findTexture(tribeName), true, findTexture(tribeName + "Pick"));

        }

        private void AddPlants() //mostly same as Jsonloader
        {
            
            //prefabs
            List<CardMetaCategory> MetaC_standard = new List<CardMetaCategory> {
                CardMetaCategory.ChoiceNode,
                CardMetaCategory.TraderOffer
            };
            List<CardMetaCategory> MetaC_choice = new List<CardMetaCategory> {
                CardMetaCategory.ChoiceNode
            };
            List<CardMetaCategory> MetaC_rare = new List<CardMetaCategory>
            {
                CardMetaCategory.Rare,
            };
            List<CardMetaCategory> MetaC_null = new List<CardMetaCategory>
            {
            };

            //NewSpecialAbility shroomAttack = ShroomAttack.InitStatIconAndAbility();

            List<CardAppearanceBehaviour.Appearance> terrainAppearance = new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.TerrainBackground, CardAppearanceBehaviour.Appearance.TerrainLayout };
            List<CardAppearanceBehaviour.Appearance> goatAppearance = new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.SexyGoat };
            List<CardAppearanceBehaviour.Appearance> rareAppearance = new List<CardAppearanceBehaviour.Appearance> {CardAppearanceBehaviour.Appearance.RareCardBackground};
            List<CardAppearanceBehaviour.Appearance> goldAppearance = new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.GoldEmission , InstantCardBackground.CustomAppearance };
            List<CardAppearanceBehaviour.Appearance> instantAppearance = new List<CardAppearanceBehaviour.Appearance> { InstantCardBackground.CustomAppearance };

            Texture2D texDecal = findTexture("ZA");

            string prefix = "zep_pvz_";



            //==========make plants=========================================================

            //----attack----
            //peashooter -evo
            {
                string imageName = "Peashooter"; //-image info

                List<Ability> abilities = new List<Ability> { }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = "pea2-04"; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix+
                    "pea1" +//-name
                    "-02", //cost
                    "Peashooter", //-display name
                    1, //-attack
                    2, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "A plant commonly used to fend off the undead", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    ); 
                
                
            }


            //repeater -evo
            {
                string imageName = "Repeater"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.SplitStrike }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = "pea3-05"; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "pea2" +//-name
                    "-04", //cost
                    "Repeater", //-display name
                    1, //-attack
                    2, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple, 
                    CardTemple.Nature, 
                    description: "A more powerful version of the iconic peashooter", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //threepeater
            {
                string imageName = "Threepeater"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.TriStrike }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "pea3" +//-name
                    "-05", //cost
                    "Threepeater", //-display name
                    1, //-attack
                    3, //-health
                    MetaC_rare, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "they say 3 heads are better than one", //-description,
                    appearanceBehaviour: rareAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    ) ;
            }

            //cactus
            {
                string imageName = "Cactus"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Sharp, Ability.Reach }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "splinters"; //-tail info

                NewCard.Add(
                    prefix +
                    "cactus" +//-name
                    "-05", //cost
                    "Cactus", //-display name
                    2, //-attack
                    2, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Try not to prick yourself on it", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //homingThistle 
            {
                string imageName = "Thistle"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Sniper }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "splinters"; //-tail info

                NewCard.Add(
                    prefix +
                    "thistle" +//-name
                    "-03", //cost
                    "Homing Thistle", //-display name
                    1, //-attack
                    1, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "This one tracks down its target with ease", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //cattail
            {
                string imageName = "Cattail"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Sniper, Ability.Submerge }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "cattail" +//-name
                    "-03", //cost
                    "Cattail", //-display name
                    1, //-attack
                    1, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Nyah~", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //chomper
            {
                string imageName = "Chomper"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Deathtouch}; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "chomper" +//-name
                    "-04", //cost
                    "Chomper", //-display name
                    2, //-attack
                    1, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "No bark and all bite", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //bonkChoy
            {
                string imageName = "BonkChoy"; //-image info

                List<Ability> abilities = new List<Ability> { }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info
                specialTriggeredAbilities.Add(SunCost.SpecialTriggeredAbility);

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "bonkchoy" +//-name
                    "-04", //cost
                    "bonk choy", //-display name
                    2, //-attack
                    2, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "A feisty one for sure", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );


            }
            
            //citron
            {
                string imageName = "Citron"; //-image info

                List<Ability> abilities = new List<Ability> { }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info
                specialTriggeredAbilities.Add(SunCost.SpecialTriggeredAbility);

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "citron" +//-name
                    "-05", //cost
                    "Citron", //-display name
                    3, //-attack
                    2, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "If you can afford to play it, it's zest is bound to aid you", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );


            }

            //redStinger
            {
                string imageName = "RedStinger"; //-image info

                List<Ability> abilities = new List<Ability> { }; //-sigil info
                List<Trait> traits = new List<Trait> {}; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility, SpecialTriggeredAbility.BellProximity }; //-sp ability info
                specialTriggeredAbilities.Add(SunCost.SpecialTriggeredAbility);

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "redstinger" +//-name
                    "-04", //cost
                    "Red Stinger", //-display name
                    0, //-attack
                    1, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Position it strategically if you want to use its full potential", //-description
                    appearanceBehaviour: null, // -appearance

                    specialStatIcon: SpecialStatIcon.Bell, //special stat icon

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)

                    );


            }

            //spikeweed 
            {
                string imageName = "Spikeweed"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Sentry }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = "spike2-05"; //-evo info
                string tailName = "splinters"; //-tail info

                NewCard.Add(
                    prefix +
                    "spike1" +//-name
                    "-03", //cost
                    "Spikeweed", //-display name
                    1, //-attack
                    1, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Will you lay the spikeweed as a trap for those who dare approach it?", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //spikerock 
            {
                string imageName = "Spikerock"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Sentry, Ability.Sentry }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "splinters"; //-tail info

                NewCard.Add(
                    prefix +
                    "spike2" +//-name
                    "-05", //cost
                    "Spikerock", //-display name
                    2, //-attack
                    3, //-health
                    MetaC_rare, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Anybody who attempts to trample it will meet a sharp fate.", //-description
                    appearanceBehaviour: rareAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }


            //----attack shrooms----
            //puffShroom -evo
            {
                string imageName = "Puff"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Brittle }; //-sigil info
                List<Trait> traits = new List<Trait> {Mushroom}; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = "fume-04"; //-evo info
                string tailName = "shroomchunks"; //-tail info

                NewCard.Add(
                    prefix +
                    "puff" +//-name
                    "-00", //cost
                    "PuffShroom", //-display name
                    1, //-attack
                    1, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "It might not be very powerful, but it can get you out of a pinch", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //seaShroom
            {
                string imageName = "SeaShroom"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Submerge }; //-sigil info
                List<Trait> traits = new List<Trait> { Mushroom }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info


                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info
                //List<SpecialAbilityIdentifier> specialAbilities = new List<SpecialAbilityIdentifier> { shroomAttack.id };

                string evoName = null; //-evo info
                string tailName = "shroomchunks"; //-tail info

                NewCard.Add(
                    prefix +
                    "sea" +//-name
                    "-00", //cost
                    "SeaShroom", //-display name
                    1, //-attack
                    1, //-health
                    MetaC_rare, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Just a little mushroom, sitting on the water", //-description
                    appearanceBehaviour: rareAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    //,specialAbilitiesIdsParam: (specialAbilities)
                    );
            }

            //scaredyShroom 
            {
                string imageName = "Scaredy"; //-image info

                List<Ability> abilities = new List<Ability> { TacticalRetreat.ability }; //-sigil info
                List<Trait> traits = new List<Trait> { Mushroom }; //-trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility}; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "shroomchunks"; //-tail info

                NewCard.Add(
                    prefix +
                    "scaredy" +//-name
                    "-01", //cost
                    "ScaredyShroom", //-display name
                    1, //-attack
                    1, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Some may consider this one a coward for its tactics", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //fumeShroom 
            {
                string imageName = "Fume"; //-image info

                List<Ability> abilities = new List<Ability> { Pierce.ability}; //-sigil info
                List<Trait> traits = new List<Trait> { Mushroom }; //-trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "shroomchunks"; //-tail info

                NewCard.Add(
                    prefix +
                    "fume" +//-name
                    "-04", //cost
                    "FumeShroom", //-display name
                    1, //-attack
                    1, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Its fumes can hit multiple foes", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //sporeShroom 
            {
                string imageName = "Spore"; //-image info

                List<Ability> abilities = new List<Ability> { SporeSpread.ability}; //-sigil info
                List<Trait> traits = new List<Trait> { Mushroom }; //-trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsAttack.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "shroomchunks"; //-tail info

                NewCard.Add(
                    prefix +
                    "spore" +//-name
                    "-03", //cost
                    "SporeShroom", //-display name
                    1, //-attack
                    1, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Put divide and conquer into practice", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }



            //----sun---- 
            //sunflower -evo 
            {
                string imageName = "Sunflower"; //-image info

                List<Ability> abilities = new List<Ability> { Sunmaker2.ability}; //-sigil info
                List<Trait> traits = new List<Trait> { (Trait)5103 ,Trait.Goat}; //-trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSun.tribe, Tribe.Squirrel }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = "sunflower2-02"; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "sunflower1" +//-name
                    "-01", //cost
                    "Sunflower", //-display name
                    0, //-attack
                    1, //-health
                    MetaC_null, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "These can power entire infantries", //-description
                    appearanceBehaviour: goatAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    altTex: findTexture("SunflowerAlt"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)

                    );
            }

            //twinflower
            {
                string imageName = "Twinflower"; //-image info

                List<Ability> abilities = new List<Ability> { Sunmaker3.ability }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSun.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "sunflower2" +//-name
                    "-02", //cost
                    "Twin Sunflower", //-display name
                    0, //-attack
                    2, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Truly a great powerplant", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //sunshroom S -evo  *art
            {
                string imageName = "SunShroomS"; //-image info

                List<Ability> abilities = new List<Ability> { Sunmaker1.ability, Ability.Evolve }; //-sigil info
                List<Trait> traits = new List<Trait> { Mushroom }; //-trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSun.tribe}; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = "sunshroom2-01"; //-evo info
                string tailName = "shroomchunks"; //-tail info

                NewCard.Add(
                    prefix +
                    "sunshroom1" +//-name
                    "-01", //cost
                    "SunShroom", //-display name
                    0, //-attack
                    1, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "allow the sunshroom some time to reach its full potential", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)

                    );
            }

            //sunshroom M -evo  *art
            {
                string imageName = "SunShroomM"; //-image info

                List<Ability> abilities = new List<Ability> { Sunmaker2.ability, Ability.Evolve }; //-sigil info
                List<Trait> traits = new List<Trait> { Mushroom }; //-trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSun.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = "sunshroom3-01"; //-evo info
                string tailName = "shroomchunks"; //-tail info

                NewCard.Add(
                    prefix +
                    "sunshroom2" +//-name
                    "-01", //cost
                    "SunShroom", //-display name
                    0, //-attack
                    2, //-health
                    MetaC_null, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "allow the sunshroom some time to reach its full potential", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)

                    );
            }
            
            //sunshroom L  *art
            {
                string imageName = "SunShroomL"; //-image info

                List<Ability> abilities = new List<Ability> { Sunmaker3.ability }; //-sigil info
                List<Trait> traits = new List<Trait> { Mushroom }; //-trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSun.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "shroomchunks"; //-tail info

                NewCard.Add(
                    prefix +
                    "sunshroom3" +//-name
                    "-01", //cost
                    "SunShroom", //-display name
                    0, //-attack
                    3, //-health
                    MetaC_null, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "allow the sunshroom some time to reach its full potential", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)

                    );
            }

            //toadstool
            {
                string imageName = "Toadstool"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Deathtouch, SunKill.ability }; //-sigil info
                List<Trait> traits = new List<Trait> {Mushroom }; //trait info
                List<Texture> decals = new List<Texture> { texDecal  }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSun.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "shroomchunks"; //-tail info

                NewCard.Add(
                    prefix +
                    "toadstool" +//-name
                    "-06", //cost
                    "ToadStool", //-display name
                    1, //-attack
                    2, //-health
                    MetaC_rare, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "A frogish fungus, how curious", //-description
                    appearanceBehaviour: rareAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //sunbean
            {
                string imageName = "SunBean"; //-image info

                List<Ability> abilities = new List<Ability> { SunKill.ability, Spudow.ability }; //-sigil info
                List<Trait> traits = new List<Trait> { Trait.Terrain }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSun.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "sunbean" +//-name
                    "-02", //cost
                    "Sun bean", //-display name
                    0, //-attack
                    2, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "will you invest in the sun bean?", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }



            //----defend----
            //wallnut -evo
            {
                string imageName = "Wallnut"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.WhackAMole}; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsDefend.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = "nut2-04"; //-evo info
                string tailName = "splinters"; //-tail info

                NewCard.Add(
                    prefix +
                    "nut1" +//-name
                    "-02", //cost
                    "Wallnut", //-display name
                    0, //-attack
                    4, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "What an iconic line of defense", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //tallnut 
            {
                string imageName = "Tallnut"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.WhackAMole, Ability.Reach }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsDefend.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "splinters"; //-tail info

                NewCard.Add(
                    prefix +
                    "nut2" +//-name
                    "-04", //cost
                    "Tallnut", //-display name
                    0, //-attack
                    8, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Near nothing can get past this one", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    ); 
            }

            //endurian 
            {
                string imageName = "Endurian"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Sharp }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsDefend.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "splinters"; //-tail info

                NewCard.Add(
                    prefix +
                    "endurian" +//-name
                    "-04", //cost
                    "Endurian", //-display name
                    0, //-attack
                    4, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "It's outer shell wont be so easy to get through", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //garlic 
            {
                string imageName = "Garlic"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.DebuffEnemy }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsDefend.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "garlic" +//-name
                    "-02", //cost
                    "Garlic", //-display name
                    0, //-attack
                    2, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Control the upcoming onslaughts with the Garlic's stench.", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //infininut -ice *
            {
                string imageName = "Infininut"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.IceCube}; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsDefend.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility, InfoStoreIce.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "infininut1" +//-name
                    "-04", //cost
                    "Infininut", //-display name
                    0, //-attack
                    2, //-health
                    MetaC_rare, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Defeating this one isn't so simple", //-description
                    appearanceBehaviour: rareAppearance, // -appearance

                    iceCubeId: findIce("infininut2-04"), //ice

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //infininut base -evo
            {
                string imageName = "InfininutBase"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Evolve }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsDefend.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = "infininut1-04"; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "infininut2" +//-name
                    "-04", //cost
                    "Infininut", //-display name
                    0, //-attack
                    1, //-health
                    MetaC_null, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Defeating this one isn't so simple", //-description
                    appearanceBehaviour: rareAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }



            //----bomb----
            //potato mine
            {
                string imageName = "Potato"; //-image info

                List<Ability> abilities = new List<Ability> { Spudow.ability }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsBomb.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "potato" +//-name
                    "-01", //cost
                    "Potato Mine", //-display name
                    0, //-attack
                    1, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "A small but potentially dangerous spud", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //cherry
            {
                string imageName = "Cherry"; //-image info

                List<Ability> abilities = new List<Ability> { Sweet.ability }; //-sigil info
                List<Trait> traits = new List<Trait> { NoSacs }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsBomb.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "cherry" +//-name
                    "-05", //cost
                    "Cherry Bomb", //-display name
                    0, //-attack
                    0, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "It may be sweet but you shouldnt underestimate it", //-description
                    appearanceBehaviour: instantAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //jalapeno
            {
                string imageName = "Jalapeno"; //-image info

                List<Ability> abilities = new List<Ability> { Spicy.ability }; //-sigil info
                List<Trait> traits = new List<Trait> { NoSacs }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsBomb.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info


                NewCard.Add(
                    prefix +
                    "jalapeno" +//-name
                    "-05", //cost
                    "Jalapeno", //-display name
                    0, //-attack
                    0, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "I would not want to eat that", //-description
                    appearanceBehaviour: instantAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //Doomshroom 
            {
                string imageName = "Doom"; //-image info

                List<Ability> abilities = new List<Ability> { DoomBringer.ability }; //-sigil info
                List<Trait> traits = new List<Trait> { Mushroom, NoSacs }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsBomb.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "shroomchunks"; //-tail info

                NewCard.Add(
                    prefix +
                    "doom2" +//-name
                    "-04", //cost
                    "DoomShroom", //-display name
                    0, //-attack
                    2, //-health
                    MetaC_rare, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "A force to be reckoned with", //-description
                    appearanceBehaviour: instantAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }


            //----support----

            //torchwood
            {
                string imageName = "Torch"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Sharp, Ability.BuffNeighbours }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSupport.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "splinters"; //-tail info

                NewCard.Add(
                    prefix +
                    "torch" +//-name
                    "-05", //cost
                    "Torchwood", //-display name
                    0, //-attack
                    2, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "It can boost it's allies' power", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //magnet
            {
                string imageName = "Magnet"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.RandomConsumable, ArmorBreak.ability }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSupport.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "shroomchunks"; //-tail info

                NewCard.Add(
                    prefix +
                    "magnet" +//-name
                    "-03", //cost
                    "MagnetShroom", //-display name
                    1, //-attack
                    1, //-health
                    MetaC_null, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Weakens the enemy to aid its allies", //-description
                    appearanceBehaviour: rareAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //gold leaf *description
            {
                string imageName = "GoldLeaf"; //-image info

                List<Ability> abilities = new List<Ability> { AddStats.ability, Sunmaker1.ability}; //-sigil info
                List<Trait> traits = new List<Trait> { NoSacs }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSupport.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "goldleaf" +//-name
                    "-03", //cost
                    "Gold leaf", //-display name
                    0, //-attack
                    0, //-health
                    MetaC_choice, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "...", //-description
                    appearanceBehaviour: goldAppearance,

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //pumpkin
            {
                string imageName = "Pumpkin"; //-image info

                List<Ability> abilities = new List<Ability> { AddStats.ability, Ability.DeathShield, }; //-sigil info
                List<Trait> traits = new List<Trait> { NoSacs }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSupport.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                
                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "pumpkin" +//-name
                    "-04", //cost
                    "Pumpkin", //-display name
                    0, //-attack
                    1, //-health
                    MetaC_choice, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Fortify all of your cards with this one simple trick", //-description
                    appearanceBehaviour: instantAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //escape root 
            {
                string imageName = "Root"; //-image info

                List<Ability> abilities = new List<Ability> { AddStats.ability, Ability.TailOnHit }; //-sigil info
                List<Trait> traits = new List<Trait> { NoSacs }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSupport.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "root" +//-name
                    "-03", //cost
                    "Escape Root", //-display name
                    0, //-attack
                    0, //-health
                    MetaC_choice, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Allows other cards the freedom of escaping to safety", //-description
                    appearanceBehaviour: instantAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //coffee bean 
            {
                string imageName = "Coffee"; //-image info

                List<Ability> abilities = new List<Ability> { AddStats.ability, Ability.Evolve }; //-sigil info
                List<Trait> traits = new List<Trait> { NoSacs }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsSupport.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "coffee" +//-name
                    "-04", //cost
                    "Coffee bean", //-display name
                    0, //-attack
                    0, //-health
                    MetaC_choice, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "It can energise plants and unlock further potential", //-description
                    appearanceBehaviour: instantAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }


            //----special----
            //sprout
            {
                string imageName = "Sprout"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Evolve }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { PlantsBomb.tribe, PlantsAttack.tribe, PlantsSun.tribe, PlantsDefend.tribe, PlantsSupport.tribe }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { SunCost.SpecialTriggeredAbility, EvolveSprout.SpecialTriggeredAbility }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "leaflitter"; //-tail info

                NewCard.Add(
                    prefix +
                    "sprout" +//-name
                    "-01", //cost
                    "Sprout", //-display name
                    0, //-attack
                    1, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "But a simple sprout, what will it grow into? nobody knows, Limitless potential", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }



            //----zombies----

            //Zombie 
            {
                string imageName = "Zombie"; //-image info

                List<Ability> abilities = new List<Ability> {  }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> {  }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> {  }; //-sp ability info

                string evoName = "zombie_cone"; //-evo info
                string tailName = "zombiearm"; //-tail info

                NewCard.Add(
                    prefix +
                    "zombie",//-name
                    "Zombie", //-display name
                    1, //-attack
                    2, //-health
                    MetaC_standard, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "A common browncoat, hungry for brains", //-description
                    appearanceBehaviour: null, //-appearance
                    bonesCost: 3, //-cost

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    ) ;
            }

            //Conehead *sigil *locked
            {
                string imageName = "ZombieCone"; //-image info

                List<Ability> abilities = new List<Ability> {HardHeaded.ability }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "zombiearm"; //-tail info

                NewCard.Add(
                    prefix +
                    "zombie_cone",//-name
                    "Conehead", //-display name
                    1, //-attack
                    2, //-health
                    MetaC_null, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "A browncoat that equipped itself with road work equipment", //-description
                    appearanceBehaviour: null, //-appearance
                    bonesCost: 4, //-cost

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //Flag Zombie 
            {
                string imageName = "ZombieFlag"; //-image info

                List<Ability> abilities = new List<Ability> {ZombieWave.ability }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { }; //-sp ability info

                string evoName = null; //-evo info
                string tailName = "zombiearm"; //-tail info

                NewCard.Add(
                    prefix +
                    "zombie_flag",//-name
                    "Flag Zombie", //-display name
                    1, //-attack
                    2, //-health
                    MetaC_rare, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "A true leader, calling in allies for combat", //-description
                    appearanceBehaviour: rareAppearance, //-appearance
                    bonesCost: 5, //-cost

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    tailId: findTail(tailName),
                    iceCubeId: findIce(tailName),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }



            //----tribeless----

            //Crater
            {
                string imageName = "Crater"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Submerge }; //-sigil info
                List<Trait> traits = new List<Trait> { Trait.Terrain }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { }; //-sp ability info

                string evoName = null; //-evo info

                NewCard.Add(
                    prefix +
                    "crater", //-name
                    "Crater", //-display name
                    0, //-attack
                    2, //-health
                    MetaC_null, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Just a hole", //-description
                    appearanceBehaviour: terrainAppearance, // -appearance

                    defaultTex: findTexture(imageName),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //Leaflitter
            {
                string imageName = "LeafLitter"; //-image info

                List<Ability> abilities = new List<Ability> { }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { }; //-sp ability info

                string evoName = null; //-evo info

                NewCard.Add(
                    prefix +
                    "leaflitter", //-name
                    "Leaf litter", //-display name
                    0, //-attack
                    2, //-health
                    MetaC_null, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "The cycle of the forest", //-description
                    appearanceBehaviour: null, // -appearance


                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    iceCubeId: findIce("leaflitter"),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //shroomchunks -evo
            {
                string imageName = "ShroomChunks"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Evolve }; //-sigil info
                List<Trait> traits = new List<Trait> { Mushroom }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { }; //-sp ability info

                string evoName = "puff-00"; //-evo info

                NewCard.Add(
                    prefix +
                    "shroomchunks", //-name
                    "Shroom Chunks", //-display name
                    0, //-attack
                    2, //-health
                    MetaC_null, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Chunks of shrooms", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    iceCubeId: findIce("shroomchunks"),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //splinters 
            {
                string imageName = "Splinters"; //-image info

                List<Ability> abilities = new List<Ability> { Ability.Sharp }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { }; //-sp ability info

                string evoName = null; //-evo info

                NewCard.Add(
                    prefix +
                    "splinters", //-name
                    "Splinters", //-display name
                    0, //-attack
                    1, //-health
                    MetaC_null, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "Shards of shells", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    iceCubeId: findIce("splinters"),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //zombiearm 
            {
                string imageName = "ZombieArm"; //-image info

                List<Ability> abilities = new List<Ability> {  }; //-sigil info
                List<Trait> traits = new List<Trait> { }; //trait info
                List<Texture> decals = new List<Texture> { texDecal }; //-decal info
                List<Tribe> tribes = new List<Tribe> { }; //-tribe info
                List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { }; //-sp ability info

                string evoName = null; //-evo info

                NewCard.Add(
                    prefix +
                    "zombiearm", //-name
                    "Zombie Arm", //-display name
                    0, //-attack
                    2, //-health
                    MetaC_null, //-metacategories
                    CardComplexity.Simple,
                    CardTemple.Nature,
                    description: "A zombie must have dropped this", //-description
                    appearanceBehaviour: null, // -appearance

                    defaultTex: findTexture(imageName),
                    emissionTex: findTexture(imageName + "2"),
                    decals: decals,
                    abilities: abilities,
                    traits: traits,
                    evolveId: findEvolution(evoName),
                    iceCubeId: findIce("zombiearm"),
                    tribes: tribes,
                    specialAbilities: (specialTriggeredAbilities)
                    );
            }

            //----other----
            //SunCounter card
            {
                SunCounterCardInfo.name = "pvz_zep_sunCounter";
                SunCounterCardInfo.displayedName = "Sun:";
                SunCounterCardInfo.baseAttack = 0;
                SunCounterCardInfo.baseHealth = 0;
                SunCounterCardInfo.hideAttackAndHealth = true;
                SunCounterCardInfo.appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance> { SunCounterBackground.CustomAppearance };

            }

        }



        //==========make sigils===============================================

        private void AddSigils()
        {

            AddTacticalRetreat();
            AddPierce();
            AddSporeSpread();

            AddSunmaker1();
            AddSunmaker2();
            AddSunmaker3();
            AddSunKill();

            AddSpudow();
            AddSpicy();
            AddSweet();
            AddDoomBringer();

            AddAddStats();
            AddArmorBreak();


            AddZombieWave();
            AddHardHeaded();


            AddInfoStoreIce();
            AddEvolveSprout();


        }

        //sunmaker1
        private NewAbility AddSunmaker1()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "SunMaker 1";
            info.rulebookDescription = "At the end of the owner's turn, a card bearing this sigil will produce 1 sun, if it belongs to the player.";
            info.flipYIfOpponent = false;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "Every little bit counts";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("Sunmaker1");

            NewAbility ability = new NewAbility(info, typeof(Sunmaker1), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            Sunmaker1.ability = ability.ability;
            return ability;
        }
        public class Sunmaker1 : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;


            public override bool RespondsToTurnEnd(bool playerTurnEnd) //checks for turn end
            {
                return base.Card.OpponentCard != playerTurnEnd && !this.Card.Dead;
            }

            public override IEnumerator OnTurnEnd(bool playerTurnEnd)
            {
                if (!this.Card.OpponentCard)
                {

                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                    yield return base.PreSuccessfulTriggerSequence();

                    yield return AddSunToCounter(1); //adds sun

                    yield return base.LearnAbility(0.1f);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    yield break;
                }
            }

            public IEnumerator AddSunToCounter(int count) //code to add sun 1 by 1
            {
                for (int i = 0; i < count; i++)
                {
                    base.Card.Anim.LightNegationEffect();

                    Plugin.Sun++; //removes cost from Sun
                    Plugin.UpdateSunCounter();
                    Plugin.SunCounterCard.Anim.LightNegationEffect();
                    yield return new WaitForSeconds(0.3f);

                }
            }

        }

        //sunmaker2
        private NewAbility AddSunmaker2()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "SunMaker 2";
            info.rulebookDescription = "At the end of the owner's turn, a card bearing this sigil will produce 2 sun, if it belongs to the player.";
            info.flipYIfOpponent = false;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "If only the moon was this useful";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("Sunmaker2");

            NewAbility ability = new NewAbility(info, typeof(Sunmaker2), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            Sunmaker2.ability = ability.ability;
            return ability;
        }
        public class Sunmaker2 : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;


            public override bool RespondsToTurnEnd(bool playerTurnEnd) //checks for turn end
            {
                return base.Card.OpponentCard != playerTurnEnd && !this.Card.Dead;
            }

            public override IEnumerator OnTurnEnd(bool playerTurnEnd)
            {
                if (!this.Card.OpponentCard)
                {

                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                    yield return base.PreSuccessfulTriggerSequence();

                    yield return AddSunToCounter(2); //adds sun

                    yield return base.LearnAbility(0.1f);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    yield break;
                }
            }


            public IEnumerator AddSunToCounter(int count) //code to add sun 1 by 1
            {
                for (int i = 0; i < count; i++)
                {
                    base.Card.Anim.LightNegationEffect();
                    Plugin.Sun++; //removes cost from Sun
                    Plugin.UpdateSunCounter();
                    Plugin.SunCounterCard.Anim.LightNegationEffect();
                    yield return new WaitForSeconds(0.3f);

                }
            }

        }

        //sunmaker2
        private NewAbility AddSunmaker3()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "SunMaker 3";
            info.rulebookDescription = "At the end of the owner's turn, a card bearing this sigil will produce 3 sun, if it belongs to the player.";
            info.flipYIfOpponent = false;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook};

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "The power of the sun, in the palm of your hand";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("Sunmaker3");

            NewAbility ability = new NewAbility(info, typeof(Sunmaker3), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            Sunmaker3.ability = ability.ability;
            return ability;
        }
        public class Sunmaker3 : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;


            public override bool RespondsToTurnEnd(bool playerTurnEnd) //checks for turn end
            {
                return base.Card.OpponentCard != playerTurnEnd && !this.Card.Dead;
            }

            public override IEnumerator OnTurnEnd(bool playerTurnEnd)
            {
                if (!this.Card.OpponentCard)
                {

                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                    yield return base.PreSuccessfulTriggerSequence();

                    yield return AddSunToCounter(3); //adds sun

                    yield return base.LearnAbility(0.1f);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    yield break;
                }
            }

            public IEnumerator AddSunToCounter(int count) //code to add sun 1 by 1
            {
                for (int i = 0; i < count; i++)
                {
                    base.Card.Anim.LightNegationEffect();
                    Plugin.Sun++; //removes cost from Sun
                    Plugin.UpdateSunCounter();
                    Plugin.SunCounterCard.Anim.LightNegationEffect();
                    yield return new WaitForSeconds(0.3f);

                }
            }

        }


        //sunmkill
        private NewAbility AddSunKill()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "SunKill";
            info.rulebookDescription = "When a card bearing this sigil defeats another card, the other card will drop 3 sun.";
            info.flipYIfOpponent = false;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "Recycle your enemies back into nutrients";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("Sunkill");

            NewAbility ability = new NewAbility(info, typeof(SunKill), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            SunKill.ability = ability.ability;
            return ability;
        }
        public class SunKill : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;


            public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
            {
                return killer != null && killer == this.Card;
            }

            public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return base.PreSuccessfulTriggerSequence();

                yield return AddSunToCounter(3); //adds sun

                yield return base.LearnAbility(0.1f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                yield break;
            }





            public IEnumerator AddSunToCounter(int count) //code to add sun 1 by 1
            {
                for (int i = 0; i < count; i++)
                {
                    base.Card.Anim.LightNegationEffect();
                    Plugin.Sun++; //removes cost from Sun
                    Plugin.UpdateSunCounter();
                    Plugin.SunCounterCard.Anim.LightNegationEffect();
                    yield return new WaitForSeconds(0.3f);

                }
            }

        }

        //spudow
        private NewAbility AddSpudow()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Spudow";
            info.rulebookDescription = "When a card bearing this sigil gets defeated, it deals 4 damage to the opposing card.";
            info.flipYIfOpponent = true;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "Always watch your step in a potato field";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("Spudow");

            NewAbility ability = new NewAbility(info, typeof(Spudow), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            Spudow.ability = ability.ability;
            return ability;
        }
        public class Spudow : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;

            public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
            {
                return base.Card.OnBoard;
            }

            public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
            {
                base.Card.Anim.LightNegationEffect();
                yield return base.PreSuccessfulTriggerSequence();
                if (!wasSacrifice)
                {
                    yield return this.ExplodeFromSlot(base.Card.Slot);
                }
                yield break;
            }

            protected IEnumerator ExplodeFromSlot(CardSlot slot)
            {

                if (slot.opposingSlot.Card != null && !slot.opposingSlot.Card.Dead)
                {
                    yield return this.BombCard(slot.opposingSlot.Card, slot.Card);
                }

                //yield return base.LearnAbility(0.25f);
                yield break;
            }

            private IEnumerator BombCard(PlayableCard target, PlayableCard attacker)
            {
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.StrongNegationEffect();

                yield return target.TakeDamage(4, attacker);
                yield break;
            }

            //learn when die
            public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
            {
                return !wasSacrifice && base.Card.OnBoard;
            }

            public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.LearnAbility(0.5f);
                yield break;
            }
        }

        //sweet
        private NewAbility AddSweet()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Sweet";
            info.rulebookDescription = "At the end of the owner's turn, any card bearing this sigil will explode and deal 3 damage to the cards in the 3 enemy slots around it, then deal 1 damage to the cards in your adjacent slots. Cards bearing this sigil cannot be used in sigil transfers or campfires.";
            info.flipYIfOpponent = true;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "Maybe there's a bit more tartness than anticipated";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("Sweet");

            NewAbility ability = new NewAbility(info, typeof(Sweet), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            Sweet.ability = ability.ability;
            return ability;
        }
        public class Sweet : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;

            public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
            {
                return base.Card.OnBoard;
            }

            public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
            {
                base.Card.Anim.LightNegationEffect();
                yield return base.PreSuccessfulTriggerSequence();
                if (!wasSacrifice)
                {
                    yield return this.ExplodeFromSlot(base.Card.Slot);
                }
                //yield return base.LearnAbility(0.5f);
                yield break;
            }

            protected IEnumerator ExplodeFromSlot(CardSlot slot)
            {
                List<CardSlot> adjacentSlots = Singleton<BoardManager>.Instance.GetAdjacentSlots(slot);



                if (adjacentSlots.Count > 0 && adjacentSlots[0].Index < slot.Index)
                {
                    if (adjacentSlots[0].Card != null && !adjacentSlots[0].Card.Dead) //check for left team
                    {
                        yield return this.BombCard(adjacentSlots[0].Card, slot.Card, 1); //explode it
                    }

                    if (adjacentSlots[0].opposingSlot.Card != null && !adjacentSlots[0].opposingSlot.Card.Dead) //check for left enemy
                    {
                        yield return this.BombCard(adjacentSlots[0].opposingSlot.Card, slot.Card, 3); //explode it
                    }


                    adjacentSlots.RemoveAt(0); //remove from adjacents
                }

                if (slot.opposingSlot.Card != null && !slot.opposingSlot.Card.Dead) //check for frontline enemy
                {
                    yield return this.BombCard(slot.opposingSlot.Card, slot.Card, 3); //explode it
                }

                if (adjacentSlots.Count > 0 && adjacentSlots[0].opposingSlot.Card != null && !adjacentSlots[0].opposingSlot.Card.Dead) //check for right enemy
                {
                    yield return this.BombCard(adjacentSlots[0].opposingSlot.Card, slot.Card, 3); //explode it
                }

                if (adjacentSlots.Count > 0 && adjacentSlots[0].Card != null && !adjacentSlots[0].Card.Dead) //check for right team
                {
                    yield return this.BombCard(adjacentSlots[0].Card, slot.Card, 1); //explode it
                }


                yield break;
            }

            private IEnumerator BombCard(PlayableCard target, PlayableCard attacker, int damage) //this is the code that does the explodey stuff
            {
                yield return new WaitForSeconds(0.1f);
                target.Anim.PlayHitAnimation();
                yield return target.TakeDamage(damage, attacker);
                yield break;
            }

            //instability
            public override bool RespondsToTurnEnd(bool playerTurnEnd)
            {
                return base.Card.OpponentCard != playerTurnEnd;
            }

            public override IEnumerator OnTurnEnd(bool playerTurnEnd)
            {
                yield return base.PreSuccessfulTriggerSequence();

                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.StrongNegationEffect();

                if (base.Card != null && !base.Card.Dead)
                {

                    if (!SaveManager.SaveFile.IsPart2)
                    {
                        Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                        yield return new WaitForSeconds(0.1f);
                    }
                    yield return base.Card.Die(false, null, true);
                    //yield return base.LearnAbility(0.25f);
                    if (!SaveManager.SaveFile.IsPart2)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                yield break;
            }
            //learn when die
            public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
            {
                return !wasSacrifice && base.Card.OnBoard;
            }

            public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.LearnAbility(0.5f);
                yield break;
            }
        }

        //spicy
        private NewAbility AddSpicy()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Spicy";
            info.rulebookDescription = "At the end of the owner's turn, any card bearing this sigil will explode and deal 4 damage to not only the opposing card but also to the card on the opposing backline. Cards bearing this sigil cannot be used in sigil transfers or campfires.";
            info.flipYIfOpponent = true;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook};

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "I've never been too fond of spiciness";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("Spicy");

            NewAbility ability = new NewAbility(info, typeof(Spicy), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            Spicy.ability = ability.ability;
            return ability;
        }
        public class Spicy : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;

            public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
            {
                return base.Card.OnBoard;
            }

            public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
            {
                base.Card.Anim.LightNegationEffect();
                yield return base.PreSuccessfulTriggerSequence();
                if (!wasSacrifice)
                {
                    yield return this.ExplodeFromSlot(base.Card.Slot);
                }
                //yield return base.LearnAbility(0.5f);
                yield break;
            }

            protected IEnumerator ExplodeFromSlot(CardSlot slot)
            {

                if (slot.opposingSlot.Card != null && !slot.opposingSlot.Card.Dead) //check for frontline enemy
                {
                    yield return this.BombCard(slot.opposingSlot.Card, slot.Card); //explode it
                }

                if (BoardManager.Instance.GetCardQueuedForSlot(slot.opposingSlot) != null) //check for backline enemy
                {
                    yield return this.BombCard(BoardManager.Instance.GetCardQueuedForSlot(slot.opposingSlot), slot.Card); //explode it
                }

                
                yield break;
            }

            private IEnumerator BombCard(PlayableCard target, PlayableCard attacker) //this is the code that does the explodey stuff
            {
                yield return new WaitForSeconds(0.1f);
                target.Anim.PlayHitAnimation();
                yield return target.TakeDamage(4, attacker);
                yield break;
            }

            //instability
            public override bool RespondsToTurnEnd(bool playerTurnEnd)
            {
                return base.Card.OpponentCard != playerTurnEnd;
            }

            public override IEnumerator OnTurnEnd(bool playerTurnEnd)
            {
                yield return base.PreSuccessfulTriggerSequence();

                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.StrongNegationEffect();

                if (base.Card != null && !base.Card.Dead)
                {

                    if (!SaveManager.SaveFile.IsPart2)
                    {
                        Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                        yield return new WaitForSeconds(0.1f);
                    }
                    yield return base.Card.Die(false, null, true);
                    //yield return base.LearnAbility(0.25f);
                    if (!SaveManager.SaveFile.IsPart2)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                yield break;
            }
            //learn when die
            public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
            {
                return !wasSacrifice && base.Card.OnBoard;
            }

            public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.LearnAbility(0.5f);
                yield break;
            }
        }


        //Doombringer
        private NewAbility AddDoomBringer()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "DoomBringer";
            info.rulebookDescription = "At the end of the owner's turn, any card bearing this sigil will explode and deal 7 damage to all of the opponent's cards, along with 1 damage to all of the owners cards, and then leave a crater where it stood.";
            info.flipYIfOpponent = true;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "Only destruction lays in its wake";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("Doombringer");

            NewAbility ability = new NewAbility(info, typeof(DoomBringer), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            DoomBringer.ability = ability.ability;
            return ability;
        }
        public class DoomBringer : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;

            public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
            {
                return base.Card.OnBoard;
            }

            public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
            {
                base.Card.Anim.LightNegationEffect();
                yield return base.PreSuccessfulTriggerSequence();
                if (!wasSacrifice)
                {
                    yield return this.ExplodeFromSlot(base.Card.Slot);
                }
                yield break;
            }

            protected IEnumerator ExplodeFromSlot(CardSlot slot)
            {

                foreach (CardSlot cardSlot in BoardManager.Instance.OpponentSlotsCopy) //go through each enemy card
                {
                    if (cardSlot.Card != null && !cardSlot.Card.Dead) //check for enemy card
                    {
                        yield return this.BombCard(cardSlot.Card, slot.Card, 7); //explode it
                    }

                    if (cardSlot.opposingSlot.Card != null && !cardSlot.opposingSlot.Card.Dead) //check for player card
                    {
                        yield return this.BombCard(cardSlot.opposingSlot.Card, slot.Card, 1); //explode it
                    }
                }

                yield break;
            }

            private IEnumerator BombCard(PlayableCard target, PlayableCard attacker, int damage) //this is the code that does the explodey stuff
            {
                yield return new WaitForSeconds(0.003f);
                target.Anim.PlayHitAnimation();
                yield return target.TakeDamage(damage, attacker);
                yield break;
            }

            //instability
            public override bool RespondsToTurnEnd(bool playerTurnEnd)
            {
                return base.Card.OpponentCard != playerTurnEnd;
            }

            public override IEnumerator OnTurnEnd(bool playerTurnEnd)
            {
                yield return base.PreSuccessfulTriggerSequence();


                base.Card.Anim.LightNegationEffect();
                yield return new WaitForSeconds(0.3f);
                base.Card.Anim.LightNegationEffect();
                yield return new WaitForSeconds(0.2f);

                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.05f);
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.02f);
                base.Card.Anim.StrongNegationEffect();

                if (base.Card != null && !base.Card.Dead)
                {

                    if (!SaveManager.SaveFile.IsPart2)
                    {
                        Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                        yield return new WaitForSeconds(0.1f);
                    }
                    yield return base.Card.Die(false, null, true);
                    //yield return base.LearnAbility(0.25f);
                    if (!SaveManager.SaveFile.IsPart2)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                yield break;
            }

            //crater
            public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
            {
                return !wasSacrifice && base.Card.OnBoard;
            }

            public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return new WaitForSeconds(0.3f);
                string name = "pvz_zep_crater";

                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName(name), base.Card.Slot, 0.15f, true);
                yield return base.LearnAbility(0.5f);
                yield break;
            }



        }

        //Tactical retreat
        private NewAbility AddTacticalRetreat()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Scaredy";
            info.rulebookDescription = "A card bearing this sigils loses 1 attack but gains 1 health when against an opposing card whose attack is greater than 0.";
            info.flipYIfOpponent = false;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "They call it a tactical retreat.";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("TacticalRetreat");

            NewAbility ability = new NewAbility(info, typeof(TacticalRetreat), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            TacticalRetreat.ability = ability.ability;
            return ability;
        }
        public class TacticalRetreat : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;



        }

        [HarmonyPatch(typeof(PlayableCard), "GetPassiveAttackBuffs")]
        public class ScaredyAttack_patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref int __result, ref PlayableCard __instance)
            {
                if (__instance.OnBoard)
                {
                    if (__instance.slot.Card != null && __instance.slot.Card.Info.HasAbility(TacticalRetreat.ability) && __instance.slot.opposingSlot.Card != null)
                    {

                        //Plugin.Log.LogInfo("CheckedAttack1");
                        if (__instance.slot.opposingSlot.Card.Attack >= 1)
                        {
                            //Plugin.Log.LogInfo("AttackLowered");
                            __result -= 1;
                        }
                    }

                }
            }
        }

        [HarmonyPatch(typeof(PlayableCard), "GetPassiveHealthBuffs")]
        public class ScaredHealth_patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref int __result, ref PlayableCard __instance)
            {
                if (__instance.OnBoard)
                {
                    if (__instance.slot.Card != null && __instance.slot.Card.Info.HasAbility(TacticalRetreat.ability) && __instance.slot.opposingSlot.Card != null)
                    {

                        //Plugin.Log.LogInfo("CheckedAttack1");
                        if (__instance.slot.opposingSlot.Card.Attack >= 1)
                        {
                            //Plugin.Log.LogInfo("AttackLowered");
                            __result += 1;
                        }
                    }

                }
            }
        }


        //pierce
        private NewAbility AddPierce()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Pierce";
            info.rulebookDescription = "After attacking, a card bearing this sigil will also attack the card in the opposing queue, if there is one.";
            info.flipYIfOpponent = true;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook};

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "Hiding in the back doesn't protect you here";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("Pierce");

            NewAbility ability = new NewAbility(info, typeof(Pierce), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            Pierce.ability = ability.ability;
            return ability;
        }
        public class Pierce : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;

        }

        [HarmonyPatch(typeof(CombatPhaseManager), "SlotAttackSlot")]
        public class Pierce_patch
        {
            [HarmonyPostfix]
            public static IEnumerator Postfix(IEnumerator enumerator, CardSlot attackingSlot, CardSlot opposingSlot, float waitAfter = 0f)
            {
                yield return enumerator;

                if (attackingSlot.Card != null && !attackingSlot.Card.Dead && attackingSlot.IsPlayerSlot && attackingSlot.Card.Info.HasAbility(Pierce.ability))
                {
                    PlayableCard queuedCard = Singleton<BoardManager>.Instance.GetCardQueuedForSlot(opposingSlot);

                    if (queuedCard != null && attackingSlot.Card.Attack > 0 && opposingSlot != Singleton<BoardManager>.Instance.GetCardQueuedForSlot(opposingSlot).slot)
                    {
                        if (attackingSlot.Card != null && queuedCard != null && !queuedCard.Dead)
                        {
                            yield return new WaitForSeconds(0.1f);
                            Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.QueueView, false, false);
                            yield return new WaitForSeconds(0.3f);

                            if (queuedCard.HasAbility(Ability.PreventAttack))
                            {
                                attackingSlot.Card.Anim.LightNegationEffect();
                            }
                            else
                            {
                                yield return queuedCard.TakeDamage(attackingSlot.Card.Attack, attackingSlot.Card);

                                
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CombatPhaseManager), "DealOverkillDamage")]
        public class PierceOverkill_patch
        {
            [HarmonyPrefix]
            public static void CheckforPierce(ref int damage, CardSlot attackingSlot, CardSlot opposingSlot)
            {
                if (attackingSlot.Card != null && attackingSlot.IsPlayerSlot && attackingSlot.Card.Info.HasAbility(Pierce.ability))
                {
                    damage = 0;
                }
            }
        }


        //sporespread
        private NewAbility AddSporeSpread()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Spore Spreader";
            info.rulebookDescription = "If a card bearing this sigil defeats another card, a copy of itself will be returned to your hand.";
            info.flipYIfOpponent = true;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "just how much will these spores spread?";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("SporeSpread");

            NewAbility ability = new NewAbility(info, typeof(SporeSpread), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            SporeSpread.ability = ability.ability;
            return ability;
        }
        public class SporeSpread : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;

            public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
            {
                Plugin.Log.LogInfo("playable card is " + card);//note

                Plugin.Log.LogInfo("this card is "+ this.Card);//note

                Plugin.Log.LogInfo("killer is " + killer);//note

                return this.Card == killer ;
            }

            public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
            {

                CardInfo CardToDraw = base.Card.Info;

                if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
                {
                    yield return new WaitForSeconds(0.2f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                    yield return new WaitForSeconds(0.2f);
                }

                yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardToDraw, null, 0.25f, null);
                yield return new WaitForSeconds(0.45f);
                yield return base.LearnAbility(0.1f);
                yield break;
            }



        }


        //addstats 
        private NewAbility AddAddStats()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Add Stats";
            info.rulebookDescription = "When a card bearing this sigil is played, its health, attack, and other sigils will be added to every other non terrain card on your side of the board. Then this card will perish. Cards bearing this sigil cannot be used in sigil transfers or campfires.";
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "The power of teamwork";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("AddStats");

            NewAbility ability = new NewAbility(info, typeof(AddStats), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            AddStats.ability = ability.ability;
            return ability;
        }

        public class AddStats : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;


            public override bool RespondsToResolveOnBoard()
            {
                return base.Card.OnBoard;
            }

            public override IEnumerator OnResolveOnBoard()
            {
                base.Card.Anim.LightNegationEffect();
                yield return base.PreSuccessfulTriggerSequence();


                foreach (CardSlot cardSlot in BoardManager.Instance.PlayerSlotsCopy) //go through each player card
                {

                    if (cardSlot.Card != null && !cardSlot.Card.Dead && Card && cardSlot.Card != this.Card && !cardSlot.Card.Info.traits.Contains(Trait.Terrain) && !cardSlot.Card.Info.traits.Contains(NoSacs)) //check for player card
                    {


                        this.Card.Anim.LightNegationEffect();

                        yield return new WaitForSeconds(0.2f);
                        this.Card.Anim.StrongNegationEffect();
                        yield return new WaitForSeconds(0.2f);
                        this.Card.Anim.StrongNegationEffect();

                        yield return new WaitForSeconds(0.3f);
                        cardSlot.Card.Anim.PlayTransformAnimation();


                        CardInfo targetCardInfo = cardSlot.Card.Info.Clone() as CardInfo;//copy card info

                        foreach ( CardModificationInfo cardmod in cardSlot.Card.Info.Mods)
                        {
                            targetCardInfo.Mods.Add(cardmod);
                        }

                        foreach (Ability ability in this.Card.Info.abilities)
                        {
                            if (ability != AddStats.ability)
                            {
                                CardModificationInfo cardModificationInfoPvZ = new CardModificationInfo(ability); //copy ability
                                targetCardInfo.Mods.Add(cardModificationInfoPvZ); //add ability 
                            }
                        }

                        CardModificationInfo cardModificationInfoPvZ2 = new CardModificationInfo(); //make base for stat mods
                        cardModificationInfoPvZ2.attackAdjustment = this.Card.Info.baseAttack;
                        cardModificationInfoPvZ2.healthAdjustment = this.Card.Info.baseHealth;
                        targetCardInfo.Mods.Add(cardModificationInfoPvZ2); //add statmods

                        cardSlot.Card.SetInfo(targetCardInfo); //implement all the new mods

                        cardSlot.Card.RenderCard();

                        yield return new WaitForSeconds(0.3f);

                    }
                }
                yield return new WaitForSeconds(0.2f);
                yield return base.Card.Die(false, null, true);


                yield return new WaitForSeconds(0.2f);
                yield return base.LearnAbility(0.5f);

                Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.DefaultView, false, false);

                yield break;

            }

        }


        //armorbreak 
        private NewAbility AddArmorBreak()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Armor breaker";
            info.rulebookDescription = "When a card bearing this sigil is played, it removes all defense sigils from all of the opposing cards.";
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook  };

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "Removing defense is a great offense";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("ArmorBreak");

            NewAbility ability = new NewAbility(info, typeof(ArmorBreak), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            ArmorBreak.ability = ability.ability;
            return ability;
        }

        public class ArmorBreak : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;


            public override bool RespondsToResolveOnBoard()
            {
                return base.Card.OnBoard;
            }

            public override IEnumerator OnResolveOnBoard()
            {
                base.Card.Anim.LightNegationEffect();
                yield return base.PreSuccessfulTriggerSequence();

                Ability[] armorAbilities = { HardHeaded.ability, Ability.DeathShield, Ability.ShieldGems, Ability.Reach, Ability.WhackAMole, Ability.GuardDog };

                foreach (CardSlot cardSlot in BoardManager.Instance.OpponentSlotsCopy) //go through each player card
                {
                    if (cardSlot.Card != null && !cardSlot.Card.Dead  ) //check for enemy card
                    {
                        foreach (Ability ability1 in armorAbilities)
                        {
                            if (cardSlot.Card.Info.HasAbility(ability1))
                            {
                                this.Card.Anim.LightNegationEffect();

                                yield return new WaitForSeconds(0.2f);
                                this.Card.Anim.StrongNegationEffect();
                                yield return new WaitForSeconds(0.2f);
                                this.Card.Anim.StrongNegationEffect();

                                yield return new WaitForSeconds(0.3f);
                                cardSlot.Card.Anim.PlayTransformAnimation();


                                CardInfo targetCardInfo = cardSlot.Card.Info.Clone() as CardInfo;//copy card info

                                foreach (CardModificationInfo cardmod in cardSlot.Card.Info.Mods)
                                {
                                    targetCardInfo.Mods.Add(cardmod);
                                }

                                CardModificationInfo cardModificationInfo = new CardModificationInfo(); //make mod info
                                cardModificationInfo.negateAbilities = new List<Ability> { ability1 }; //negate selected sigil

                                targetCardInfo.Mods.Add(cardModificationInfo); //apply negation

                                cardSlot.Card.SetInfo(targetCardInfo); //implement all the new mods

                                cardSlot.Card.RenderCard();

                                yield return new WaitForSeconds(0.3f);

                                yield return base.LearnAbility(0.5f);
                            }

                        }

                    }
                }


                yield break;

            }

        }


        //zombie caller
        private NewAbility AddZombieWave()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Zombie Caller";
            info.rulebookDescription = "When a card bearing this sigil is played, A zombie gets played on each empty adjacent space. A zombie is defined as: 1 Power 2 Health.";
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook};

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "A huge wave of zombies is approaching";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("ZombieWave");

            NewAbility ability = new NewAbility(info, typeof(ZombieWave), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            ZombieWave.ability = ability.ability;
            return ability;
        }
        public class ZombieWave : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;

            public override bool RespondsToResolveOnBoard()
            {
                return true;
            }

            public override IEnumerator OnResolveOnBoard()
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
                CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
                bool toLeftValid = toLeft != null && toLeft.Card == null;
                bool toRightValid = toRight != null && toRight.Card == null;

                yield return base.PreSuccessfulTriggerSequence();
                if (toLeftValid)
                {
                    yield return new WaitForSeconds(0.1f);
                    yield return SpawnCardOnSlot(toLeft);
                }
                if (toRightValid)
                {
                    yield return new WaitForSeconds(0.1f);
                    yield return SpawnCardOnSlot(toRight);
                }
                if (toLeftValid || toRightValid)
                {
                    Say("A huge wave of zombies is approaching");
                }
                yield break;
            }

            private IEnumerator SpawnCardOnSlot(CardSlot slot)
            {
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("zep_pvz_zombie"), slot, 0.15f, true);
                yield break;
            }
        }


        //hardheaded
        private NewAbility AddHardHeaded()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Hard Headed";
            info.rulebookDescription = "When a card bearing this sigil is attacked, the damage taken will be reduced by 2 and Touch of Death will be ignored. Then this sigil will be removed.";
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "Headwear is surprisingly good at negating wounds";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            // Load the image into a Texture2D object
            Texture2D tex = findTexture("HardHeaded");

            NewAbility ability = new NewAbility(info, typeof(HardHeaded), tex, AbilityIdentifier.GetID(PluginGuid, info.rulebookName));
            HardHeaded.ability = ability.ability;
            return ability;
        }
        public class HardHeaded : AbilityBehaviour
        {
            public override Ability Ability
            {
                get { return ability; }
            }
            public static Ability ability;

            public override bool RespondsToResolveOnBoard()
            {
                return true;
            }

        }



        //==========special abilities====================================================

        //keepinfoice 
        private NewSpecialAbility AddInfoStoreIce()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Info Store Ice";
            NewSpecialAbility newSpecialAbility = new NewSpecialAbility(typeof(InfoStoreIce), SpecialAbilityIdentifier.GetID(PluginGuid, info.rulebookName), null);
            InfoStoreIce.SpecialTriggeredAbility = newSpecialAbility.specialTriggeredAbility;
            return newSpecialAbility;
        } 
        
        //eolvesprout
        private NewSpecialAbility AddEvolveSprout()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Info Store Ice";
            NewSpecialAbility newSpecialAbility = new NewSpecialAbility(typeof(EvolveSprout), SpecialAbilityIdentifier.GetID(PluginGuid, info.rulebookName), null);
            EvolveSprout.SpecialTriggeredAbility = newSpecialAbility.specialTriggeredAbility;
            return newSpecialAbility;
        }

        //suncost
        private NewSpecialAbility AddSunCost()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.rulebookName = "Sun Cost";
            NewSpecialAbility newSpecialAbility = new NewSpecialAbility(typeof(SunCost), SpecialAbilityIdentifier.GetID(PluginGuid, info.rulebookName), null);
            SunCost.SpecialTriggeredAbility = newSpecialAbility.specialTriggeredAbility;
            return newSpecialAbility;
        }


        //==========make shroom stats====================================================
        public class ShroomAttack : VariableStatBehaviour
        {
            private static SpecialStatIcon specialStatIcon;
            public override SpecialStatIcon IconType
            {
                get
                {
                    return ShroomAttack.specialStatIcon;
                }
            }

            public override int[] GetStatValues()
            {
                List<CardSlot> source = base.PlayableCard.Slot.IsPlayerSlot ? Singleton<BoardManager>.Instance.PlayerSlotsCopy : Singleton<BoardManager>.Instance.OpponentSlotsCopy;
                int num = (from slot in source
                           where slot.Card != null
                           select slot).Count((CardSlot cardSlot) => cardSlot.Card.Info.HasTrait((Trait)7801)); //check for mushroom trait
                return new int[] {0,num};
            }

            public static NewSpecialAbility InitStatIconAndAbility()
            {
                StatIconInfo statIconInfo = ScriptableObject.CreateInstance<StatIconInfo>();
                statIconInfo.appliesToAttack = true;
                statIconInfo.appliesToHealth = false;
                statIconInfo.rulebookName = "Mushroom (Attack)";
                statIconInfo.rulebookDescription = "The value represented with this sigil will be equal to the number of Mushrooms that the owner has on their side of the table.";

                statIconInfo.iconGraphic = Plugin.findTexture("Shrooms");

                SpecialAbilityIdentifier id = SpecialAbilityIdentifier.GetID("zepht.inscryption.cards.ZephtPvZ.", statIconInfo.rulebookName);
                NewSpecialAbility newSpecialAbility = new NewSpecialAbility(typeof(ShroomAttack), id, statIconInfo);
                ShroomAttack.specialStatIcon = newSpecialAbility.statIconInfo.iconType;

                return newSpecialAbility;

            }

        } //locked


        //==========update====================================================
        private void Update()//dev locked
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Say("You have " + Plugin.Sun.ToString() + " Sun.");
                UpdateSunCounter();
            }
            if (Input.GetKeyDown(KeyCode.Z) && false)
            {
                Say("You have gained sun.");
                Sun++;

                
                if (SunCounterObject != null)
                {
                    UpdateSunCounter(); 
                    SunCounterCard.Anim.LightNegationEffect(); 
                }
            }
            if (Input.GetKeyDown(KeyCode.C) && false)
            {
                Say("You have lost sun.");
                Sun--;

                if (SunCounterObject != null) 
                { 
                    UpdateSunCounter(); 
                    SunCounterCard.Anim.LightNegationEffect(); 
                }
            }

        }


        //==========Patches for sun cost======================================
        [HarmonyPatch(typeof(TurnManager), "DoUpkeepPhase")]
        public class TurnStart_patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref bool playerUpkeep)
            {
                if (playerUpkeep)
                {
                    Plugin.Log.LogInfo("Sun upkeep patch initialised");//note
                    SunCounterCard.Anim.LightNegationEffect();

                    Plugin.Sun+=1; //number of sun per round
                    UpdateSunCounter();

                   
                }
            }
        }

        [HarmonyPatch(typeof(GameFlowManager), "TransitionTo")]
        public class Transition_To_patch
        {
            [HarmonyPrefix]
            public static void Prefix()
            {

                Plugin.Log.LogInfo("Sun transition patch initialised");//note
                Plugin.Sun = 2; //number of starting sun (then + upkeep)
            }
        }




        [HarmonyPatch(typeof(CardDrawPiles), "Initialize")]
        public class Initialise_patch
        {
            [HarmonyPostfix]

            public static void Postfix()
            {
                Plugin.Log.LogInfo("Sun counter patch initialised");//note

                SunCounterObject = UnityEngine.Object.Instantiate<GameObject>(Singleton<CardSpawner>.Instance.playableCardPrefab);

                UpdateSunCounter();
                SunCounterCard.Anim.StrongNegationEffect();

                Vector3 SunCounterTranslation = new Vector3(1.24f, 1.05f, 5.014f ); //remove offset
                SunCounterObject.transform.Translate(SunCounterTranslation);

                SunCounterTranslation = new Vector3(-2.52f, -0.4f, -4.83f ); //add custom position
                SunCounterObject.transform.Translate(SunCounterTranslation);

                Vector3 SunCounterRotation = new Vector3(-90, 0, 0);//remove rotation
                SunCounterObject.transform.Rotate(SunCounterRotation);

                Vector3 SunCounterRotation2 = new Vector3(15, -35f, 0f); //add custom rotation
                SunCounterObject.transform.Rotate(SunCounterRotation2);

                SunCounterObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

                
            }
        }

        [HarmonyPatch(typeof(CardDrawPiles), "CleanUp")]
        public class Cleanup_patch
        {
            [HarmonyPostfix]

            public static void Postfix()
            {
                Plugin.Log.LogInfo("Sun counter patch clean up");//note

                if (SunCounterObject != null)
                {
                    SunCounterCard.Anim.StrongNegationEffect();
                    UnityEngine.Object.Destroy(SunCounterObject.gameObject);
                }
            }
        }

        public static void UpdateSunCounter()
        {
            PlayableCard component = SunCounterObject.GetComponent<PlayableCard>();
            SunCounterCardInfo.displayedName = "Sun: " + Sun;
            
            component.SetInfo(SunCounterCardInfo);

        }

        public static void UpdateSunCoin()
        {
            PlayableCard component = SunCoinObject.GetComponent<PlayableCard>();
            SunCoinCardInfo.displayedName = " ";

            component.SetInfo(SunCoinCardInfo);

        }

        public static IEnumerator playSunAnimation(CardSlot slot, bool intoCounter)
        {

            Plugin.SunCoinObject = UnityEngine.Object.Instantiate<GameObject>(Singleton<CardSpawner>.Instance.playableCardPrefab);
            UpdateSunCoin();

            SunCoinCard.Anim.StrongNegationEffect();
            Log.LogInfo("called from " + slot.Index);

            float offset = 1.4f * slot.Index;

            Vector3 SunCoinCardTarget = new Vector3(-0.19f + offset, -0.46f, -0.1f); //remove offset
            Vector3 SunCoinCounterTarget = new Vector3(- 1.4f, 0.65f, -0.9f);

            float SunCoinSize = 0.6f;
            SunCoinObject.transform.localScale = new Vector3(SunCoinSize , SunCoinSize , SunCoinSize );

            SunCoinSize /= 10;

            if (intoCounter)
            {

                Log.LogInfo("card at " + SunCoinObject.transform.position);
                SunCoinObject.transform.Translate(SunCoinCardTarget);

                Vector3 step = SunCoinCounterTarget - SunCoinCardTarget;
                step /= 3;

                yield return new WaitForSeconds(0.1f);

                for (int i = 0; i < 8; i++)
                {

                    yield return new WaitForSeconds(0.005f);
                    SunCoinObject.transform.Translate(step);


                    Log.LogInfo("card at " + SunCoinObject.transform.position);

                }

                Destroy(SunCoinObject);

            }
        } //not used until maybe model added


        //==========card backgrounds===================================================

        public class SunCounterBackground : CardAppearanceBehaviour
        {
            public static Appearance CustomAppearance;
            public static void Initialize()
            {
                NewCardAppearanceBehaviour newBackgroundBehaviour = NewCardAppearanceBehaviour.AddNewBackground(typeof(SunCounterBackground), "SunCounterBackground");
                CustomAppearance = newBackgroundBehaviour.Appearance;
            }

            public override void ApplyAppearance()
            {
                base.Card.renderInfo.baseTextureOverride = findTexture("SunCounter");

                Color color = new Color();
                color.a = 1;


                color.r = 1f;
                color.g = 0.5f;
                color.b = 0f;

                base.Card.renderInfo.nameTextColor = color;
                SunCounterCard = base.Card;

            }
        }

        public class InstantCardBackground : CardAppearanceBehaviour
        {
            public static Appearance CustomAppearance;
            public static void Initialize()
            {
                NewCardAppearanceBehaviour newBackgroundBehaviour = NewCardAppearanceBehaviour.AddNewBackground(typeof(InstantCardBackground), "InstantCardBackground");
                CustomAppearance = newBackgroundBehaviour.Appearance;
            }

            public override void ApplyAppearance()
            {
                base.Card.renderInfo.baseTextureOverride = findTexture("InstantBack");

                if (base.Card.Info.baseAttack == 0)
                {
                    base.Card.renderInfo.hiddenAttack = true;
                }

                if (base.Card.Info.baseHealth == 0)
                {
                    base.Card.renderInfo.hiddenHealth = true;
                }
            }
        }


        //==========card gen patches====================================================
        [HarmonyPatch(typeof(Part1CardChoiceGenerator), "GenerateTribeChoices")]
        public class TribeChoice_patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref List<CardChoice> __result, int randomSeed)
            {

                var list = __result;

                __result.Clear(); //remove all vanilla tribes

                //add each of my tribes
                CardChoice attackChoice = new CardChoice();
                attackChoice.tribe = PlantsAttack.tribe;
                list.Add(attackChoice);

                CardChoice sunChoice = new CardChoice();
                sunChoice.tribe = PlantsSun.tribe;
                list.Add(sunChoice);

                CardChoice bombChoice = new CardChoice();
                bombChoice.tribe = PlantsBomb.tribe;
                list.Add(bombChoice);

                CardChoice defendChoice = new CardChoice();
                defendChoice.tribe = PlantsDefend.tribe;
                list.Add(defendChoice);

                CardChoice supportChoice = new CardChoice();
                supportChoice.tribe = PlantsSupport.tribe;
                list.Add(supportChoice);
                
                //shuffle the cards
                Plugin.ShuffleChoice(list);
                

                while (list.Count > 3)
                {
                    list.RemoveAt(SeededRandom.Range(0, list.Count, randomSeed++)); //remove the excess
                }

                __result = list;
            }
        }

        [HarmonyPatch(typeof(CardSingleChoicesSequencer), "GetCardbackTexture")]
        public class SunCostBack_patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref Texture __result, CardChoice choice)
            {
                if (choice.resourceType == SunResource)
                {
                    __result = findTexture("SunCostTrial");
                }
            }
        }

        [HarmonyPatch(typeof(Part1CardChoiceGenerator), "GenerateCostChoices")]
        public class CostChoice_patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref List<CardChoice> __result, int randomSeed)
            {
                List<CardChoice> list = __result;

                CardChoice sunChoice = new CardChoice();
                sunChoice.resourceType = SunResource;
                sunChoice.resourceAmount = 1;
                sunChoice.tribe = PlantsAttack.tribe;

                list.Add(sunChoice);

                ShuffleChoice(list);

                while (list.Count > 3)
                {
                    list.RemoveAt(SeededRandom.Range(0, list.Count, randomSeed++));
                }

                if (list[0] != sunChoice && list[1] != sunChoice && list[2] != sunChoice) //guarantee sun
                {
                    list[0] = sunChoice;
                    ShuffleChoice(list);

                }

                __result = list;
            }
        }

        [HarmonyPatch(typeof(CardSingleChoicesSequencer))]
        public class SunCostCreateCard_patch
        {
            [HarmonyPostfix]
            [HarmonyPatch("CostChoiceChosen")]
            public static IEnumerator Postfix(IEnumerator enumerator, CardSingleChoicesSequencer __instance, SelectableCard card)
            {
                if (card.ChoiceInfo.resourceType == SunResource)
                {
                    CardInfo cardInfo = GetRandomChoosablePlantCard(SaveManager.SaveFile.GetCurrentRandomSeed());
                    card.SetInfo(cardInfo);
                    card.SetFaceDown(false, false);
                    card.SetInteractionEnabled(false);
                    yield return __instance.TutorialTextSequence(card);
                    card.SetCardbackToDefault();
                    yield return __instance.WaitForCardToBeTaken(card);
                    yield break;
                }
                yield return enumerator;
                yield break;
            }
        }
        public static CardInfo GetRandomChoosablePlantCard(int randomSeed)
        {
            List<CardInfo> list = CardLoader.RemoveDeckSingletonsIfInDeck(ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x)
                => (x.metaCategories.Contains(CardMetaCategory.ChoiceNode ))
                && (x.IsOfTribe(Plugin.PlantsAttack.tribe) || x.IsOfTribe(Plugin.PlantsDefend.tribe) || x.IsOfTribe(Plugin.PlantsSun.tribe) || x.IsOfTribe(Plugin.PlantsBomb.tribe) || x.IsOfTribe(Plugin.PlantsSupport.tribe)
                && (!x.specialAbilities.Contains(EvolveSprout.SpecialTriggeredAbility))
                )));

            CardInfo result;

            if (list.Count == 0)
            {
                result = CardLoader.Clone(CardLoader.GetCardByName("zep_pvz_puffshroom-00"));
            }
            else
            {
                result = CardLoader.Clone(list[SeededRandom.Range(0, list.Count, randomSeed)]);
            }
            return result;
        }

        
        [HarmonyPatch(typeof(DeckTrialSequencer), "GenerateRewardChoices")]
        public class TrialChoice_patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref List<CardInfo> __result, int randomSeed)
            {

                var list = __result;

                //remove nosac cards!!!


                

                while (list.Count > 3)
                {
                    list.RemoveAt(SeededRandom.Range(0, list.Count, randomSeed++)); //remove the excess
                }

                __result = list;
            }
        }


        [HarmonyPatch(typeof(GainConsumablesSequencer), "FullConsumablesSequence")]
        public class Rat_patch
        {
            [HarmonyPrefix]
            public static void Prefix(ref GainConsumablesSequencer __instance)
            {
                CardInfo ratReplace = CardLoader.GetCardByName("zep_pvz_magnet-03");

                __instance.fullConsumablesReward = ratReplace;
            }
        }

        [HarmonyPatch(typeof(PackMule), "GenerateCardPack")]
        public class Mule_patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref List<CardInfo> __result)
            {
                var list = __result;

                int currentRandomSeed = SaveManager.SaveFile.GetCurrentRandomSeed();

                while (list.Count <= 5)
                {
                    list.Add(GetRandomChoosableCardWithSunCost(currentRandomSeed++));
                }

                __result = list;
            }
        }


        [HarmonyPatch(typeof(CardMergeSequencer), "GetValidCardsForHost")] //sacstone host
        public class SacStoneHost_patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref List<CardInfo> __result)
            {
                var list = __result;

                list.RemoveAll((CardInfo x) => x.traits.Contains(NoSacs) || x.abilities.Contains(AddStats.ability) || x.abilities.Contains(Spicy.ability) || x.abilities.Contains(Sweet.ability));

                __result = list;
            }
        }


        [HarmonyPatch(typeof(CardMergeSequencer), "GetValidCardsForSacrifice")] //sacstone sacrifice
        public class SacStoneSacrifice_patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref List<CardInfo> __result)
            {
                var list = __result;

                list.RemoveAll((CardInfo x) => x.traits.Contains(NoSacs) || x.abilities.Contains(AddStats.ability) || x.abilities.Contains(Spicy.ability) || x.abilities.Contains(Sweet.ability));

                __result = list;
            }
        }


        [HarmonyPatch(typeof(CardStatBoostSequencer), "GetValidCards")] //campfire
        public class CampfireBoost_patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref List<CardInfo> __result)
            {
                var list = __result;

                list.RemoveAll((CardInfo x) => x.traits.Contains(NoSacs) || x.abilities.Contains(AddStats.ability) || x.abilities.Contains(Spicy.ability) || x.abilities.Contains(Sweet.ability));

                __result = list;
            }
        }



        //==========other subroutines========================================================
        private void makeDecals()
        {
            costDecals = new Texture[] { findTexture("SunCost1"), findTexture("SunCost2"), findTexture("SunCost3"), findTexture("SunCost4"), findTexture("SunCost5"), findTexture("SunCost6"), findTexture("SunCost7"), findTexture("SunCost8"), findTexture("SunCost9") };

            foreach (Texture tex in costDecals)
            {
                tex.name = "customsuncost";//set label
            }
        }

        public static Texture2D findTexture(string imageName)
        {
            byte[] imgBytes = ExtractEmbeddedResource("ZephtPvZ/Assets/"+imageName+".png");
            Texture2D tex = new Texture2D(2, 2);
            tex.filterMode = FilterMode.Point;
            tex.LoadImage(imgBytes);

            return tex;
        }

        public static byte[] ExtractEmbeddedResource(String filePath)
        {
            filePath = filePath.Replace("/", ".");
            filePath = filePath.Replace("\\", ".");
            var baseAssembly = Assembly.GetCallingAssembly();

            using (Stream resFilestream = baseAssembly.GetManifestResourceStream(filePath))
            {
                if (resFilestream == null)
                {
                    return null;
                }
                byte[] ba = new byte[resFilestream.Length];
                resFilestream.Read(ba, 0, ba.Length);

                return ba;
            }

        }

        private EvolveIdentifier findEvolution(string evoName)
        {
            EvolveIdentifier evolveIdentifier = new EvolveIdentifier("zep_pvz_" + evoName, 1, null);
            if (evoName == null) { evolveIdentifier = null; }
            return evolveIdentifier;
        }

        private TailIdentifier findTail(string tailName)
        {
            TailIdentifier tailIdentifier = new TailIdentifier("zep_pvz_" + tailName);
            if (tailName == null) { tailIdentifier = null; }
            return tailIdentifier;
        }

        private IceCubeIdentifier findIce(string iceName)
        {
            IceCubeIdentifier iceCubeIdentifier = new IceCubeIdentifier("zep_pvz_" + iceName);
            if (iceName == null) { iceCubeIdentifier = null; }
            return iceCubeIdentifier;
        }

        private static void Say(string text)
        {
            Singleton<TextDisplayer>.Instance.StartCoroutine(Singleton<TextDisplayer>.Instance.ShowUntilInput(text, 0f, 0.5f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true));

        }

        public static void ShuffleChoice(List<CardChoice> list)
        {

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                CardChoice value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static CardInfo GetRandomChoosableCardWithSunCost(int randomSeed)
        {
            List<CardInfo> list = CardLoader.GetUnlockedCards(CardMetaCategory.ChoiceNode, CardTemple.Nature).FindAll((CardInfo x) => x.specialAbilities.Contains(SunCost.SpecialTriggeredAbility));
            if (list.Count == 0)
            {
                return null;
            }
            return CardLoader.Clone(list[SeededRandom.Range(0, list.Count, randomSeed)]);
        }
    }
}

