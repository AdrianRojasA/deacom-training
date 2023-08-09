using System.Data.Common;

namespace DeacomTraining.BusinessClasses.Interfaces
{
    //-- this interface should maintain its current method, no more or less
    public interface IDBAccess<T>
    {
        public T GetOne(DbConnection conn, int id);
    }

}
