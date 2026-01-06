using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace UpTulse.Application.Models
{
    public class Contents
    {
        [JsonProperty("en")]
        public string En { get; set; } = string.Empty;
    }

    public class Headings
    {
        [JsonProperty("en")]
        public string En { get; set; } = string.Empty;
    }

    public class PushNotificationRequest
    {
        [JsonProperty("android_channel_id")]
        public string AndroidChannelId { get; set; } = string.Empty;

        [JsonProperty("app_id")]
        public string AppId { get; set; } = string.Empty;

        [JsonProperty("contents")]
        public Contents Contents { get; set; } = new();

        [JsonProperty("headings")]
        public Headings Headings { get; set; } = new();

        [JsonProperty("included_segments")]
        public string[] IncludedSegments { get; set; } = ["Total Subscriptions"];

        [JsonProperty("ios_category")]
        public string IosCategory { get; set; } = string.Empty;

        [JsonProperty("ios_sound")]
        public string IosSound { get; set; } = string.Empty;

        [JsonProperty("isAndroid")]
        public bool IsAndroid { get; set; } = true;

        [JsonProperty("isIos")]
        public bool IsIos { get; set; } = true;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("subtitle")]
        public Subtitle Subtitle { get; set; } = new();

        [JsonProperty("target_channel")]
        public string TargetChannel { get; set; } = "push";
    }

    public class Subtitle
    {
        [JsonProperty("en")]
        public string En { get; set; } = string.Empty;
    }
}