using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;


namespace LootedVillageInteractionsRevived
{
    public class LVISubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            InformationManager.DisplayMessage(new InformationMessage("[LVIR] Mod Loaded !", LVISubModule.Ini_Color));
        }

        protected override void InitializeGameStarter(Game game, IGameStarter gameStarterObject)
        {
            try
            {
                base.InitializeGameStarter(game, gameStarterObject);
                if (gameStarterObject is CampaignGameStarter starter)
                {
                    starter.AddBehavior(new LVIBehavior());
                }
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage("[LVIR] Error Initializing Game Starter: " + ex.Message, LVISubModule.Dbg_Color));
            }
        }

        private static readonly Color Ini_Color = Color.FromUint(7194750U);

        public static readonly Color Dbg_Color = Color.FromUint(16005134U);
    }
}