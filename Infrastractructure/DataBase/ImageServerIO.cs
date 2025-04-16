using RestaurantLibrary.Database;
using RestaurantLibrary.Helpers;
using RestSharp;

namespace RestaurantLibrary.DataBase
{
    public static class ImageServerIO
    {
        public static string photosFolderUri = DataSQL.URL + @"/photos";
        private static readonly RestClient client = new RestClient(photosFolderUri);

        public static void SaveImageToServer(byte[] imageBytes, string imageName, string format = "jpeg")
        {
            if (imageBytes == null || string.IsNullOrEmpty(imageName))
                return;

            try
            {
                imageName = TransliterationHelper.CyrillicToLatin(imageName);
                var req = new RestRequest("/upload_image.php")
                    .AddFile("image", imageBytes, $"{imageName}.{format}", $"image/{format}");

                var res = client.Post(req);
            }
            catch (Exception ex)
            {
                UserNotifier.Notifier?.ShowError("Ошибка при отправке изображения: " + ex.Message);
            }
        }

        public static byte[] DownloadImageBytes(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return null;

            try
            {
                imageName = TransliterationHelper.CyrillicToLatin(imageName);
                string fullImagePath = $"{photosFolderUri}/images/{imageName}";
                var client = new RestClient(fullImagePath);
                var request = new RestRequest();

                return client.DownloadData(request);
            }
            catch (Exception ex)
            {
                UserNotifier.Notifier?.ShowError("Ошибка при загрузке изображения: " + ex.Message);
                return null;
            }
        }
    }
}
