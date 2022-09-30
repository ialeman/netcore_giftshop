using SS.Template.Model.Territories;
using System;
using System.Collections.Generic;
using System.IO;

namespace SS.Template.Services
{
    public class TerritoryDataExtractor
    {
        private const int CountryAlpha2Position = 1;
        private const int CountryNamePosition = 0;
        private const int RegionCodePosition = 7;
        private const int RegionPosition = 5;
        private const char Separator = '\t';
        private const int SubregionCodePosition = 8;
        private const int SubregionPosition = 6;
        private readonly Dictionary<string, Country> _countries = new Dictionary<string, Country>();
        private readonly string _path;
        private readonly Dictionary<string, Region> _regions = new Dictionary<string, Region>();
        private readonly Dictionary<string, Subregion> _subregions = new Dictionary<string, Subregion>();

        public TerritoryDataExtractor(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            _path = path;
        }

        public ICollection<Country> Countries
        {
            get { return _countries.Values; }
        }

        public bool HasRun { get; private set; }
        public Action<Country> OnCountryExtracted { get; set; }
        public Action<Region> OnRegionExtracted { get; set; }

        public Action<Subregion> OnSubregionExtracted { get; set; }

        public ICollection<Region> Regions
        {
            get { return _regions.Values; }
        }

        public ICollection<Subregion> Subregions
        {
            get { return _subregions.Values; }
        }

        public void Run()
        {
            if (HasRun) return;

            using (var reader = new StreamReader(_path))
            {
                string line;
                bool headerRead = false;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (!headerRead)
                    {
                        // Ignore header
                        headerRead = true;
                        continue;
                    }

                    var parts = line.Split(Separator);
                    var country = new Country
                    {
                        Name = parts[CountryNamePosition],
                        Code = parts[CountryAlpha2Position],
                        Subregion = GetSubregion(parts)
                    };
                    OnCountryExtracted?.Invoke(country);
                    _countries.Add(country.Code, country);
                }
            }

            HasRun = true;
        }

        private Region GetRegion(string[] parts)
        {
            var code = parts[RegionCodePosition];
            if (!_regions.TryGetValue(code, out var region))
            {
                region = new Region { Code = code, Name = parts[RegionPosition] };
                OnRegionExtracted?.Invoke(region);
                _regions.Add(code, region);
            }
            return region;
        }

        private Subregion GetSubregion(string[] parts)
        {
            var code = parts[SubregionCodePosition];
            if (!_subregions.TryGetValue(code, out var subregion))
            {
                subregion = new Subregion
                {
                    Code = code,
                    Name = parts[SubregionPosition],
                    Region = GetRegion(parts)
                };
                OnSubregionExtracted?.Invoke(subregion);
                _subregions.Add(code, subregion);
            }
            return subregion;
        }
    }
}
