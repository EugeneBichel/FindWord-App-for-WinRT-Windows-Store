using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace FindWord.Services
{
    public sealed class SessionStateService
    {
        private const string filename = "session.xml";

        public async Task<SessionState> LoadState()
        {
            // Get the input stream for the SessionState file.
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                if (file == null) return null;

                using (IInputStream inStream = await file.OpenSequentialReadAsync())
                {
                    // Deserialize the Session State.
                    DataContractSerializer serializer = new DataContractSerializer(typeof(SessionState));

                    using (Stream stream = inStream.AsStreamForRead())
                    {
                        SessionState state = (SessionState)serializer.ReadObject(stream);

                        if (!state.IsValid())
                            return null;

                        return state;
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(ex.StackTrace);
                // Restoring state is best-effort.  If it fails, the app will just come up with a new session.
            }

            return null;
        }

        public async Task SaveState(SessionState state)
        {
            if (state == null || !state.IsValid())
            {
                Debug.WriteLine("state is not valid in SaveState(SessionState state).");
                return;
            }

            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

                using (IRandomAccessStream raStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
                    {
                        // Serialize the Session State.
                        DataContractSerializer serializer = new DataContractSerializer(typeof(SessionState));

                        using (Stream stream = outStream.AsStreamForWrite())
                        {
                            serializer.WriteObject(stream, state);
                            await outStream.FlushAsync();
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Debug.WriteLine(e);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
