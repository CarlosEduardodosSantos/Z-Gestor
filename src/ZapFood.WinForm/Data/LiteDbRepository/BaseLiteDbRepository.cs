using System;
using LiteDB;

namespace ZapFood.WinForm.Data.LiteDbRepository
{
    public class BaseLiteDbRepository : IDisposable
    {
        private string DbFile
        {
            get { return $"{Environment.CurrentDirectory}\\zFoodGestor.db"; }
        }
        public LiteDatabase Connection
        {
            get { return new LiteDatabase($"Filename={DbFile}"); }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}