using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;




namespace ZephtPvZ
{
    public class EvolveSprout : SpecialCardBehaviour
    {

        public static SpecialTriggeredAbility SpecialTriggeredAbility;

        private void Start()
        {

        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return true;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {

            //Plugin.Log.LogInfo("evocheck 1");

            EvolveParams evoParams = new EvolveParams();
            evoParams.turnsToEvolve = 1;

            int randomSeed = base.GetRandomSeed();

            //Plugin.Log.LogInfo("seed = "+ randomSeed);
            CardInfo randomChoosableCard = GetRandomSproutEvo(randomSeed++);
            evoParams.evolution = randomChoosableCard;


            Plugin.Log.LogInfo("card = " + randomChoosableCard.name);

            //Plugin.Log.LogInfo("evocheck 2");

            this.Card.Info.evolveParams = evoParams;

            //Plugin.Log.LogInfo("evocheck 3");

            yield break;

        }

        public static CardInfo GetRandomSproutEvo(int randomSeed)
        {
            Plugin.Log.LogInfo("get random sprout");
            List<CardInfo> unlockedCards = GetPlantCards();
            return CardLoader.Clone(unlockedCards[SeededRandom.Range(0, unlockedCards.Count, randomSeed)]);
        }

        public static List<CardInfo> GetPlantCards()
        {
            Plugin.Log.LogInfo("get plant card");
            return CardLoader.RemoveDeckSingletonsIfInDeck(ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x)
                => (x.metaCategories.Contains(CardMetaCategory.ChoiceNode) || x.metaCategories.Contains(CardMetaCategory.Rare))
                && (x.IsOfTribe(Plugin.PlantsAttack.tribe) || x.IsOfTribe(Plugin.PlantsDefend.tribe) || x.IsOfTribe(Plugin.PlantsSun.tribe) || x.IsOfTribe(Plugin.PlantsBomb.tribe) || x.IsOfTribe(Plugin.PlantsSupport.tribe)
                && (!x.specialAbilities.Contains(EvolveSprout.SpecialTriggeredAbility))
                && (!x.abilities.Contains(Plugin.AddStats.ability))
                )));
        }
    }





}