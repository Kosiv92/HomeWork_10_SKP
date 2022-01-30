using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace HomeWork_10_SKP
{
    /// <summary>
    /// Статический класс с методами взаимодействия с репозиторием 
    /// </summary>
    public class Repository
    {
        //строка хранящая путь к репозиторию
        readonly string pathToRepository;

        DirectoryInfo _repoDirectory;

        /// <summary>
        /// Свойство доступа к директории репозитория
        /// </summary>
        public string PathToRepository
        {
            get { return pathToRepository; }
        }

        public Repository()
        {
            pathToRepository = Environment.CurrentDirectory + "\\repo\\";

            CreateRepoDirectory();
        }

        /// <summary>
        /// Метод создания директории для загрузки/скачивания и хранения файлов, в случае если такой директории не существует
        /// </summary>
        public void CreateRepoDirectory()
        {
            if (!CheckRepoDirectory())
            {
                Directory.CreateDirectory(pathToRepository);
            }

            _repoDirectory = new DirectoryInfo(pathToRepository);
        }
        
        /// <summary>
        /// Метод проверки существования директории для загрузки/скачивания и хранения файлов
        /// </summary>
        /// <returns>Результат проверки</returns>
        bool CheckRepoDirectory()
        {
            DirectoryInfo path = new DirectoryInfo(pathToRepository);
            if (path.Exists) return true;
            else return false;
        }

        /// <summary>
        /// Получение списка файлов хранящихся в репозитории
        /// </summary>
        /// <returns>Список файлов хранящихся в репозитории</returns>
        public FileInfo[] GetFilesName()
        {
            //FileInfo[] Files = repoDirectory.GetFiles("*.pdf");
            FileInfo[] files = _repoDirectory.GetFiles();
            string str = "";
            foreach (FileInfo file in files)
            {
                str = str + ", " + file.Name;
            }
            return files;
        }

        public string GetFileList()
        {
            FileInfo[] files = _repoDirectory.GetFiles();
            StringBuilder fileList = new StringBuilder();
            foreach (var file in files)
            {
                fileList.Append($"- {file.Name}\n");
            }

            return fileList.ToString();

        }

    }
}
