using System;
using TaleWorlds.CampaignSystem;

namespace Looted_Village_Interactions_Revived
{
    public partial class LVIBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, this.OnSessionLaunched);
        }

        public override void SyncData(IDataStore dataStore)
        {
            throw new System.NotImplementedException();
        }

        private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            this.AddGameMenus(campaignGameStarter);
        }
    }
}