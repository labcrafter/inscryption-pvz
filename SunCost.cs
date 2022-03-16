using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using DiskCardGame;
using UnityEngine;


namespace ZephtPvZ
{
    public class SunCost : SpecialCardBehaviour
    {

        public static SpecialTriggeredAbility SpecialTriggeredAbility;

        private void Start()
        {
            int num = int.Parse(findCost(base.Card));//read card name for number

            if(num > 9) { num = 9; } //temp upper limit of cost render *

            //Plugin.Log.LogInfo("checking for " + base.Card.name);//note
            //Plugin.Log.LogInfo("num is " + num);//note
            //Plugin.Log.LogInfo("cost decal length is "+ Plugin.costDecals.Length);//note

            if (num != 0 && base.Card.Info.decals.Count <=1 ) //if number found, and doesnt already have a sun cost decal
            {
                //Plugin.Log.LogInfo("inside num");//note
                base.Card.Info.decals.Add(Plugin.costDecals[num-1]); //add the corresponding decal
            }

            base.Card.RenderCard(); 
        }

        public override bool RespondsToPlayFromHand() //checks for play
        {
            return TurnManager.Instance.IsPlayerMainPhase;
        }

        public override IEnumerator OnPlayFromHand() //when played
        {

            yield return new WaitForSeconds(0.1f);
            int count = 0; //sets default cost

            if (int.TryParse(findCost(base.Card), out count)) //converts last digits of name to cost
            { 
                for (int i = 0; i < count; i++)
                    {
                        if (Plugin.Sun > 0)
                        {
                        Plugin.SunCounterCard.Anim.LightNegationEffect();
                        Plugin.Sun -= 1; //removes cost from Sun
                        Plugin.UpdateSunCounter();
                        yield return new WaitForSeconds(0.1f);
                        }
                    }
            }
            yield break;
        }




       //patches

        [HarmonyPatch(typeof(PlayableCard), "CanPlay")] // checks if card can be played
        public class CanPlay_patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref PlayableCard __instance, ref bool __result)
            {
                if (__instance.Info.SpecialAbilities.Contains(SunCost.SpecialTriggeredAbility) & __result) //checks if played card has suncost
                {
                    int num = int.Parse(findCost(__instance));

                    if (Plugin.Sun < num) //checks if enough sun to play
                    {
                        __instance.StartCoroutine(Singleton<TextDisplayer>.Instance.ShowUntilInput("You lack the sun to play this creature.", 0f, 0.5f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true));
                        Plugin.SunCounterCard.Anim.StrongNegationEffect();
                        Plugin.SunCounterCard.Anim.StrongNegationEffect();
                        __result = false;
                    }
                }
            }
        }


        [HarmonyPatch(typeof(CardDisplayer3D), "DisplayDecals")] //stop costs from overlapping
        public class DisplayDecals_patch
        {
            [HarmonyPrefix]
            public static void Prefix(List<Texture> decalTextures, CardDisplayer3D __instance)
            {
                foreach (Texture texture in decalTextures)
                {
                    int num = 0;

                    if (__instance.info.BloodCost > 0)
                    {
                        num++;
                    }
                    if (__instance.info.BonesCost > 0)
                    {
                        num++;
                    }
                    if (__instance.info.EnergyCost > 0)
                    {
                        num++;
                    }
                    if (__instance.info.gemsCost.Count > 0)
                    {
                        num++;
                    }
                    if (texture.name == "customsuncost")
                    {
                        __instance.decalRenderers[1].transform.localPosition = new Vector3(0f, -0.1f, 0f) * (float)num;
                    }
                }
            }
        }
        //utility
        private static string findCost(Card card)
        {
            string name = card.Info.name.ToString();
            string cost = (name[name.Length - 2].ToString()
                + name[name.Length - 1].ToString());

            return cost;
        }
    }





}