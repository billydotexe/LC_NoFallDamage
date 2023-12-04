using GameNetcodeStuff;
using HarmonyLib;

namespace NoFallDamage.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB), 
                    nameof(PlayerControllerB.DamagePlayer)
    )]
    internal class PlayerControllerB_DamagePlayer : IPatchable
    {
        /// <summary>
        /// Intercept the DamagePlayer call and thinker with the parameters
        /// if the damage being takes is from fall damage or the cause of theath would be gravity
        /// disable fall damage and set the damage that whould be taken to zero
        /// </summary>
        /// <param name="damageNumber">injected from harmony</param>
        /// <param name="causeOfDeath">injected from harmony</param>
        /// <param name="fallDamage">injected from harmony</param>
        [HarmonyPrefix]
        public static void Prefix([HarmonyArgument("damageNumber")]ref int damageNumber,
                                    [HarmonyArgument("causeOfDeath")] ref CauseOfDeath causeOfDeath,
                                    [HarmonyArgument("fallDamage")] ref bool fallDamage)
        {
            if(causeOfDeath == CauseOfDeath.Gravity || fallDamage)
            {
                NoFallDamagePatch.mls.LogMessage("damage nullified");
                damageNumber = 0;
                fallDamage = false;
            }
        }

        /// <summary>
        /// after DamagePlayer was called if the damage would have been fall damage
        /// or if the cause of thead whould have been gravity call the animation method
        /// passing 0s as animation time to override the behavior and avoid the animation
        /// to be played
        /// </summary>
        /// <param name="__instance">the instance of PlayerControllerB that called DamagePlayer</param>
        /// <param name="damageNumber">injected from harmony</param>
        /// <param name="causeOfDeath">injected from harmony</param>
        /// <param name="fallDamage">injected from harmony</param>
        [HarmonyPostfix]
        public static void Postix(PlayerControllerB __instance,
                                    [HarmonyArgument("damageNumber")] ref int damageNumber,
                                    [HarmonyArgument("causeOfDeath")] ref CauseOfDeath causeOfDeath,
                                    [HarmonyArgument("fallDamage")] ref bool fallDamage)
        {
            if (causeOfDeath == CauseOfDeath.Gravity || fallDamage)
            {
                __instance.PlayQuickSpecialAnimation(0f);
            }
        }
    }
}
