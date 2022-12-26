using System;

namespace SmartEl.Dtos
{
    [Serializable]
    public class RoleRequest
    {
        public RolesEnum roleType;
        public string password;

        public RoleRequest(RolesEnum roleType, string password)
        {
            this.roleType = roleType;
            this.password = password;
        }
    }
}