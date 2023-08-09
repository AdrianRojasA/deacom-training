using System.Data.Common;
using DeacomTraining.Data;
using DeacomTraining.Data.Enums;
using DeacomTraining.Data.Factories;

namespace DeacomTraining.Service
{
    public class MainService
    {
        public static MemoryContext memoryContext = MemoryContext.GetInstance();

        public DbConnection connection = memoryContext.GetConnection();
    }
}
