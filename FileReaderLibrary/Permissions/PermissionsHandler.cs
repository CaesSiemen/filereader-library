using System;
using System.Collections.Generic;
using System.Text;

namespace FileReaderLibrary.Permissions
{
    public interface IPermissionsHandler
    {
        public bool HasReadPermission(string roleName);
    }
}
