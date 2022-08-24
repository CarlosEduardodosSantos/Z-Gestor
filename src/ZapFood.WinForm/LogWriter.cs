using System;
using System.IO;
using System.Reflection;

namespace ZapFood.WinForm
{
    public class LogWriter
    {
        private string m_exePath = string.Empty;

        public void LogWrite(string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                if (!Directory.Exists($"{m_exePath}\\Log\\" ))
{
                    Directory.CreateDirectory($"{m_exePath}\\Log\\");
                }
                var path = $"{m_exePath}\\Log\\logPDV_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}.txt";
                if (!File.Exists(path))
                {
                    File.Create(path);


                }
                using (var file = File.Open(path, FileMode.Append, FileAccess.Write))
                using (var writer = new StreamWriter(file))
                {
                    Log(logMessage, writer);
                }
            }
            catch(Exception ex)
            {
                //ignore
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Erro : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}