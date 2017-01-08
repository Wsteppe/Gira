namespace Gira.Data.Enums
{
    public enum IssueTransition
    {
        Assign = 1, // to dispatcher
        Treat, // to solver
        Solve,
        Close,
        Cancel,
        Enquire,
        Refuse
    }
}