using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordFinder.Services;
using WordFinder.Tiles;

namespace WordFinder.Messaging
{
    public class UpdateTileHandler : IAsyncHandler<UpdateTileMessage>
    {
        private AppState _appState;

        public UpdateTileHandler(AppState appState)
        {
            _appState = appState;
        }

        public async Task HandleAsync(UpdateTileMessage message)
        {
            try
            {
                TileManager tileMananger = new TileManager(true);

                var words = await GetWords();

                tileMananger.Update(words);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<IList<string>> GetWords()
        {
            return new List<string>();
        }
    }
}