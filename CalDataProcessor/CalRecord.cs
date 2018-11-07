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
        public DataTable ParameterTable { get; private set; }
        public DataTable ValuesTable { get; private set; }
        public string SampleType { get; private set; }
        public float BinderMass { get; private set; }


        public CalRecord(string filename)
        {
            // Create the DataTable of parameters and values, and set an empty SampleType
            this.ParameterTable = GenerateParameterTable(filename);
            this.ValuesTable = GenerateValuesTable(filename);
            this.SampleType = "";

            this.CheckSampleType(filename);
            this.BinderMass = this.CalculateBinderMass();
        }

        private float CalculateBinderMass()
        {
            float sampleMass = float.TryParse(ParameterTable.Rows[0]["Sample Mass, g"].ToString(), out sampleMass) ? sampleMass : 0;
            float cementMass = float.TryParse(ParameterTable.Rows[0]["Cement Mass, g"].ToString(), out cementMass) ? cementMass : 0;
            float slagMass = float.TryParse(ParameterTable.Rows[0]["Suppl 1 Mass, g"].ToString(), out slagMass) ? slagMass : 0;
            float flyashMass = float.TryParse(ParameterTable.Rows[0]["Suppl 2 Mass, g"].ToString(), out flyashMass) ? flyashMass : 0;
            float waterMass = float.TryParse(ParameterTable.Rows[0]["Water Mass, g"].ToString(), out waterMass) ? waterMass : 0;
            float aggregateMass = float.TryParse(ParameterTable.Rows[0]["Aggr Mass, g"].ToString(), out aggregateMass) ? aggregateMass : 0;

            float binderMass = (cementMass + slagMass + flyashMass) / 
                (cementMass + slagMass + flyashMass + waterMass + aggregateMass) *
                sampleMass;

            return binderMass;
        }

        private void CheckSampleType(string filename)
        {
            // Get the start of the file name, which is the sample name
            // EFC sample names should start with full year i.e. 2018
            // OPC sample names should start with abbreviated year i.e. 18
            string yearStart = Path.GetFileNameWithoutExtension(filename).Substring(0, 2);

            // Get the actual year to compare the sample name to
            string year = DateTime.Now.Year.ToString();

            // Dictionary that equates sample name to sample type 
            Dictionary<string, string> sampleTypeOptions = new Dictionary<string, string>()
            {
                // "20"
                { year.Substring(0,2), "EFC" },
                // currently "18"
                { year.Substring(2,2), "OPC" }
            };

            // Check that the sample name actually starts with one of the options "20" or "18" (currently)
            if (sampleTypeOptions.ContainsKey(yearStart))
            {
                this.SampleType = sampleTypeOptions[yearStart];
            }
            // Otherwise create a dialog requesting sample type input
            else
            {
                SampleTypeDialog check = new SampleTypeDialog(Path.GetFileNameWithoutExtension(filename));

                Nullable<bool> result = check.ShowDialog();

                if (result == true)
                {
                    this.SampleType = check.SampleType;
                }
            }
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

            // Read through CSV until values
            string line;
            while ((line = reader.ReadLine()) != "") { }

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
