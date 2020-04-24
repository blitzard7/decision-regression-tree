namespace DecisionTree.Logic.Validator
{
    public interface IValidator
    {
        bool IsMetaInformationFormatValid(string input);
        bool IsRowFormatValid(string[] rows);
        bool IsColumnFormatValid(string input);
        bool IsHeaderPresent(string input);
        bool IsDataPresent(string input);
        bool AreAmountOfRowValuesWithColumnEntriesEqual(string[] column, string[] rows);
    }
}
