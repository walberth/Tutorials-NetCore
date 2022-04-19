namespace Loans.Domain.Applications
{
    public interface ICreditScorer
    {
        int Score { get; }

        void CalculateScore(string applicantName, string applicantAddress);

        ScoreResult ScoreResult { get; }

        int Count { get; set; }
    }
}