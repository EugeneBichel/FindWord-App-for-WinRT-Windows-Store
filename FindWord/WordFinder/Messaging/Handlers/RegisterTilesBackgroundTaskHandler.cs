using Windows.ApplicationModel.Background;
using WordFinder.Services;

namespace WordFinder.Messaging
{
    public class RegisterTilesBackgroundTaskHandler : IHandler<RegisterTilesBackgroundTaskMessage>
    {
        #region Fields

        private readonly IRegistrationBackgroundTaskService _backGrdTaskRegistrator;

        #endregion //Fields

        #region Constructor

        public RegisterTilesBackgroundTaskHandler(IRegistrationBackgroundTaskService backGrdTaskRegistrator)
        {
            _backGrdTaskRegistrator=backGrdTaskRegistrator;
        }

        #endregion //Constructor

        public void Handle(RegisterTilesBackgroundTaskMessage message)
        {
            var trigger = new SystemTrigger(SystemTriggerType.InternetAvailable, false);
            var condition = new SystemCondition(SystemConditionType.UserPresent);

            _backGrdTaskRegistrator.RegisterBackgroundTask(WordFinder.Common.BackgroundTasks.TilesBackgroundTaskEntryPoint,
                                   WordFinder.Common.BackgroundTasks.TilesBackgroundTaskName,
                                   trigger,
                                   condition);
        }
    }
}