using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalDataProcessor
{
    class CalRecordRepository
    {
        // Create dictionary of tables to store values for all samples
        public Dictionary<string, DataTable> CalValuesDictionary { get; set; }
        // Create table to store parameters from all samples
        public DataTable ParameterTable { get; set; }

        public CalRecordRepository(string[] filenames)
        {

            CalValuesDictionary = new Dictionary<string, DataTable>();

            // For each of the TSV calorimetry files
            foreach (string filename in filenames)
            {
                // Create a record that organises values and parameters
                CalRecord record = new CalRecord(filename);

                // Copy the structure of the first parameter table
                if (ParameterTable == null)
                {
                    ParameterTable = record.ParameterTable.Clone();
                }

                // Copy the single row from the record table into the combined repository table
                foreach (DataRow row in record.ParameterTable.Rows)
                {
                    ParameterTable.Rows.Add(row.ItemArray);
                }

                // Store values in dictionary with Sample ID as key
                CalValuesDictionary[record.ParameterTable.Rows[0]["Sample ID"].ToString()] = record.ValuesTable;
            }

        }
    }
}
