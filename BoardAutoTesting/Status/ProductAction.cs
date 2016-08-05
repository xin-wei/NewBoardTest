using System.Collections.Generic;

namespace BoardAutoTesting.Status
{
    public enum ProductAction
    {
        OnLine = 0,
        Testing,
        EndTest
    }

    public enum ProductStatus
    {
        UnKnown = 0,
        Pass,
        Fail
    }

    public class AllRoutes
    {
        public static List<string> LstRoutes = new List<string>(); 
    }
}