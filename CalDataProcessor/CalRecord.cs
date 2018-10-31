using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalDataProcessor
{
    class CalRecord
    {
        public DataTable ParameterTable { get; set; }
        public DataTable ValuesTable { get; set; }

        public CalRecord(string filename)
        {
            this.ParameterTable = GenerateParameterTable(filename);
            this.ValuesTable = GenerateValuesTable(filename);
            
        }

        private DataTable GenerateParameterTable(string filename)
        {
            DataTable parameterTable = new DataTable();
            parameterTable.Columns.Add("Name");
            parameterTable.Columns.Add("Value");

            StreamReader reader = new StreamReader(filename);
            char[] delimeter = new char[] { '\t' };

            // Read parameter lines at top of file until empty string (newline in file separates
            // parameters from values)
            string line;
            while ((line = reader.ReadLine()) != "")
            {
                DataRow row = parameterTable.NewRow();
                row.ItemArray = line.Split(delimeter);
                parameterTable.Rows.Add(row);
            }

            // Transpose parameters table so parameter names are headers
            parameterTable = GenerateTransposedTable(parameterTable);
            parameterTable.Columns.Remove("Name");

            return parameterTable;
        }

        private DataTable GenerateValuesTable(string filename)
        {
            DataTable valuesTable = new DataTable();

            StreamReader reader = new StreamReader(filename);
            char[] delimeter = new char[] { '\t' };

            string line;
            while ((line = reader.ReadLine()) != "")
            {

            }

            // Read headers into values table
            string[] columnHeaders = reader.ReadLine().Split(delimeter);
            foreach (string columnHeader in columnHeaders)
            {
                valuesTable.Columns.Add(columnHeader);
            }

            // Read empty line between headers and values
            reader.ReadLine();

            // Read values into table until end of file.
            while ((line = reader.ReadLine()) != null)
            {
                DataRow row = valuesTable.NewRow();
                row.ItemArray = line.Split(delimeter);
                valuesTable.Rows.Add(row);
            }

            return valuesTable;
        }

        private DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }

    }
}
