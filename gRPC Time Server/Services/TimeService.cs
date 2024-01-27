using Grpc.Core;
using gRPC_Time_Server.Attributes;

namespace gRPC_Time_Server.Services
{
    public class TimeService: gRPC_Time_Server.TimeService.TimeServiceBase
    {
        #region Props
        private readonly ILogger<TimeService> _logger;

        // Used for simple fileDB
        private string _file;
        private string _desktopPath;
        private string _filePath;
        #endregion

        #region Methods

        #region public
        public TimeService(ILogger<TimeService> logger)
        {
            _logger = logger;

            _file = "times.txt";

            // Get the path to the desktop
            _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Combine the desktop path with the file name
            _filePath = Path.Combine(_desktopPath, _file);
        }

        public override Task<TimeResponse> GetCurrentTime(TimeRequest request, ServerCallContext context)
        {
            var currentTime = System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

            _logger.LogInformation($"Current time {currentTime} is saved!");

            LogTimeInFileDB(currentTime);

            return Task.FromResult(new TimeResponse { CurrentTime = "LoggedTime in DB." });
        }

        [RequireHttps]
        public override Task<QueryResponse> QueryTimeDatabase(Certificate certificate, ServerCallContext context)
        {
            var timeLogs = GetTimeLogs();

            if(timeLogs.Count <= 0)
                return Task.FromResult(new QueryResponse { TimeLogs = { "Storage File not found!" } });

            // If timelogs are not empty, we have at least 1 entry.
            return Task.FromResult(new QueryResponse { TimeLogs = { timeLogs } });
        }
        #endregion

        #region private
        // Write the logs to a custom file.
        private void LogTimeInFileDB(string currentTime)
        {
            // Check if the file exists
            if (!File.Exists(_filePath))
            {
                // If the file doesn't exist, create it
                using (StreamWriter sw = File.CreateText(_filePath))
                {
                    // Write the current time to the file
                    sw.WriteLine(currentTime);
                }
            }

            else
            {
                // If the file exists, append the current time to it
                using (StreamWriter sw = File.AppendText(_filePath))
                {
                    // Write the current time to the file
                    sw.WriteLine(currentTime);
                }
            }
        }

        // Retrieve the logs from the file.
        private List<string> GetTimeLogs()
        {
            if (!File.Exists(_filePath))
            {
                _logger.LogCritical("No DB file found on system!");

                return new List<string>() { };
            }

            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(_filePath);

                // Convert the array to a List<string> and return
                return lines.ToList();
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log, throw, etc.)
                _logger.LogError($"Error reading time logs from file: {ex.Message}");
                return new List<string>();
            }
        }
        #endregion
        #endregion
    }
}