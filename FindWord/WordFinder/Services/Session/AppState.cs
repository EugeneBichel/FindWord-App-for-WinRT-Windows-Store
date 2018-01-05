using System.Threading.Tasks;

namespace FindWord.Services
{
    public class AppState
    {
        private static SessionStateService _sessionPersistenceService = new SessionStateService();

        public AppState()
            : base()
        { }

        public AppState(SessionState sessionState)
            : base()
        {
            SessionState = sessionState;

        }
        
        public SessionState SessionState { get; set; }

        #region Public Methods

        public static async Task<AppState> LoadStateAsync()
        {
            var appState = new AppState();

            appState.SessionState = await _sessionPersistenceService.LoadState();

            if (appState.SessionState == null)
                appState.SessionState = new SessionState();

            return appState;
        }

        public async Task SaveSessionStateAsync()
        {
            if (SessionState.IsValid())
                await _sessionPersistenceService.SaveState(SessionState);
        }

        public async Task SaveStateAsync()
        {
            await SaveSessionStateAsync();
        }

        public bool IsValid()
        {
            if (SessionState == null)
                return false;

            if (SessionState.IsValid() == true)
                return true;

            return false;
        }

        #endregion //Public Methods
    }
}