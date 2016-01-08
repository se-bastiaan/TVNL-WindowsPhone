// Copyright (c) Attack Pattern LLC.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration.Pnp;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace WindowsPhone81TVNL.Helpers
{
    public class DeviceInformationHelper
    {
        const string ItemNameKey = "System.ItemNameDisplay";
        const string ModelNameKey = "System.Devices.ModelName";
        const string ManufacturerKey = "System.Devices.Manufacturer";
        const string DeviceClassKey = "{A45C254E-DF1C-4EFD-8020-67D146A850E0},10";
        const string PrimaryCategoryKey = "{78C34FC8-104A-4ACA-9EA4-524D52996E57},97";
        const string DeviceDriverVersionKey = "{A8B865DD-2E3D-4094-AD97-E593A70C75D6},3";
        const string RootContainer = "{00000000-0000-0000-FFFF-FFFFFFFFFFFF}";
        const string RootQuery = "System.Devices.ContainerId:=\"" + RootContainer + "\"";
        const string HalDeviceClass = "4d36e966-e325-11ce-bfc1-08002be10318";

        public static async Task<ProcessorArchitecture> GetProcessorArchitectureAsync()
        {
            var halDevice = await GetHalDevice(ItemNameKey);
            if (halDevice != null && halDevice.Properties[ItemNameKey] != null)
            {
                var halName = halDevice.Properties[ItemNameKey].ToString();
                if (halName.Contains("x64")) return ProcessorArchitecture.X64;
                if (halName.Contains("ARM")) return ProcessorArchitecture.Arm;
                return ProcessorArchitecture.X86;
            }
            return ProcessorArchitecture.Unknown;
        }

        public static string GetDeviceManufacturer()
        {
            var deviceInfo = new EasClientDeviceInformation();
            return deviceInfo.SystemManufacturer;
        }

        public static string GetDeviceModel()
        {
            var deviceInfo = new EasClientDeviceInformation();
            return deviceInfo.SystemProductName;
        }

        public static Task<string> GetDeviceCategoryAsync()
        {
            return GetRootDeviceInfoAsync(PrimaryCategoryKey);
        }

        public static async Task<string> GetWindowsVersionAsync()
        {
            // There is no good place to get this.
            // The HAL driver version number should work unless you're using a custom HAL... 
            var hal = await GetHalDevice(DeviceDriverVersionKey);
            if (hal == null || !hal.Properties.ContainsKey(DeviceDriverVersionKey))
                return null;

            var versionParts = hal.Properties[DeviceDriverVersionKey].ToString().Split('.');
            return string.Join(".", versionParts.Take(2).ToArray());
        }

        private static async Task<string> GetRootDeviceInfoAsync(string propertyKey)
        {
            var pnp = await PnpObject.CreateFromIdAsync(PnpObjectType.DeviceContainer,
                        RootContainer, new[] { propertyKey });
            return (string)pnp.Properties[propertyKey];
        }

        /// <summary>
        /// Uses the WebView control to get the OS Version from the UserAgent string
        /// </summary>
        /// <returns></returns>
        public static string GetOsVersion()
        {
            var deviceInfo = new EasClientDeviceInformation();
            return deviceInfo.OperatingSystem;
        }

        public static string GetFirmwareVersion()
        {
            var deviceInfo = new EasClientDeviceInformation();
            return deviceInfo.SystemFirmwareVersion;
        }

        private static async Task<PnpObject> GetHalDevice(params string[] properties)
        {
            var actualProperties = properties.Concat(new[] { DeviceClassKey });
            var rootDevices = await PnpObject.FindAllAsync(PnpObjectType.Device,
                actualProperties, RootQuery);

            foreach (var rootDevice in rootDevices.Where(d => d.Properties != null && d.Properties.Any()))
            {
                var lastProperty = rootDevice.Properties.Last();
                if (lastProperty.Value != null)
                    if (lastProperty.Value.ToString().Equals(HalDeviceClass))
                        return rootDevice;
            }
            return null;
        }
    }
}