using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Framework.Core.UserDataManagement;
using Microsoft.AspNetCore.Components.Authorization;
using TMS.Models.Shared;

namespace Framework.UserDataManagement
{
    public class UserDataManager : IUserDataManager
    {
        private ClaimsPrincipal user;
        private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private UserGlobalData userGlobalData;
        public UserDataManager(AuthenticationStateProvider authenticationStateProvider,
            UserGlobalData userGlobalData)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            this.userGlobalData = userGlobalData;
            SetUser();
        }

        private async Task SetUser()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            user = authState.User;
            claims = user.Claims;
        }

        public bool IsInRole(string role)
        { 
            SetUser();
            return claims.Any(c => c.Type == "role" && c.Value.Contains(role));
        }

        public string GetAccessibleCenters()
        {
            string accessibleCenters = null;
            while (accessibleCenters == null)
            {
                SetUser();
                accessibleCenters = claims.SingleOrDefault(c => c.Type == "TMSCenterID").Value;
            }
            return accessibleCenters ;
        }


        public async Task SetDefaultActiveCenterId()
        {
            if (userGlobalData.ActiveCenter == 0)
            {
                var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user.Identity.IsAuthenticated == false) return;
                if (user.Claims.Any(c => c.Type == "role" && c.Value.Contains("TMS_GodUser") || c.Value.Contains("TMS_SuperUser")))
                {
                    userGlobalData.ActiveCenter = ShonizCenters.SaeedAbad;
                }
                else
                {
                    userGlobalData.ActiveCenter = (ShonizCenters) int.Parse(user.Claims.FirstOrDefault(c => c.Type == "TMSCenterID").Value);
                }
            }
        }

        public UserGlobalData GetUserGlobalData()
        {
            return userGlobalData;
        }
    }
}
