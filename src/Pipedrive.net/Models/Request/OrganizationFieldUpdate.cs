﻿using Newtonsoft.Json;

namespace Pipedrive
{
    public class OrganizationFieldUpdate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("options")]
        public object Options { get; set; }
    }
}
