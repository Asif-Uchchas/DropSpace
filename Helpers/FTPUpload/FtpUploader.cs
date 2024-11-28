using System;
using System.IO;
using System.Net;

public class FtpUploader
{
    public static void UploadFile(string ftpUrl, string filePath, string username, string password)
    {
        var fileName = Path.GetFileName(filePath);
        var ftpFullPath = $"{ftpUrl}/{fileName}";

        try
        {
            // Create an FTP request
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFullPath);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Set the credentials
            request.Credentials = new NetworkCredential(username, password);

            // Read the file's contents into a byte array
            byte[] fileContents;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fileContents = new byte[fileStream.Length];
                fileStream.Read(fileContents, 0, fileContents.Length);
            }

            // Write the file contents to the request stream
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            // Get the response
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
