using Inventory.Data;
using Inventory.Properties;
using Microsoft.Maui.Storage;
using System;
using Windows.Storage;

namespace Inventory.Service
{
    public class HelperService
    {
        private readonly DatabaseContext _context;
        public HelperService(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        public async Task<(bool success, string message)> DownloadDb()
        {
            try
            {
                var dbLocation = _context.GetDbLocation();
                File.Copy(dbLocation, $"{Resource.DownloadLocation}\\{Resource.DatabaseName}", true);
                return (true, $"Downloaded to Location: {Resource.DownloadLocation}{Resource.DatabaseName}");
            }
            catch (Exception e)
            {
                return (false, $"Error uploading database file: {e.Message}");
            }
        }

        public async Task<(bool success, string message)> UploadSnapshotDb()
        {
            try {
                var file = await FilePicker.PickAsync();

                if (file == null) 
                    return (false, $"Please select a file");

                if (Path.GetExtension(file.FullPath) != ".db")
                   return (false, $"Only .db extension files are compatible");

                var dbLocation = _context.GetDbLocation();
                File.Copy(file.FullPath, dbLocation, true);
                return (true, $"{file.FileName} uploaded successfully");
            }

            catch (Exception e)
            {
                return (false, $"Error uploading database file: {e.Message}");
            }

        }
    }
}
