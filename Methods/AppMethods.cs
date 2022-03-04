using Covid_Api.Data;
using Covid_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Covid_Api.Methods
{
    public class AppMethods
    {


        public static void LogDb(Log log, CovidAppContext context)
        {
            context.logs.Add(log);
            context.SaveChanges();
        }
    }
}