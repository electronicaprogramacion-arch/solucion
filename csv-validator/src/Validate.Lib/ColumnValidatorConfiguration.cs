
namespace FormatValidator
{
    public class ColumnValidatorConfiguration
    {
        public string Name { get; set; }

        public bool Unique { get; set; }

        public int MaxLength { get; set; }

        public string Pattern { get; set; }

        public bool IsNumeric { get; set; }

        public bool IsRequired { get; set; }

        public bool IsOnlyRead { get; set; }

        public string Formula { get; set; }

        public string DefaultValue { get; set; }

        public string SelectOptions { get; set; }

        public bool IsBoolean { get; set; }

        public bool IsReportView { get; set; }

        public bool IsHeader { get; set; }

        public string FormulaClass { get; set; }

    }
}
