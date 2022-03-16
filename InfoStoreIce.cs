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


namespace ZephtPvZ
{
    public class InfoStoreIce : SpecialCardBehaviour
    {

        public static SpecialTriggeredAbility SpecialTriggeredAbility;

        private void Start()
        {
        }

        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        public override IEnumerator OnResolveOnBoard()
        {

            Plugin.Log.LogInfo("icecheck 1");

            //CardModificationInfo mod = new CardModificationInfo();

            //mod.fromCardMerge = true;
            //this.Card.Info.ModAbilities;

            //this.Card.Info.Mods.Exists((CardModificationInfo x) => x.fromCardMerge);

            //this.Card.Info.iceCubeParams.creatureWithin.abilities[0].


            CardModificationInfo copymod = new CardModificationInfo(0, 0);
            copymod.abilities.AddRange(base.Card.Info.ModAbilities);
            if (base.Card.Info.ModAbilities.Count > 0)
            {
                copymod.fromCardMerge = true;
            }
            this.Card.Info.iceCubeParams.creatureWithin.Mods.Add(copymod);

            Plugin.Log.LogInfo("icecheck 2");

            yield break;

        }
    }





}