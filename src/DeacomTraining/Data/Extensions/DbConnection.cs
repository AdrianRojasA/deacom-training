using System.Data.Common;


namespace DeacomTraining.Data.Extensions
{
    public static class DbConnectionExtensions
    {
        public static void AddParameterWithValue(this DbCommand command, string parameterName, object parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            command.Parameters.Add(parameter);
        }
    }
}

