using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;

namespace Looted_Village_Interactions_Revived
{
    public partial class LVIBehavior : CampaignBehaviorBase
    {
        private void AddGameMenus(CampaignGameStarter campaignGameStarter)
        {
            campaignGameStarter.AddGameMenuOption("village_looted", "lvi_investigate_option", "{=LVI_Investigate}Investigate", 
                new GameMenuOption.OnConditionDelegate(this.LVI_Investigate_Condition), 
                new GameMenuOption.OnConsequenceDelegate(this.LVI_Investigate_Consequence), false, 2, false, null);

            campaignGameStarter.AddGameMenuOption("village_looted", "lvi_help_option", "{=LVI_Help}Help Village",
                new GameMenuOption.OnConditionDelegate(this.LVI_Help_Condition),
                new GameMenuOption.OnConsequenceDelegate(this.LVI_Help_Consequence), false, 2, false, null);

            campaignGameStarter.AddGameMenuOption("village_looted", "lvi_pillage_option", "{=LVI_Pillage}Start Pillage ",
                new GameMenuOption.OnConditionDelegate(this.LVI_Pillage_Condition),
                new GameMenuOption.OnConsequenceDelegate(this.LVI_Pillage_Consequence), false, 2, false, null);

        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        // Investigate Menu Option (LVI) - Condition & Consequence
        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        private bool LVI_Investigate_Condition(MenuCallbackArgs args)
        {
            // A DEV
            return true;
        }

        private void LVI_Investigate_Consequence(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        // Help Menu Option (LVI) - Condition & Consequence
        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

        private bool LVI_Help_Condition(MenuCallbackArgs args)
        {
            // A DEV
            return true;
        }

        private void LVI_Help_Consequence(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        // Pillage Menu Option (LVI) - Condition & Consequence
        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

        private bool LVI_Pillage_Condition(MenuCallbackArgs args)
        {
            // A DEV
            return true;
        }

        private void LVI_Pillage_Consequence(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Pillage;
        }

    }
}
