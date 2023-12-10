using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;


namespace Looted_Village_Interactions_Revived
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
            InformationManager.DisplayMessage(new InformationMessage("[LVI] Mod Loaded !", LVISubModule.Ini_Color));
        }

        protected override void InitializeGameStarter(Game game, IGameStarter gameStarterObject)
        {

            base.InitializeGameStarter(game, gameStarterObject);
            if (gameStarterObject is CampaignGameStarter starter && game.GameType is Campaign)
            {
                ((CampaignGameStarter)gameStarterObject).AddBehavior((CampaignBehaviorBase)new LVIBehavior());
            }
        }

        private static readonly bool DebugMode = true;

        private static readonly Color Ini_Color = Color.FromUint(7194750U);

        public static readonly Color Dbg_Color = Color.FromUint(16005134U);


    }
}