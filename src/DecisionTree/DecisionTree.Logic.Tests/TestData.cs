namespace DecisionTree.Logic.Tests
{
    public class TestData
    {
        public const string TestCsvContent = @"
Outlook;Temperatur;Humidity;Wind;NeedUmbrella;
Data
Sunny;Hot;High;Weak;Yes;
Sunny;Hot;High;Strong;Yes;
";

        public const string TestCsvInvalidContentDataMissing = @"
Outlook;Temperatur;Humidity;Wind;NeedUmbrella;
Sunny;Hot;High;Weak;Yes;
Sunny;Hot;High;Strong;Yes;
";
    }
}
