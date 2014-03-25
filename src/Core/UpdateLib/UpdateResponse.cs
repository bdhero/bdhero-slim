﻿// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UpdateLib
{
    public class UpdateResponse
    {
        [JsonConverter(typeof(VersionConverter))]
        public Version Version { get; set; }

        /// <summary>
        /// ISO 8601 date format (e.g. <c>2008-04-12T12:53Z</c>).  See <see cref="IsoDateTimeConverter"/>.
        /// </summary>
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Date { get; set; }

        public List<string> Mirrors { get; set; }

        public PlatformList Platforms { get; set; }

        public string ReleaseNotes { get; set; }

        public UpdateResponse()
        {
            Version = new Version();
            Date = DateTime.Now;
            Mirrors = new List<string>();
            Platforms = new PlatformList();
            ReleaseNotes = "";
        }
    }

    public class PlatformList
    {
        public Platform Windows { get; set; }

        [JsonIgnore]
        public Platform Mac { get; set; }

        [JsonIgnore]
        public Platform Linux { get; set; }

        public PlatformList()
        {
            Windows = new Platform();
            Mac = new Platform();
            Linux = new Platform();
        }
    }

    public class Platform
    {
        public PackageList Packages { get; set; }

        public Platform()
        {
            Packages = new PackageList();
        }
    }

    public class PackageList
    {
        public Package Setup { get; set; }
        public Package Zip { get; set; }
    }

    public class Package
    {
        [JsonProperty(PropertyName = "filename")]
        public string FileName { get; set; }

        public string SHA1 { get; set; }
        public long Size { get; set; }
    }
}
