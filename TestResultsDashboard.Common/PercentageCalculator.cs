namespace TestResultsDashboard.Common;

public static class PercentageCalculator
{
    public static string Calculate(int part, int total)
    {
        return ((double)part / total).ToString("P");
    }
}