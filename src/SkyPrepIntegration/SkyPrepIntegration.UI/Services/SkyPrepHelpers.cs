using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyPrepIntegration.UI.Services
{
    public class SkyPrepHelpers
    {
        public static string ListToString(List<string> list, string delimiter)
        {
            return string.Join(delimiter, list);
        }
    }
}