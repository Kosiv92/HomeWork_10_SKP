namespace HomeWork_10_SKP
{
    /// <summary>
    /// Класс хранящий состояние взаимодействия с пользователем в чает
    /// </summary>
    public class ClientState
    {
        
        public ClientState()
        {
            isWeatherSearchOn = false;

            isFileSendOn = false;
        }

        #region Поля
        /// <summary>
        /// Состояние поиска погоды
        /// </summary>
        public bool isWeatherSearchOn;

        /// <summary>
        /// Состояние загрузки файлов
        /// </summary>
        public bool isFileSendOn;
        #endregion
    }
}