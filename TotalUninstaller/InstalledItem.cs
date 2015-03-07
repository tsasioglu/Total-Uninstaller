using System;

namespace TotalUninstaller
{
    public class InstalledItem
    {
        public string   Product         { get; set; }
        public string   ProductCode     { get; set; }
        public string   InstallDate     { get; set; }
        public string   InstallLocation { get; set; }
        public Uri      Url             { get; set; }
        public Version  Version         { get; set; }
        public bool     Uninstall       { get; set; }

        public InstalledItem(string product, string productCode, DateTime installDate, string installLocation, Uri url, Version version)
        {
            Product         = product;
            ProductCode     = productCode;
            InstallDate     = installDate.ToShortDateString();
            InstallLocation = installLocation;
            Url             = url;
            Version         = version;
        }
    }
}
