﻿using QSP.LandingPerfCalculation.Boeing;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QSP.LandingPerfCalculation
{
    public class LdgTableLoader
    {
        public const string DefaultFolderPath = @"PerformanceData\LDG";
        private string folderPath;        

        public LdgTableLoader(string folderPath = DefaultFolderPath)
        {
            this.folderPath = folderPath;
        }

        /// <summary>
        /// Load all xml in the landing performance data folder.
        /// Files in wrong format are ignored.
        /// Files containing the same profile name are not loaded and
        /// a message will be included in returning value.
        /// </summary>
        public TableImportResult Load()
        {
            var tables = new List<PerfTable>();

            foreach (var i in Directory.GetFiles(folderPath))
            {
                try
                {
                    tables.Add(new PerfDataLoader().ReadFromXml(i));
                }
                catch (Exception ex)
                {
                    LoggerInstance.WriteToLog(ex);
                }
            }

            var groups = tables.GroupBy(x => x.Entry.ProfileName);
            var result = groups
                .Where(g => g.Count() == 1)
                .Select(g => g.First())
                .ToList();

            return new TableImportResult(result, Message(tables));
        }

        private static string Message(List<PerfTable> item)
        {
            var groups = item.GroupBy(x => x.Entry.ProfileName);

            try
            {
                var duplicate = groups.First(g => g.Count() > 1);

                return
                    "The following aircrafts have" +
                    " identical profile names:\n\n" +
                    string.Join("\n", duplicate.Select(x => x.Entry.FilePath)) +
                    "\n\nNone of these profiles will be loaded.";
            }
            catch (InvalidOperationException)
            {
                // There is no duplicate.
                return null;
            }
        }

        public class TableImportResult
        {
            public List<PerfTable> Tables { get; private set; }
            public string Message { get; private set; }

            public TableImportResult(List<PerfTable> Tables, string Message)
            {
                this.Tables = Tables;
                this.Message = Message;
            }
        }
    }
}
