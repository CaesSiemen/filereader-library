using System;
using System.Collections.Generic;

namespace FileReaderLibrary.Permissions
{
    public class DefaultPermissionHandler : IPermissionsHandler
    {
        private readonly List<string> internalRoles = new List<string> { "admin", "regular", "temporary" };

        public bool HasReadPermission(string roleName)
        {
            if (internalRoles.Contains(roleName.ToLower()))
            {
                return roleName.ToLower() switch
                {
                    "admin" => true,
                    "regular" => true,
                    "temporary" => false,
                    _ => false
                };
            }
            else
                throw new UnauthorizedAccessException("Unauthorized to read this file. Provided role is unknown.");
        }
    }
}
