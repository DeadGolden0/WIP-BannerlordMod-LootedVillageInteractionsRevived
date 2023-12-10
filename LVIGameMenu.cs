using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace LootedVillageInteractionsRevived
{
    public partial class LVIBehavior : CampaignBehaviorBase
    {
        private void AddGameMenus(CampaignGameStarter campaignGameStarter)
        {
            campaignGameStarter.AddGameMenuOption("village_looted", "lvi_investigate_option", "{=LVI_Investigate}Investigate", 
                new GameMenuOption.OnConditionDelegate(this.LVI_Investigate_Condition), 
                new GameMenuOption.OnConsequenceDelegate(this.LVI_Investigate_Consequence), false, 3, false, null);

            campaignGameStarter.AddGameMenuOption("village_looted", "lvi_help_option", "{=LVI_Help}Help Village",
                new GameMenuOption.OnConditionDelegate(this.LVI_Help_Condition),
                new GameMenuOption.OnConsequenceDelegate(this.LVI_Help_Consequence), false, 2, false, null);

            campaignGameStarter.AddWaitGameMenu("lvi_help_wait_menu", "{=LVI_Help_Waiting}You are helping the villager of {VILLAGE_NAME}", 
                new OnInitDelegate(this.LVI_Help_Wait_Menu_Init), 
                new OnConditionDelegate(this.LVI_Help_Wait_Menu_Condition), 
                new OnConsequenceDelegate(this.LVI_Help_Wait_Menu_Consequence), 
                new OnTickDelegate(this.LVI_Help_Wait_Menu_Tick), 
                GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption, GameOverlays.MenuOverlayType.None, 0f, GameMenu.MenuFlags.None, null);


            campaignGameStarter.AddGameMenuOption("lvi_help_wait_menu", "lvi_help_menu_leave_option", "{=LVI_Stop_Help}Stop Helping",
                new GameMenuOption.OnConditionDelegate(this.LVI_Help_Menu_Condition),
                new GameMenuOption.OnConsequenceDelegate(this.LVI_Help_Menu_Consequence), false, -1, false, null);

            //campaignGameStarter.AddGameMenuOption("village_looted", "lvi_pillage_option", "{=LVI_Pillage}Start Pillage ",
            //   new GameMenuOption.OnConditionDelegate(this.LVI_Pillage_Condition),
            //   new GameMenuOption.OnConsequenceDelegate(this.LVI_Pillage_Consequence), false, 0, false, null);

        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        // Investigate Menu Option (LVI) - Condition & Consequence
        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        private bool LVI_Investigate_Condition(MenuCallbackArgs args)
        {
            // A DEV
            args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
            return true;
        }

        private void LVI_Investigate_Consequence(MenuCallbackArgs args)
        {
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        // Help Menu Option (LVI) - Condition & Consequence
        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        // HELP MENU - HELP OPTION
        private bool LVI_Help_Condition(MenuCallbackArgs args)
        {
            // A DEV
            args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
            if (LVISubModule.DebugMode)
            {
                args.Tooltip = new TextObject("{=LVI_DebugMod}DebugMode Activated", null);
                args.IsEnabled = true;
                return true;
            }
            if (!FactionManager.IsAlliedWithFaction(Hero.MainHero.MapFaction, Settlement.CurrentSettlement.MapFaction))
            {
                args.Tooltip = new TextObject("{=LVI_Cant_help}It would not be wise to help an enemy village", null);
                args.IsEnabled = false;
            }
            return true;
        }

        private void LVI_Help_Consequence(MenuCallbackArgs args)
        {
            GameMenu.SwitchToMenu("lvi_help_wait_menu");
        }

        // HELP WAIT MENU - HELP OPTION
        private void LVI_Help_Wait_Menu_Init(MenuCallbackArgs args)
        {
            //args.MenuContext.GameMenu.SetTargetedWaitingTimeAndInitialProgress(1f, 0f);
            MBTextManager.SetTextVariable("VILLAGE_NAME", PlayerEncounter.EncounterSettlement.Name, false);
            _startTimeOfWaiting = CampaignTime.Now.GetHourOfDay;
            _targetWaitingHour = (_startTimeOfWaiting + 1) % CampaignTime.HoursInDay;
        }

        private bool LVI_Help_Wait_Menu_Condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Wait;
            return true;
        }

        private void LVI_Help_Wait_Menu_Consequence(MenuCallbackArgs args)
        {
            GameMenu.SwitchToMenu("village_looted");
        }

        private void LVI_Help_Wait_Menu_Tick(MenuCallbackArgs args, CampaignTime dt)
        { 
            int currentHour = CampaignTime.Now.GetHourOfDay;
            int elapsedHours = (currentHour < _startTimeOfWaiting) ?
                (CampaignTime.HoursInDay - _startTimeOfWaiting) + currentHour :
                currentHour - _startTimeOfWaiting;

            float progress = (float)elapsedHours / (float)(24);

            args.MenuContext.GameMenu.SetTargetedWaitingTimeAndInitialProgress(1f, progress);
            //args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(progress);
        }


        // HELP MENU - LEAVE OPTION
        private bool LVI_Help_Menu_Condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Leave;
            return true;
        }

        private void LVI_Help_Menu_Consequence(MenuCallbackArgs args)
        {
            GameMenu.SwitchToMenu("village_looted");
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        // Pillage Menu Option (LVI) - Condition & Consequence
        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

        private bool LVI_Pillage_Condition(MenuCallbackArgs args)
        {
            // A DEV
            args.optionLeaveType = GameMenuOption.LeaveType.Pillage;
            return true;
        }

        private void LVI_Pillage_Consequence(MenuCallbackArgs args)
        {
        }


        private int _startTimeOfWaiting;

        private int _targetWaitingHour;
    }
}
