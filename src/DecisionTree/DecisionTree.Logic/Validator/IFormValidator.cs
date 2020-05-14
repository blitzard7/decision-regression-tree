namespace DecisionTree.Logic.Validator
{
    public interface IFormValidator
    {
        bool IsMetaInformationFormatValid(string input);
        bool IsRowFormatValid(string[] rows);
        bool IsRowFormatValid(string row);
        bool IsColumnFormatValid(string input);
        bool IsDataSeparatedCorrectly(string data);
        bool IsHeaderPresent(string input);
        bool IsDataPresent(string input);
        bool AreAmountOfRowValuesWithColumnEntriesEqual(string[] column, string[] rows);
    }
}
