using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace FindWord
{
    public static class Utilities
    {
        public static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public static void ThreadSleep(TimeSpan timeSpan)
        {
            new System.Threading.ManualResetEvent(false).WaitOne(timeSpan);
        }

        public static async Task<T> LoadState<T>(string fileName)
        {
            try
            {
                var storageFolder = ApplicationData.Current.LocalFolder;

                var isFileExist = await IsFileExist(storageFolder, fileName);

                if (isFileExist == false)
                    return default(T);

                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                if (file == null) return default(T);

                using (IInputStream inStream = await file.OpenSequentialReadAsync())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));

                    using (Stream stream = inStream.AsStreamForRead())
                    {
                        var state = (T)serializer.ReadObject(stream);

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

            return default(T);

        }

        public static async Task SaveState<T>(T state, string fileName)
        {
            if (state == null)
                return;

            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (IRandomAccessStream raStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));

                    using (Stream stream = outStream.AsStreamForWrite())
                    {
                        serializer.WriteObject(stream, state);
                        await outStream.FlushAsync();
                    }
                }
            }
        }

        private static async Task<bool> IsFileExist(StorageFolder folder, string fileName)
        {
            try
            {
                await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public enum CharContainerOrder
        {
            First = 1,
            Last = 2,
            Other = 3
        }
    }
}