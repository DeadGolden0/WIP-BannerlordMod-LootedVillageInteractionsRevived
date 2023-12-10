using System;
using System.Diagnostics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
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

            campaignGameStarter.AddGameMenuOption("village_looted", "lvi_help_option", "{=LVI_Help}Help Village ({GOLD_TO_HELP}{GOLD_ICON} / Hours)",
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
            args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
            MBTextManager.SetTextVariable("GOLD_TO_HELP", _helpingPrice);

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
            if (!PlayerHasEnoughGold(_helpingPrice))
            {
                args.Tooltip = new TextObject("{=LVI_Not_Enought_Money}You don't have enough money to help this village.", null);
                args.IsEnabled = false;
            }
            return true;
        }

        private void LVI_Help_Consequence(MenuCallbackArgs args)
        {
            _lastHourOfHelping = -10;
            GameMenu.SwitchToMenu("lvi_help_wait_menu");
        }

        // HELP WAIT MENU - HELP OPTION
        private void LVI_Help_Wait_Menu_Init(MenuCallbackArgs args)
        {
            // Définir l'image de fond du menu si nécessaire
            args.MenuContext.SetBackgroundMeshName("book_menu_sprite6");

            MBTextManager.SetTextVariable("VILLAGE_NAME", PlayerEncounter.EncounterSettlement.Name, false);

            args.MenuContext.GameMenu.SetTargetedWaitingTimeAndInitialProgress(100 / HealpingPerHourProgress(), (float)_canHelpVillager / 100);
        }

        private bool LVI_Help_Wait_Menu_Condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Wait;
            return true;
        }

        private void LVI_Help_Wait_Menu_Consequence(MenuCallbackArgs args)
        {
            if (_canHelpVillager >= 100) _canHelpVillager = 0;
            float clanRenown = Clan.PlayerClan.Renown;
            int renownGain = MBRandom.RandomInt(this.minRenownWin, this.maxRenownWin);
            Clan.PlayerClan.Renown = clanRenown - (float)renownGain;

            // EXPERIMENTAL
            CreatePopupVMLayer("{VILLAGE_NAME}", "TEST1", "{=LVI_Finish_Help}You have successfully helped the people of {VILLAGE_NAME} escape their dire situation. As a result, your clan {PLAYER_CLAN} earned {RENOW_EARN} points of renown", "TEST2", "lt_education_popup2", "Continue");

            SoundEvent.PlaySound2D("event:/ui/notification/peace");
            //TextObject eventText = new TextObject("{=LVI_Finish_Help}You have successfully helped the people of {VILLAGE_NAME} escape their dire situation. As a result, your clan {PLAYER_CLAN} earned {RENOW_EARN} points of renown", null)
            //    .SetTextVariable("VILLAGE_NAME", PlayerEncounter.EncounterSettlement.Name)
            //    .SetTextVariable("PLAYER_CLAN", Clan.PlayerClan.Name.ToString())
            //    .SetTextVariable("RENOW_EARN", renownGain);
            //MBInformationManager.AddQuickInformation(eventText, 1000, Hero.MainHero.CharacterObject, null);

            GameMenu.SwitchToMenu("village");
        }

        private void LVI_Help_Wait_Menu_Tick(MenuCallbackArgs args, CampaignTime dt)
        {
            int hour = (int)CampaignTime.Now.CurrentHourInDay;
            if (_lastHourOfHelping != -10 && _lastHourOfHelping != hour)
            {
                GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, -_helpingPrice, false);

                _canHelpVillager += HealpingPerHourProgress();
            }
            _lastHourOfHelping = hour;

            args.MenuContext.GameMenu.SetProgressOfWaitingInMenu((float)_canHelpVillager / 100);

            if (Hero.MainHero.Gold < _helpingPrice)
            {
                TextObject eventText = new TextObject("{=LVI_Help_Not_Enought_Money}You no longer have enough money to continue helping this village.", null);
                MBInformationManager.AddQuickInformation(eventText, 1000, Hero.MainHero.CharacterObject, null);

                args.MenuContext.GameMenu.EndWait();
                GameMenu.SwitchToMenu("village_looted");
            }

            // Afficher le débogage si nécessaire.
            if (LVISubModule.DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage("[LVI DebugMod] Progress : " + (float)_canHelpVillager / 100, LVISubModule.Dbg_Color));
            }

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


        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        // Other Functions (LVI) 
        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        private float HealpingPerHourProgress()
        {
            float tacticts = Hero.MainHero.GetSkillValue(DefaultSkills.Tactics);
            if (tacticts > 10) tacticts = 10;
            float progress = (tacticts - 4f) * 0.2f + 5f;
            return progress;
        }

        private bool PlayerHasEnoughGold(int value)
        {
            return Hero.MainHero.Gold >= value;
        }


        private int _lastHourOfHelping;

        private float _canHelpVillager;

        private int _helpingPrice = 15;

        private int minRenownWin = 10;

        private int maxRenownWin = 50;
    }
}
