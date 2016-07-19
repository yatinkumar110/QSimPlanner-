﻿using QSP.AviationTools;
using QSP.Utilities.Units;
using QSP.WindAloft;
using System.Linq;
using System.Text.RegularExpressions;

namespace QSP.Metar
{
    public static class ParaExtractor
    {
        /// <summary>
        /// E.g. matches "VRB", "02013KT", "020/13KT", "02013G20KT"
        /// or "14007MPS".
        /// If not found, returns null.
        /// </summary>
        public static Wind? GetWind(string metar)
        {
            if (Regex.Match(metar,
                @"\bVRB\d{1,3}(KTS?|MPS)\b").Success)
            {
                return new Wind(0.0, 0.0);
            }

            var match = Regex.Match(metar,
                @"(^|\s)\d{3}/?\d{1,3}(G\d{1,3})?(KTS?|MPS)($|\s)",
                RegexOptions.Multiline);

            if (match.Success)
            {
                var val = match.Value.Trim();
                int direction = int.Parse(val.Substring(0, 3));
                double speed = int.Parse(
                    Regex.Match(val.Substring(3), @"^/?(?<wind>\d{1,3})")
                    .Groups["wind"].Value);

                if (val.Contains("MPS"))
                {
                    speed /= AviationTools.Constants.KnotMpsRatio;
                }
                return new Wind(direction, speed);
            }

            return null;
        }

        /// <summary>
        /// Matches 12/09, 01/M01, M04/M06, etc.
        /// Returns null if no match is found.
        /// </summary>
        public static int? GetTemp(string metar)
        {
            var match = Regex.Match(
                metar, @"(^|\s)(?<temp>M?\d{1,3})/M?\d{1,3}($|\s)",
                RegexOptions.Multiline);

            if (match.Success)
            {
                var val = match.Groups["temp"].Value;

                if (val[0] == 'M')
                {
                    return -int.Parse(val.Substring(1));
                }
                else
                {
                    return int.Parse(val);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Matches Q1013, A3000.
        /// </summary>
        public static PressureSetting GetPressure(string metar)
        {
            var match = Regex.Match(
                metar, @"(^|\s)[AQ]\d{4}($|\s)", RegexOptions.Multiline);

            if (match.Success)
            {
                var val = match.Value.Trim();
                var unit = val.Contains("A") ?
                    PressureUnit.inHg : PressureUnit.Mb;

                return new PressureSetting(unit,
                    double.Parse(val.Substring(1, 4)) *
                    (unit == PressureUnit.inHg ? 0.01 : 1.0));
            }

            return null;
        }

        public static bool PrecipitationExists(string metar)
        {
            var wordList = new string[]
            {
                "DZ", // Drizzle
                "RA", // Rain
                "SN", // Snow
                "SG", // Snow Grains
                "IC", // Ice Crystals
                "PL", // Ice Pellets
                "GR", // Hail
                "GS", // Small Hail and/or Snow Pellets
                "UP", // Unknown Precipitation
            };

            return wordList.Any(w =>
                Regex.Match(
                    metar,
                    @"(^|\s)[+-]?" + w + @"($|\s)",
                    RegexOptions.Multiline)
                .Success);
        }

        public class PressureSetting
        {
            public PressureUnit PressUnit { get; private set; }
            public double Value { get; private set; }

            public PressureSetting(PressureUnit PressUnit, double Value)
            {
                this.PressUnit = PressUnit;
                this.Value = Value;
            }
        }
    }
}
