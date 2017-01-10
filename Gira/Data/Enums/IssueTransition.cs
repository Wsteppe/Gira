namespace Gira.Data.Enums
{
    public enum IssueTransition
    {
        Assign = 1, // to solver
        Solve,
        Close,
        Cancel,
        Enquire,
        Refuse
    }
}