using System;
using System.IO;
using System.Threading;

namespace Watcher
{
    internal static class Program
    {
        private static object executeOnChanged = null;
        private static object executeExecutar = null;

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { @"..\..\..\teste\teste.txt" };
            }

            IniciarAnalise(args[0]);
        }

        public static void IniciarAnalise(string path)
        {
            if (File.Exists(path) && Path.GetExtension(path).Equals(".txt"))
            {
                var dir = Path.GetDirectoryName(path);
                var file = Path.GetFileName(path);

                Executar(path);

                using FileSystemWatcher watcher = new(dir, file);

                watcher.NotifyFilter = NotifyFilters.LastWrite;

                watcher.Changed += OnChanged;

                watcher.EnableRaisingEvents = true;
                while (true) ;
            }
            else
            {
                Console.WriteLine($"Selecione um arquivo válido!");
                Console.WriteLine($"O caminho especificado {path} não existe, ou não é um arquivo .txt");
            }
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            if (executeOnChanged is null)
            {
                Executar(e.FullPath);
                executeOnChanged = source;
            }
            else
            {
                executeOnChanged = null;
            }
        }

        private static void Executar(string path)
        {
            try
            {
                if (executeExecutar is null)
                {
                    executeExecutar = new();
                    Analisar(path);
                }
            }
            catch (IOException)
            {
                Thread.Sleep(500);
                executeExecutar = null;
                Executar(path);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Informe este erro ao desenvolvedor: {e.Message}");
            }
            finally
            {
                executeExecutar = null;
            }
        }

        private static void Analisar(string path)
        {
            Console.Clear();
            Console.WriteLine("Pressione ctrl + c para sair.\n");
            Console.WriteLine($"{DateTime.Now:HH:mm:ss}\nAnalisando o arquivo {path}");
            //aqui vai o código
        }
    }
}